// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using System;

	[AddComponentMenu(Config.AddComponentMenu.PORTAL)]
	internal class Portal : MonoBehaviour
	{
		public enum PortalShape { Rect, Oval, }

		[Serializable]
		public struct PortalBounds
		{
			public PortalShape shape;
			public Vector2 extents;
		}

		public Portal Link => _link;
		public PortalBounds Bounds => _bounds;

#if UNITY_EDITOR
		// editor helper
		internal static class _FN
		{
			public const string
			LINK = nameof(_link),
			BOUNDS = nameof(_bounds);
		}
#endif

		[SerializeField] private Portal _link = null;

		[InlineFields
		(
			nameof(PortalBounds.shape),
			nameof(PortalBounds.extents) + ".x",
			nameof(PortalBounds.extents) + ".y"
		)]
		[SerializeField] private PortalBounds _bounds = new PortalBounds()
		{
			shape = PortalShape.Rect,
			extents = Config.Defaults.PORTAL_EXTENTS,
		};

	}
}