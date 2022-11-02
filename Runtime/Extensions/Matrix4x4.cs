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