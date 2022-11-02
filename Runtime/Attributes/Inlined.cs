// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using System.Diagnostics;
	using System;

	/// <summary>
	/// Render nested fields on a single line
	/// </summary>
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	internal class FieldWidthAttribute : Attribute
	{
		internal string name { get; }
		internal float width { get; }

		public FieldWidthAttribute(string name, float width)
		{
			this.name = name;
			this.width = width;
		}

		public FieldWidthAttribute(float width)
		{
			this.width = width;
		}
	}

	/// <summary>
	/// Render nested fields on a single line
	/// </summary>
	[Conditional("UNITY_EDITOR")]
	internal class InlinedAttribute : PropertyAttribute
	{
		/// <summary>
		/// Space between fields
		/// </summary>
		internal byte padding { get; } = 2;
		public InlinedAttribute() { }
	}
}