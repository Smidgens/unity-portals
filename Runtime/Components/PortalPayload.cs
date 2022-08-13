// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using UnityEngine.Events;
	using System.Collections.Generic;

	[AddComponentMenu(Config.AddComponentMenu.PAYLOAD)]
	public class PortalPayload : MonoBehaviour
	{
		public static int Count => _instances.Count;
		public static PortalPayload GetAt(int i)
		{
			return i >= 0 && i < _instances.Count ? _instances[i] : null;
		}

		public Vector3 PreviousPosition { get; private set; }

#if UNITY_EDITOR
		// editor helper
		internal static class _FN
		{
			public const string
			ONUPDATE = nameof(_onUpdate);
		}
#endif

		[SerializeField] private UnityEvent _onUpdate = default;

		private static List<PortalPayload> _instances = new List<PortalPayload>();

		protected virtual void OnUpdate() { }

		protected void Update()
		{
			PreviousPosition = transform.position;
			OnUpdate();
			_onUpdate.Invoke();
		}

		private void OnEnable()
		{
			_instances.Add(this);
		}

		private void OnDisable()
		{
			_instances.Remove(this);
		}
	}
}