// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.Reflection;
	using SP = UnityEditor.SerializedProperty;
	using System.Collections.Generic;
	using System.Linq;

	[CustomPropertyDrawer(typeof(InlinedAttribute))]
	internal class _Inlined : PropertyDrawer
	{
		public override void OnGUI(Rect pos, SP prop, GUIContent l)
		{
			EnsureInit();

			EditorGUI.BeginProperty(pos, l, prop);

			if (l != GUIContent.none && !fieldInfo.FieldType.IsArray)
			{
				pos = EditorGUI.PrefixLabel(pos, l);
			}

			pos = pos.SliceTop(EditorGUIUtility.singleLineHeight);

			var fields = _init.fields;
			if (fields.Length == 0) { return; }
			var padding = _init.attribute.padding;
			var totalPad = padding * (fields.Length - 1);
			var totalWidth = pos.width - totalPad;
			var ratioWidth = totalWidth - _init.fixedSize;
			var area = pos;
			for (var i = 0; i < fields.Length; i++)
			{
				if(i > 0) { area.SliceLeft(padding); }
				var fw = _init.widths[i];
				if(fw <= 1f) { fw = fw * ratioWidth; }
				var frect = area.SliceLeft(fw);
				var fieldProp = prop.FindPropertyRelative(fields[i]);
				if (fieldProp != null) { EditorGUI.PropertyField(frect, fieldProp, GUIContent.none); }
				else { EditorGUI.DrawRect(frect, Color.red * 0.3f); } // field not found
			}

			EditorGUI.EndProperty();
		}

		private bool _hasInit = false;
		private Init _init = default;

		private struct Init
		{
			public InlinedAttribute attribute;
			public string[] fields;
			public float[] widths;
			public float fixedSize;
		}

		private void EnsureInit()
		{
			if (_hasInit) { return; }

			_hasInit = true;

			var a = attribute as InlinedAttribute;

			FindFields(out var fields, out var widths, out var fixedSize);

			_init = new Init
			{
				attribute = a,
				fields = fields,
				widths = widths,
				fixedSize = fixedSize
			};
		}

		private void FindFields(out string[] fnames, out float[] widths, out float fixedSize)
		{
			var t = fieldInfo.FieldType;
			if (t.IsArray)
			{
				t = t.GetElementType();
			}
			var fields = t.GetFields(_INSTANCE_FIELD).ToList();
			fields.RemoveAll(IsNotSerialized);

			fnames = new string[fields.Count];
			widths = new float[fields.Count];

			var outerWidths = fieldInfo.GetCustomAttributes<FieldWidthAttribute>();
			for(var i = 0; i < fields.Count; i++)
			{
				fnames[i] = fields[i].Name;
				var a = fields[i].GetCustomAttribute<FieldWidthAttribute>();
				if(a == null)
				{
					a = Find(fields[i].Name, outerWidths);
				}
				if(a != null) { widths[i] = a.width; }
			}
			Normalize(widths, out fixedSize);
		}

		private static FieldWidthAttribute Find(string name, IEnumerable<FieldWidthAttribute> widths)
		{
			var i = 0;
			foreach(var w in widths)
			{
				if (w.name == name) { return w; }
				i++;
			}
			return null;
		}

		private static void Normalize(float[] sizes, out float fixedSize)
		{
			fixedSize = 0f;
			var ratio = 0f;
			for(var i = 0; i < sizes.Length; i++)
			{
				if (sizes[i] > 1f) { fixedSize += sizes[i]; continue; }
				if (sizes[i] <= 0f) { sizes[i] = 1f; }
				ratio += sizes[i];
			}
			if (ratio > 0f)
			{
				for (var i = 0; i < sizes.Length; i++)
				{
					if (sizes[i] > 1f) { continue; }
					sizes[i] = sizes[i] / ratio;
				}
			}
		}
		private static bool IsNotSerialized(FieldInfo f)
		{
			return !IsSerialized(f);
		}

		private static bool IsSerialized(FieldInfo m)
		{
			if (m.IsNotSerialized) { return false; }
			if (m.IsDefined(typeof(HideInInspector))) { return false; }
			if (!m.IsPublic)
			{
				if (!m.IsDefined(typeof(SerializeField))) { return false; }
			}
			if (m.IsDefined(typeof(NonSerializedAttribute))) { return false; }
			return true;
		}

		private const BindingFlags _INSTANCE_FIELD =
		BindingFlags.Instance
		| BindingFlags.NonPublic
		| BindingFlags.Public;

	}
}
