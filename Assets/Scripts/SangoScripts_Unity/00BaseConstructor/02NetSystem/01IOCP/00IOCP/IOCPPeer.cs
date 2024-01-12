using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SangoScripts_Unity.Net
{
    public class IOCPPeer<T> where T : IClientPeer_IOCP, new()
    {
        private Socket _socket;
        private SocketAsyncEventArgs _socketAsyncEventArgs;

        public IOCPPeer()
        {
            _socketAsyncEventArgs = new SocketAsyncEventArgs();
            _socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
        }

        #region Client
        private T _clientPeer;
        public T ClientPeer
        {
            get
            {
                return _clientPeer;
            }
        }

        public void InitAsClient(string ip, int port)
        {
            IOCPLogger.Start("IOCP ClientPeer Init as Client, hello to the world.");
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socketAsyncEventArgs.RemoteEndPoint = point;
            OnConnect();
        }

        private void OnConnect()
        {
            if (_socket != null)
            {
                bool isConnetWaiting = _socket.ConnectAsync(_socketAsyncEventArgs);
                if (isConnetWaiting == false)
                {
                    ProcessConnect();
                }
            }
        }

        private void ProcessConnect()
        {
            if (_socket != null)
            {
                _clientPeer = new T();
                _clientPeer.InitClientPeer(_socket);
            }
        }

        public void CloseClient()
        {
            if (_clientPeer != null)
            {
                _clientPeer.OnClientClose();
                _clientPeer = null;
            }
            if (_socket != null)
            {
                _socket = null;
            }
        }
        #endregion

        #region Server
        private int _currentConnectCount = 0;
        private int _backLog = IOCPConfig.ServerBackLogCount;
        //private int _maxConnectCount = IOCPConfig.ServerMaxConnectCount;

        private Semaphore _acceptSeamaphore;
        private IOCPClientPeerPool<T> _peerPool;
        private List<T> _peerList;

        public void InitAsServer(string ip, int port, int maxConnectCount)
        {
            IOCPLogger.Start("IOCP ClientPeer Init as Server, hello to the world.");
            _currentConnectCount = 0;
            _acceptSeamaphore = new Semaphore(maxConnectCount, maxConnectCount);
            _peerPool = new IOCPClientPeerPool<T>(maxConnectCount);
            for (int i = 0; i < maxConnectCount; i++)
            {
                T peer = new T
                {
                    PeerId = i,
                };
                _peerPool.Push(peer);
            }
            _peerList = new List<T>();
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(point);
            _socket.Listen(_backLog);
            IOCPLogger.Done("IOCPServer is Init");
            OnAccept();
        }

        private void OnAccept()
        {
            if (_acceptSeamaphore != null && _socket != null)
            {
                _socketAsyncEventArgs.AcceptSocket = null;
                _acceptSeamaphore.WaitOne();
                bool isAcceptWaiting = _socket.AcceptAsync(_socketAsyncEventArgs);
                if (isAcceptWaiting == false)
                {
                    ProcessAccept();
                }
            }
            else
            {
                IOCPLogger.Error("IClientPeer Error: socket or seamaphore is null.");
            }
        }

        private void ProcessAccept()
        {
            if (_peerPool != null && _peerList != null)
            {
                Socket peerSocket = _socketAsyncEventArgs.AcceptSocket;
                if (peerSocket != null)
                {
                    Interlocked.Increment(ref _currentConnectCount);
                    T peer = _peerPool.Pop();
                    lock (_peerList)
                    {
                        _peerList.Add(peer);
                    }
                    peer.InitClientPeer(peerSocket);
                    peer.OnClientPeerCloseCallBack = RecycleClientPeerPool;
                    IOCPLogger.Done("Client Online, allocate ClientId:{0}", peer.PeerId);
                }
                OnAccept();
            }
            else
            {
                IOCPLogger.Error("IClientPeer Error: peerList or peerPool is null.");
            }
        }

        public void CloseServer()
        {
            if (_peerList != null)
            {
                for (int i = 0; i < _peerList.Count; i++)
                {
                    _peerList[i].OnClientClose();
                }
                _peerList.Clear();
                _peerList = null;
            }
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        private void RecycleClientPeerPool(int peerId)
        {
            if (_peerList != null && _peerPool != null && _acceptSeamaphore != null)
            {
                int index = -1;
                for (int i = 0; i < _peerList.Count; i++)
                {
                    if (_peerList[i].PeerId == peerId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                {
                    _peerPool.Push(_peerList[index]);
                    lock (_peerList)
                    {
                        _peerList.RemoveAt(index);
                    }
                    Interlocked.Decrement(ref _currentConnectCount);
                    _acceptSeamaphore.Release();
                }
                else
                {
                    IOCPLogger.Error("IClientPeer: {0} can`t find in server peerList.", peerId);
                }
            }
            else
            {
                IOCPLogger.Error("IClientPeer Error: peerList or peerPool or seamaphore is null.");
            }
        }

        public List<T> GetAllClientPeerList()
        {
            return _peerList;
        }
        #endregion

        private void OnIO_Completed(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            switch (socketAsyncEventArgs.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccept();
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnect();
                    break;
                default:
                    IOCPLogger.Warning("The last operation completed on the socket was not a accept or connect");
                    break;
            }
        }
    }
}