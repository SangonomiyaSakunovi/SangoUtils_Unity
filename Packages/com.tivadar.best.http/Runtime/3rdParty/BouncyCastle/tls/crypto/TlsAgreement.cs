#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Tls.Crypto
{
    /// <summary>Base interface for ephemeral key agreement calculator.</summary>
    public interface TlsAgreement
    {
        /// <summary>Generate an ephemeral key pair, returning the encoding of the public key.</summary>
        /// <returns>a byte encoding of the public key.</returns>
        /// <exception cref="IOException"/>
        byte[] GenerateEphemeral();

        /// <summary>Pass in the public key for the peer to the agreement calculator.</summary>
        /// <param name="peerValue">a byte encoding of the peer public key.</param>
        /// <exception cref="IOException"/>
        void ReceivePeerValue(byte[] peerValue);

        /// <summary>Calculate the agreed secret based on the calculator's current state.</summary>
        /// <returns>the calculated secret.</returns>
        /// <exception cref="IOException"/>
        TlsSecret CalculateSecret();
    }
}
#pragma warning restore
#endif
