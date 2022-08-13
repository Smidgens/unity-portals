// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(PortalTeleport))]
	internal class PortalTeleport_ : Base_<PortalTeleport>
	{
		protected override void GetDefaultNames(out string[] names)
		{
			names = _DEFAULT_PROPS;
		}

		private static readonly string[] _DEFAULT_PROPS =
		{
		};
	}
}