// smidgens @ github

#pragma warning disable 0414

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using SMath = System.Math;

	[AddComponentMenu(Config.AddComponentMenu.TELEPORT)]
	[RequireComponent(typeof(Portal))]
	internal partial class PortalTeleport : MonoBehaviour
	{
		[Expand(true)]
		[SerializeField] private TeleportSettings _settings = TeleportSettings.DEFAULTS;

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
			for(var i = 0; i < PortalTraveller.Count; i++)
			{
				// 
				var t = PortalTraveller.GetAt(i);
				if(!t) { continue; }
				// close enough to bother checking?
				if(t.DistanceTo(this) > minDistance) { continue; }

				if (HasEnteredPortal(t, _portal))
				{
					Teleport(t, _portal);
				}
			}
		}

		// teleport to exit
		private void Teleport(PortalTraveller target, Portal from)
		{
			var (pos,rot) = PortalMath.Teleport.GetExitOrientation
			(
				target.transform,
				from.transform,
				from.Link.transform,
				_settings
			);
			target.transform.position = pos;
			target.transform.rotation = rot;
		}

		// did target just enter portal
		private static bool HasEnteredPortal(PortalTraveller target, Portal portal)
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
				case PortalShape.Rect:
					return PortalMath.Linear.InsideRect(portal.Bounds.extents, localPos);
				case PortalShape.Oval:
					return PortalMath.Linear.InsideOval(portal.Bounds.extents, localPos);
			}
			return false;
		}
	}
}
