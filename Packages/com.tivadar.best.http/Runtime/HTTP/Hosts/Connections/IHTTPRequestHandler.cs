#if !UNITY_WEBGL || UNITY_EDITOR
using System;

using Best.HTTP.Shared;
using Best.HTTP.Shared.Logger;

namespace Best.HTTP.Hosts.Connections
{
    /// <summary>
    /// Common interface for implementations that will coordinate request processing inside a connection.
    /// </summary>
    public interface IHTTPRequestHandler : IDisposable
    {
        KeepAliveHeader KeepAlive { get; }

        bool CanProcessMultiple { get; }

        /// <summary>
        /// Number of assigned requests to process.
        /// </summary>
        int AssignedRequests { get; }

        /// <summary>
        /// Maximum number of assignable requests.
        /// </summary>
        int MaxAssignedRequests { get; }

        ShutdownTypes ShutdownType { get; }

        LoggingContext Context { get; }

        void Process(HTTPRequest request);

        void RunHandler();

        /// <summary>
        /// An immediate shutdown request that called only on application closure.
        /// </summary>
        void Shutdown(ShutdownTypes type);
    }
}

#endif
