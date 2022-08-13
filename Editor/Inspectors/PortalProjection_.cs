// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEditor;

	[CustomEditor(typeof(PortalProjection))]
	internal class PortalProjection_ : Base_<PortalProjection>
	{
		protected override void GetDefaultNames(out string[] names)
		{
			names = _DEFAULT_PROPS;
		}

		private static readonly string[] _DEFAULT_PROPS =
		{
			PortalProjection._FN.MAIN_CAM
		};
	}
}