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

		[SerializeField] private Camera _mainCamera;
		[SerializeField] private Camera _cameraPreset = default;

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
		private static Camera GetCameraInit(PortalProjection pp)
		{
			pp._portalCamera = CreateCamera(pp.transform, pp._cameraPreset);
			pp._camGetter = pp.GetCamera;
			return pp._portalCamera;
		}

		private static Camera CreateCamera(Transform owner, Camera preset)
		{
			Camera cam = NewOrPreset(preset, SetCameraDefaults);
			cam.gameObject.name = SPAWN_NAME;
			cam.transform.ParentAndReset(owner);
			cam.enabled = false;
			return cam;
		}

		private static void SetCameraDefaults(Camera cam)
		{
			cam.nearClipPlane = 0.01f;
		}

		private static T NewOrPreset<T>(T preset, Action<T> initFn) where T : Component
		{
			T instance;
			if (preset != null) { instance = Instantiate(preset); }
			else
			{
				instance = new GameObject().AddComponent<T>();
				initFn.Invoke(instance);
			}
			return instance;
		}
	}
}