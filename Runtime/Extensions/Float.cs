// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	internal static class Float_
	{
		public static bool KindaZero(this in float v) => Mathf.Approximately(v, 0f);
		public static bool KindaZeroOrLess(this in float v) => v < 0f || v.KindaZero();
		public static float PO2(this in float v) => v * v;
	}
}