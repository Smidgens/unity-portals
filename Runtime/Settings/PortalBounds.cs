// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using System;
	using UnityEngine;

	[Serializable]
	internal struct PortalBounds : IEquatable<PortalBounds>
	{
		public static readonly PortalBounds DEFAULTS = new PortalBounds
		{
			shape = PortalShape.Rect,
			extents = Config.Defaults.PORTAL_EXTENTS,
		};

		[FieldWidth(50f)]
		public PortalShape shape;

		[Inlined]
		public Vector2 extents;

		public bool Equals(PortalBounds other)
		{
			if (other.extents != extents) { return false; }
			if (other.shape != shape) { return false; }
			return true;
		}
	}

	internal enum PortalShape { Rect, Oval, }
}
