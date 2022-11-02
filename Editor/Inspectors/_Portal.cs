// smidgens

namespace Smidgenomics.Unity.Portals.Editor
{
	using System;
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Portal))]
	internal class _Portal : _Base<Portal>
	{
		protected override void OnAfterDefaults()
		{
			foreach(var c in _CHECKERS)
			{
				if (c.disable) { continue; }
				if (!c.fn.Invoke(_Target))
				{
					EditorGUILayout.HelpBox(c.msg, c.type);
					break;
				}
			}
		}

		private struct StateChecker
		{
			public Func<Portal, bool> fn;
			public string msg;
			public MessageType type;
			public bool disable;
		}

		private static readonly StateChecker[] _CHECKERS =
		{
			new StateChecker
			{
				fn = StateCheck.HasLink,
				msg = "Portal has no link",
				type = MessageType.Info,
			},
			new StateChecker
			{
				fn = StateCheck.HasLinkBack,
				msg = "Linked Portal does not link back",
				type = MessageType.Info,
				//disable = true
			},
			new StateChecker
			{
				fn = StateCheck.HasNoSelfLink,
				msg = "Portal links to itself",
				type = MessageType.Warning,
			},
			new StateChecker
			{
				fn = StateCheck.HasMatchingShape,
				msg = "Portal does not match link in size or shape",
				type = MessageType.Warning,
				disable = true
			}
		};

		private static class StateCheck
		{
			public static bool HasLink(Portal p) => p.Link;
			public static bool HasNoSelfLink(Portal p) => p.Link != p;
			public static bool HasLinkBack(Portal p) => p.Link?.Link == p;
			public static bool HasMatchingShape(Portal p)
			{
				if (!p.Link) { return false; }
				return p.Bounds.Equals(p.Link.Bounds);
			}
		}
	}

}