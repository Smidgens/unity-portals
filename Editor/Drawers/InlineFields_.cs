// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(InlineFieldsAttribute))]
	internal class InlineFieldsAttribute_ : PropertyDrawer
	{
		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent l)
		{
			if (l != GUIContent.none && !fieldInfo.FieldType.IsArray)
			{
				pos = EditorGUI.PrefixLabel(pos, l);
			}

			var a = attribute as InlineFieldsAttribute;
			var fields = a.Fields;
			if (fields.Length == 0) { return; }

			var padding = a.Padding;
			var totalPad = padding * (fields.Length - 1);
			var fieldWidth = (pos.width - totalPad) / fields.Length;
			pos.height = EditorGUIUtility.singleLineHeight;

			var frect = pos;
			frect.width = fieldWidth;

			for (var i = 0; i < fields.Length; i++)
			{
				var fieldProp = prop.FindPropertyRelative(fields[i]);
				var xpad = i < fields.Length ? padding : 0;
				var offset = new Vector2(i * (fieldWidth + xpad), 0f);
				frect.position = pos.position + offset;
				if (frect != null) { EditorGUI.PropertyField(frect, fieldProp, GUIContent.none); }
				else { EditorGUI.DrawRect(frect, Color.red * 0.3f); } // field not found
			}
		}
	}
}
