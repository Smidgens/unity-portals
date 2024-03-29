﻿// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using UCamera = UnityEngine.Camera;
	using SMath = System.Math;

	internal static class PortalMath
	{
		public static class Matrix
		{
			public static readonly Matrix4x4
			ROTATION_180 = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(180f, Vector3.up), Vector3.one);
		}

		public static class Linear
		{
			public static bool CrossedPlane(in Vector3 origin, in Vector3 normal, in Vector3 position, in Vector3 prevPosition)
			{
				var sideNew = SMath.Sign(Vector3.Dot(normal, position - origin));
				var sideOld = SMath.Sign(Vector3.Dot(normal, prevPosition - origin));
				return sideNew != sideOld && sideNew <= 0;
			}

			public static bool InsideRect(in Vector2 extents, in Vector2 localPos)
			{
				return
				Mathf.Abs(localPos.x) <= extents.x
				&& Mathf.Abs(localPos.y) <= extents.y;
			}

			public static bool InsideOval(in Vector2 extents, in Vector2 localPos)
			{
				if (NonZeroXY(extents)) { return false; }
				float sx = 1f, sy = 1f;
				float r = Mathf.Max(extents.x, extents.y);
				if (extents.y < extents.x)
				{
					sy = extents.x / extents.y;
				}
				else if(extents.x < extents.y)
				{
					sx = extents.y / extents.x;
				}
				var x = localPos.x * sx;
				var y = localPos.y * sy;
				return InsideCircle(r, x, y);
			}

			public static bool InsideCircle(in float radius, in float x, in float y)
			{
				return x.PO2() + y.PO2() < radius.PO2();
			}

			public static bool InsideCircle(in float radius, in Vector2 pos)
			{
				return InsideCircle(radius, pos.x, pos.y);
			}

			private static bool NonZeroXY(in Vector2 v)
			{
				return v.x > 0f && v.y > 0f;
			}

		}

		public static class Teleport
		{
			public static (Vector3, Quaternion) GetExitOrientation
			(
				Transform t,
				Transform a,
				Transform b,
				in TeleportSettings settings
			)
			{
				// TODO: factor in settings
				var aflip = Matrix.ROTATION_180 * a.worldToLocalMatrix;
				var a2b = b.localToWorldMatrix * aflip;
				var position = a2b.MultiplyPoint(t.position);
				var rotation = b.rotation * aflip.GetRotation() * t.rotation;
				return (position, rotation);
			}
		}


		public static class Camera
		{
			public static Matrix4x4 GetProjection
			(
				UCamera mainCamera,
				UCamera portalCamera,
				Transform portalExit
			)
			{
				Matrix4x4 cameraClipMatrix = portalCamera.worldToCameraMatrix.inverse.transpose;
				Vector4 worldClipPlane = portalExit.forward;
				worldClipPlane.w = Vector3.Dot(portalExit.position, -portalExit.forward);
				Vector4 clipPlane = cameraClipMatrix * worldClipPlane;
				return mainCamera.CalculateObliqueMatrix(clipPlane);
			}

			public static (Vector3, Quaternion) GetOrientation
			(
				Transform view,
				Transform from,
				Transform to
			)
			{
				// mirrored space relative to start portal
				var localFlipped = Matrix.ROTATION_180 * from.worldToLocalMatrix;
				var mirrorPos = localFlipped.MultiplyPoint(view.position);
				var mirrorRot = localFlipped.GetRotation() * view.rotation;
				var pos = to.localToWorldMatrix.MultiplyPoint(mirrorPos);
				var rot = to.rotation * mirrorRot;
				return (pos, rot);
			}
		}
	}
}