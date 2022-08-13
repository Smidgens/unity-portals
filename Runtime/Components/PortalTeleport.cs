// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using SMath = System.Math;

	[AddComponentMenu(Config.AddComponentMenu.TELEPORT)]
	[RequireComponent(typeof(Portal))]
	internal partial class PortalTeleport : MonoBehaviour
	{
		private Portal _portal = default;

		private void Awake()
		{
			_portal = GetComponent<Portal>();
		}

		private void LateUpdate()
		{
			if(_portal?.Link == null) { return; }

			var extents = _portal.Bounds.extents;
			float minDistance = Mathf.Max(extents.x, extents.y) * 2f;
			minDistance += 1f;

			// loop through travelers in scene
			for(var i = 0; i < PortalPayload.Count; i++)
			{
				// 
				var t = PortalPayload.GetAt(i);

				if(!t) { continue; }

				// close enough to bother checking?
				if(t.DistanceTo(this) > minDistance) { continue; }

				if (HasEnteredPortal(t, _portal))
				{
					Teleport(t, _portal);
				}
			}
		}

		/// <summary>
		/// teleport target to exit
		/// </summary>
		private void Teleport(PortalPayload target, Portal from)
		{
			var (pos,rot) = PortalMath.Teleport.GetExitOrientation(target.transform, from.transform, from.Link.transform);
			target.transform.position = pos;
			target.transform.rotation = rot;
		}

		/// <summary>
		/// Target just entered portal
		/// </summary>
		private static bool HasEnteredPortal(PortalPayload target, Portal portal)
		{
			return CheckBoundsCrossed
			(
				portal,
				target.transform.position,
				target.PreviousPosition
			);
		}

		private static bool CheckBoundsCrossed
		(
			Portal portal,
			in Vector3 position,
			in Vector3 previousPosition
		)
		{
			var normal = portal.transform.forward;

			// negative dot -> position is on other side
			var sideNew = SMath.Sign(Vector3.Dot(normal, position - portal.transform.position));
			var sideOld = SMath.Sign(Vector3.Dot(normal, previousPosition - portal.transform.position));

			// has the current side changed?
			if(sideNew == sideOld || sideNew > 0)
			{
				return false;
			}

			// get current position relative to portal
			var localPos = portal.transform.InverseTransformPoint(position);

			switch(portal.Bounds.shape)
			{
				case Portal.PortalShape.Rect:
					return PortalMath.Linear.InsideRect(portal.Bounds.extents, localPos);
				case Portal.PortalShape.Oval:
					return PortalMath.Linear.InsideOval(portal.Bounds.extents, localPos);
			}
			return false;
		}
	}
}
