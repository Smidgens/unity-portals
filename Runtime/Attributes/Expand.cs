// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using System;
	using UnityEngine;
	using Conditional = System.Diagnostics.ConditionalAttribute;

	// display type fields expanded
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	internal class ExpandAttribute : PropertyAttribute
	{
		public ExpandAttribute(string label = null)
		{
			this.label = label;
		}

		public ExpandAttribute(bool hideLabel)
		{
			this.hideLabel = hideLabel;
		}

		internal bool hideLabel { get; }
		internal string label { get; }
	}
}
