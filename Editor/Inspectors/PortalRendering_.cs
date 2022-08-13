// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(PortalRendering))]
	internal class PortalRendering_ : Base_<PortalRendering>
	{
		protected override void GetDefaultNames(out string[] names)
		{
			names = _DEFAULT_PROPS;
		}

		private static readonly string[] _DEFAULT_PROPS =
		{
			PortalRendering._FN.MESH,
		};
	}
}