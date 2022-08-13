// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	[AddComponentMenu(Config.AddComponentMenu.RENDERING)]
	[RequireComponent(typeof(Portal))]
	[RequireComponent(typeof(PortalProjection))]
	[DisallowMultipleComponent]
	internal class PortalRendering : MonoBehaviour
	{
		[SerializeField] private Mesh _renderMesh = default;

		public const string SHADER_NAME = Config.Shader.Name.PORTAL;
		public const string SHADER_TEX_PROP = "_Tex";
		public const int TEXTURE_SIZE = 1024;

#if UNITY_EDITOR
		// editor helper
		internal static class _FN
		{
			public const string
			MESH = nameof(_renderMesh);
		}
#endif

		// re-used material
		private static Material _closeMaterial = default;

		private Portal _portal = default;
		private PortalProjection _projection = default;

		private MaterialPropertyBlock _pb = null;
		private RenderTexture _texture = default;

		private void OnEnable()
		{
			if (!_renderMesh || !_portal)
			{
				enabled = false;
			}
		}

		private void Awake()
		{
			_pb = new MaterialPropertyBlock();
			_portal = GetComponent<Portal>();
			_projection = GetComponent<PortalProjection>();

			// init texture
			_texture = CreateTexture(TEXTURE_SIZE);
			_pb.SetTexture(SHADER_TEX_PROP, _texture);
			_projection.PortalCamera.targetTexture = _texture;

			if (!_closeMaterial)
			{
				_closeMaterial = new Material(Shader.Find(SHADER_NAME));
			}
		}

		private void Update()
		{
			if (!_projection.MainCamera) { return; }
			if (!IsVisible()) { return; }
			RenderCamera();
			DrawPortal();
		}


		private bool IsVisible()
		{
			var camera = _projection.MainCamera;
			var inFront = Vector3.Dot(transform.forward, camera.transform.position - transform.position) >= 0;
			if (!inFront) { return false; }
			Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
			Vector3 size = _portal.Bounds.extents * 2f;
			size.z = Mathf.Max(size.x, size.y);
			var bounds = new Bounds(transform.position, size);
			return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
		}

		private void RenderCamera()
		{
			var camera = _projection.PortalCamera;
			if (!camera) { enabled = false; }
			if(camera.enabled) { camera.enabled = false; }
			camera.Render();
		}

		private void DrawPortal()
		{
			var material = SelectMaterial();
			var (pos,scale) = ComputeTransform();
			var lmatrix = Matrix4x4.TRS(pos, Quaternion.identity, scale);
			var matrix = transform.localToWorldMatrix * lmatrix;
			Graphics.DrawMesh(_renderMesh, matrix, material, 0, null, submeshIndex: 0, properties: _pb);
		}

		private Material SelectMaterial()
		{
			return _closeMaterial;
		}

		private (Vector3, Vector3) ComputeTransform()
		{
			var cam = _projection.MainCamera;
			float halfHeight = cam.nearClipPlane * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float halfWidth = halfHeight * cam.aspect;
			float distToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, cam.nearClipPlane).magnitude;

			var sameFace = Vector3.Dot(transform.forward, transform.position - cam.transform.position) > 0f;

			Vector3 scale = _portal.Bounds.extents * 2f;
			scale.z = distToNearClipPlaneCorner;

			//var offset = Vector3.zero;
			var offset = Vector3.forward * distToNearClipPlaneCorner * (sameFace ? 0.5f : -0.5f);

			return (offset, scale);
		}

		private static RenderTexture CreateTexture(in int size)
		{
			return new RenderTexture(size, size, 0);
		}
	}
}


namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEditor;

	internal class PortalRenderer_Editor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}

		private void OnEnable()
		{
			
		}
	}

}