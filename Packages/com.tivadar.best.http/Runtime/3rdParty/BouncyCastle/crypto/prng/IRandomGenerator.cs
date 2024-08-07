#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	/// <remarks>Generic interface for objects generating random bytes.</remarks>
	public interface IRandomGenerator
	{
		/// <summary>Add more seed material to the generator.</summary>
		/// <param name="seed">A byte array to be mixed into the generator's state.</param>
		void AddSeedMaterial(byte[] seed);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        void AddSeedMaterial(ReadOnlySpan<byte> seed);
#endif

        /// <summary>Add more seed material to the generator.</summary>
        /// <param name="seed">A long value to be mixed into the generator's state.</param>
        void AddSeedMaterial(long seed);

		/// <summary>Fill byte array with random values.</summary>
		/// <param name="bytes">Array to be filled.</param>
		void NextBytes(byte[] bytes);

		/// <summary>Fill byte array with random values.</summary>
		/// <param name="bytes">Array to receive bytes.</param>
		/// <param name="start">Index to start filling at.</param>
		/// <param name="len">Length of segment to fill.</param>
		void NextBytes(byte[] bytes, int start, int len);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
		void NextBytes(Span<byte> bytes);
#endif
	}
}
#pragma warning restore
#endif
