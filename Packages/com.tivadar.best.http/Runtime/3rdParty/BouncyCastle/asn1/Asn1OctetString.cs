#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
    public abstract class Asn1OctetString
        : Asn1Object, Asn1OctetStringParser
    {
        internal class Meta : Asn1UniversalType
        {
            internal static readonly Asn1UniversalType Instance = new Meta();

            private Meta() : base(typeof(Asn1OctetString), Asn1Tags.OctetString) {}

            internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString)
            {
                return octetString;
            }

            internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence)
            {
                return sequence.ToAsn1OctetString();
            }
        }

        internal static readonly byte[] EmptyOctets = new byte[0];

        /**
         * return an Octet string from the given object.
         *
         * @param obj the object we want converted.
         * @exception ArgumentException if the object cannot be converted.
         */
        public static Asn1OctetString GetInstance(object obj)
        {
            if (obj == null)
                return null;

            if (obj is Asn1OctetString asn1OctetString)
                return asn1OctetString;

            if (obj is IAsn1Convertible asn1Convertible)
            {
                Asn1Object asn1Object = asn1Convertible.ToAsn1Object();
                if (asn1Object is Asn1OctetString converted)
                    return converted;
            }
            else if (obj is byte[] bytes)
            {
                try
                {
                    return (Asn1OctetString)Meta.Instance.FromByteArray(bytes);
                }
                catch (IOException e)
                {
                    throw new ArgumentException("failed to construct OCTET STRING from byte[]: " + e.Message);
                }
            }

            throw new ArgumentException("illegal object in GetInstance: " + Org.BouncyCastle.Utilities.Platform.GetTypeName(obj), "obj");
        }

        /**
         * return an octet string from a tagged object.
         *
         * @param taggedObject the tagged object holding the object we want.
         * @param declaredExplicit true if the object is meant to be explicitly tagged false otherwise.
         * @exception ArgumentException if the tagged object cannot be converted.
         */
        public static Asn1OctetString GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)
        {
            return (Asn1OctetString)Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);
        }

        internal readonly byte[] contents;

        /**
         * @param string the octets making up the octet string.
         */
        internal Asn1OctetString(byte[] contents)
        {
			if (null == contents)
				throw new ArgumentNullException("contents");

			this.contents = contents;
        }

        public Stream GetOctetStream()
		{
			return new MemoryStream(contents, false);
		}

		public Asn1OctetStringParser Parser
		{
			get { return this; }
		}

		public virtual byte[] GetOctets()
        {
            return contents;
        }

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(GetOctets());
        }

		protected override bool Asn1Equals(
			Asn1Object asn1Object)
		{
			DerOctetString other = asn1Object as DerOctetString;

			if (other == null)
				return false;

			return Arrays.AreEqual(GetOctets(), other.GetOctets());
		}

		public override string ToString()
		{
			return "#" + Hex.ToHexString(contents);
		}

        internal static Asn1OctetString CreatePrimitive(byte[] contents)
        {
            return new DerOctetString(contents);
        }
    }
}
#pragma warning restore
#endif
