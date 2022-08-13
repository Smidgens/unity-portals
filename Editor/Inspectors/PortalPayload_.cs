// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEditor;

	[CustomEditor(typeof(PortalPayload))]
	internal class PortalPayload_ : Base_<PortalPayload>
	{
		protected override void GetDefaultNames(out string[] names)
		{
			names = _DEFAULT_PROPS;
		}

		private static readonly string[] _DEFAULT_PROPS =
		{
			PortalPayload._FN.ONUPDATE
		};
	}
}