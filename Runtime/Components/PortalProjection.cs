// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using System;

	[AddComponentMenu(Config.AddComponentMenu.PROJECTION)]
	[RequireComponent(typeof(Portal))]
	[DisallowMultipleComponent]
	internal class PortalProjection : MonoBehaviour
	{
		public const string SPAWN_NAME = "portal_view";

		public Camera MainCamera => _mainCamera;
		public Camera PortalCamera => _camGetter.Invoke(this);

		internal static class _FN
		{
			public const string
			MAIN_CAM = nameof(_mainCamera);
		}


		[SerializeField] private Camera _mainCamera;
		private Camera _portalCamera = default;
		private Portal _portal = default;

		private Func<PortalProjection, Camera> _camGetter = GetCameraInit;
		private void Awake()
		{
			_portal = GetComponent<Portal>();
		}

		private void LateUpdate()
		{
			if(!_portalCamera || !_mainCamera || !_portal || _portal.Link == null) { return; }
			UpdateOrientation();
			UpdateProjection();
		}

		private void UpdateOrientation()
		{
			var (pos, rot) = PortalMath.Camera.GetOrientation(_mainCamera.transform, _portal.transform, _portal.Link.transform);
			_portalCamera.transform.position = pos;
			_portalCamera.transform.rotation = rot;
		}

		private void UpdateProjection()
		{
			_portalCamera.projectionMatrix =
			PortalMath.Camera.GetProjection(_mainCamera, _portalCamera, _portal.Link.transform);
		}

		// post init getter
		private Camera GetCamera(PortalProjection _)
		{
			return _portalCamera;
		}

		// lazy init voodoo
		private static Camera GetCameraInit(PortalProjection pj)
		{
			pj._portalCamera = CreateCamera(pj.transform);
			pj._camGetter = pj.GetCamera;
			return pj._portalCamera;
		}

		private static Camera CreateCamera(Transform owner)
		{
			var cam = new GameObject(SPAWN_NAME).AddComponent<Camera>();
			cam.transform.parent = owner;
			cam.transform.localPosition = Vector3.zero;
			cam.nearClipPlane = 0.01f;
			cam.enabled = false;
			return cam;
		}

	}
}