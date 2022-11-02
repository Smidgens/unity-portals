// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using System;
	using System.Reflection;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;
	using UnityObject = UnityEngine.Object;
	using SP = UnityEditor.SerializedProperty;

	internal abstract class _Base<T> : Editor where T : UnityObject
	{
		public override sealed void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			OnDrawDefaults();
			OnAfterDefaults();
			serializedObject.ApplyModifiedProperties();
		}

		protected T _Target { get; private set; } = null;

		protected virtual void OnDrawDefaults()
		{
			foreach(var p in _defaultProps)
			{
				EditorGUILayout.PropertyField(p);
			}
		}

		protected virtual void OnAfterDefaults() { }

		protected virtual void GetFieldNames(out string[] names)
		{
			var sfields = FindSerializedFields(_Target.GetType());
			names = sfields.ToArray();
		}

		private SP[] _defaultProps = null;

		private void OnEnable()
		{
			_Target = target as T;
			GetFieldNames(out var fields);

			_defaultProps = new SP[fields.Length];
			for(var i = 0; i < fields.Length; i++)
			{
				_defaultProps[i] = serializedObject.FindProperty(fields[i]);
			}
		}

		private static List<string> FindSerializedFields(Type t)
		{
			var fields = new List<string>();
			var flags =
			BindingFlags.Public
			| BindingFlags.NonPublic
			| BindingFlags.Instance;

			foreach(var f in t.GetFields(flags))
			{
				if (!IsSerialized(f)) { continue; }
				fields.Add(f.Name);
			}
			return fields;
		}

		private static bool IsSerialized(FieldInfo f)
		{
			if (f.IsNotSerialized) { return false; }
			if (f.IsPrivate)
			{
				if (!f.IsDefined(typeof(SerializeField))) { return false; }
			}
			if (f.IsDefined(typeof(NonSerializedAttribute))) { return false; }
			return true;
		}

	}
}