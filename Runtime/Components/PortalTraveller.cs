// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using UnityEngine.Events;
	using System.Collections.Generic;

	[AddComponentMenu(Config.AddComponentMenu.TRAVELLER)]
	public class PortalTraveller : MonoBehaviour
	{
		public static int Count => _instances.Count;
		public static PortalTraveller GetAt(int i)
		{
			return i >= 0 && i < _instances.Count ? _instances[i] : null;
		}

		public Vector3 PreviousPosition { get; private set; }

		[SerializeField] private UnityEvent _onUpdate = default;

		private static List<PortalTraveller> _instances = new List<PortalTraveller>();

		protected virtual void OnUpdate() { }

		protected void Update()
		{
			PreviousPosition = transform.position;
			OnUpdate();
			_onUpdate.Invoke();
		}

		private void OnEnable()
		{
			Add(this);
		}

		private void OnDisable()
		{
			Remove(this);
		}

		private static void Add(PortalTraveller t)
		{


			_instances.Add(t);
		}

		private static void Remove(PortalTraveller t)
		{
			_instances.Remove(t);
		}
	}
}