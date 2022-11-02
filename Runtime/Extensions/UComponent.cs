// smidgens @ github

// unity components / gameobject
namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	internal static class Component_
	{
		public static float DistanceTo<T1, T2>(this T1 c1, T2 c2)
			where T1 : Component
			where T2 : Component
		{
			return Vector3.Distance(c1.transform.position, c2.transform.position);
		}

		public static void ParentAndReset(this Transform t, Transform parent)
		{
			t.parent = parent;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
		}
	}
}