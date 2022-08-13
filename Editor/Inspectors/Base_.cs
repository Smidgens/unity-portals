// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using UnityEngine;
	using UnityEditor;
	using System;

	using UObject = UnityEngine.Object;
	using SP = UnityEditor.SerializedProperty;

	internal abstract class Base_<T> : Editor where T : UObject
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

		protected virtual void GetDefaultNames(out string[] names)
		{
			names = new string[0];
		}

		private SP[] _defaultProps = null;

		private void OnEnable()
		{
			_Target = target as T;
			GetDefaultNames(out var fields);
			_defaultProps = new SP[fields.Length];
			for(var i = 0; i < fields.Length; i++)
			{
				_defaultProps[i] = serializedObject.FindProperty(fields[i]);
			}
		}

	}
}