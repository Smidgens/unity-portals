// smidgens @ github

#pragma warning disable IDE0051 // "unused" private methods, called by Unity

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEngine;
	using UnityEditor;

	internal static class PortalGizmos
	{
		private static class Colors
		{
			public static readonly Color
			PORTAL = Color.blue * 0.8f;
		}

		[DrawGizmo((GizmoType)~0, typeof(Portal))]
		private static void OnPortal(Portal target, GizmoType _)
		{
			var skip =
			!target
			|| !target.isActiveAndEnabled
			|| Application.isPlaying;

			if (skip) { return; }

			var pos = Vector3.zero;
			Vector3 size = target.Bounds.extents * 2f;
			size.z = 0.05f;
			Gizmos.color = Colors.PORTAL;
			Gizmos.matrix = target.transform.localToWorldMatrix;
			Gizmos.DrawCube(pos, size);
		}
	}
}