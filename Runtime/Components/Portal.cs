// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	[AddComponentMenu(Config.AddComponentMenu.PORTAL)]
	internal class Portal : MonoBehaviour
	{
		public Portal Link => _link;
		public PortalBounds Bounds => _bounds;

#if UNITY_EDITOR

		internal static class __FN
		{
			public const string
			LINK = nameof(_link),
			BOUNDS = nameof(_bounds);
		}
#endif

		[SerializeField] private Portal _link = null;
		[SerializeField] private PortalBounds _bounds = PortalBounds.DEFAULTS;

	}
}