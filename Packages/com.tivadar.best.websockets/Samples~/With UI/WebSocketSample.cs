using System;

using Best.WebSockets.Examples.Helpers;

using UnityEngine;
using UnityEngine.UI;

namespace Best.WebSockets.Examples
{
    class WebSocketSample : SampleBase
    {
#pragma warning disable 0649

        [SerializeField]
        [Tooltip("The WebSocket address to connect")]
        private string address = "wss://besthttpwebgldemo.azurewebsites.net/ws";

        [SerializeField]
        private InputField _input;

        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private RectTransform _contentRoot;

        [SerializeField]
        private TextListItem _listItemPrefab;

        [SerializeField]
        private int _maxListItemEntries = 100;

        [SerializeField]
        private Button _connectButton;

        [SerializeField]
        private Button _closeButton;

#pragma warning restore

        /// <summary>
        /// Saved WebSocket instance
        /// </summary>
        WebSocket webSocket;

        protected override void Start()
        {
            base.Start();

            SetButtons(true, false);
            this._input.interactable = false;
        }

        void OnDestroy()
        {
            if (this.webSocket != null)
            {
                this.webSocket.Close();
                this.webSocket = null;
            }
        }

        public void OnConnectButton()
        {
            // Create the WebSocket instance
            this.webSocket = new WebSocket(new Uri(address));

            // Subscribe to the WS events
            this.webSocket.OnOpen += OnOpen;
            this.webSocket.OnMessage += OnMessageReceived;
            this.webSocket.OnClosed += OnClosed;

            // Start connecting to the server
            this.webSocket.Open();

            AddText("Connecting...");

            SetButtons(false, true);
            this._input.interactable = false;
        }

        public void OnCloseButton()
        {
            AddText("Closing!");

            // Close the connection
            this.webSocket.Close(WebSocketStatusCodes.NormalClosure, "Bye!");

            SetButtons(false, false);
            this._input.interactable = false;
        }
       
        public void OnInputField(string textToSend)
        {
            if ((!Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKeyDown(KeyCode.Return)) || string.IsNullOrEmpty(textToSend))
                return;

            AddText(string.Format("Sending message: <color=green>{0}</color>", textToSend))
                .AddLeftPadding(20);

            // Send message to the server
            this.webSocket.Send(textToSend);
        }

        #region WebSocket Event Handlers

        /// <summary>
        /// Called when the web socket is open, and we are ready to send and receive data
        /// </summary>
        void OnOpen(WebSocket ws)
        {
            AddText("WebSocket Open!");

            this._input.interactable = true;
        }

        /// <summary>
        /// Called when we received a text message from the server
        /// </summary>
        void OnMessageReceived(WebSocket ws, string message)
        {
            AddText(string.Format("Message received: <color=yellow>{0}</color>", message))
                .AddLeftPadding(20);
        }

        /// <summary>
        /// Called when the web socket closed
        /// </summary>
        void OnClosed(WebSocket ws, WebSocketStatusCodes code, string message)
        {
            AddText(string.Format("WebSocket closed! Code: {0} Message: {1}", code, message));

            webSocket = null;

            SetButtons(true, false);
        }

        #endregion

        private void SetButtons(bool connect, bool close)
        {
            if (this._connectButton != null)
                this._connectButton.interactable = connect;

            if (this._closeButton != null)
                this._closeButton.interactable = close;
        }

        private TextListItem AddText(string text)
        {
            return GUIHelper.AddText(this._listItemPrefab, this._contentRoot, text, this._maxListItemEntries, this._scrollRect);
        }
    }
}