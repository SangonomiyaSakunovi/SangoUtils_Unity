#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.Collections.Generic;

using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	/**
	 * Basic generator that just returns a preconstructed attribute table
	 */
	public class SimpleAttributeTableGenerator
		: CmsAttributeTableGenerator
	{
		private readonly AttributeTable attributes;

		public SimpleAttributeTableGenerator(
			AttributeTable attributes)
		{
			this.attributes = attributes;
		}

		public virtual AttributeTable GetAttributes(IDictionary<CmsAttributeTableParameter, object> parameters)
		{
			return attributes;
		}
	}
}
#pragma warning restore
#endif
