// smidgens

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Portal))]
	internal class Portal_ : Base_<Portal>
	{
		protected override void OnAfterDefaults()
		{
			if (!Validate(_Target, out string msg, out MessageType t))
			{
				EditorGUILayout.HelpBox(msg, t);
			}
		}

		protected override void GetDefaultNames(out string[] names)
		{
			names = _DEFAULT_PROPS;
		}

		private static readonly string[] _DEFAULT_PROPS =
		{
			Portal._FN.BOUNDS,
			Portal._FN.LINK,
		};

		private static bool Validate(Portal p, out string msg, out MessageType type)
		{
			type = MessageType.None;
			msg = null;

			if (!p.Link)
			{
				type = MessageType.Warning;
				msg = "Portal has no link";
				return false;
			}

			if (p.Link.Link != p)
			{
				type = MessageType.Warning;
				msg = "Linked Portal does not link back.";
				return false;
			}

			else if (p.Link == p)
			{
				type = MessageType.Warning;
				msg = "Portal links to itself.";
				return false;
			}

			var b1 = p.Bounds;
			var b2 = p.Link.Bounds;

			if (b1.shape != b2.shape || b1.extents != b2.extents)
			{
				type = MessageType.Warning;
				msg = "Portal does not match link in size and shape";
				return false;
			}
			return true;
		}

	}

}