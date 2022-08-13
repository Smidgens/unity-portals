// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	internal static class Matrix4x4_
	{
		public static Quaternion GetRotation(this in Matrix4x4 m)
		{
			return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
		}
	}
}

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	internal static class Component_
	{
		public static float DistanceTo<T1,T2>(this T1 c1, T2 c2)
			where T1 : Component
			where T2 : Component
		{
			return Vector3.Distance(c1.transform.position, c2.transform.position);
		}
	}

}

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	internal static class Float_
	{
		public static bool KindaZero(this in float v)
		{
			return Mathf.Approximately(v, 0f);
		}

		public static bool KindaZeroOrLess(this in float v)
		{
			return v < 0f || Mathf.Approximately(v, 0f);
		}
	}
}