// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using System.Diagnostics;

	/// <summary>
	/// Render nested fields on a single line
	/// </summary>
	[Conditional("UNITY_EDITOR")]
	internal class InlineFieldsAttribute : PropertyAttribute
	{
		public const float DEFAULT_PADDING = 2f;

		/// <summary>
		/// Space between fields
		/// </summary>
		public float Padding { get; set; } = DEFAULT_PADDING;

		/// <summary>
		/// Field names
		/// </summary>
		public string[] Fields { get; } = { };

		public InlineFieldsAttribute(params string[] fields)
		{
			Fields = fields;
		}
	}
}