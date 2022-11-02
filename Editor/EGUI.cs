// smidgens @ github

namespace Smidgenomics.Unity.Portals.Editor
{
	using System;
	using UnityEngine;
	using UnityEditor;

	internal static partial class EGUI
	{

		public static void Oval(in Rect pos)
		{
			Circle(pos);
		}

	

		public static void Circle(in Rect pos)
		{
			//var c = Color.red;

			//var a = pos.position;
			//var b = pos.max;
			////Line(a, b, Color.green);

			//var n = 30;
			//var angle = (360f / n);

			//var radius = pos.width * 0.5f;
			//float theta_scale = 0.01f;
			//int size = 10;

			//float theta = 2.0f * Mathf.PI * theta_scale;

			//var sx = 1f;
			//var sy = 1f;
			//var r = pos;
			//if (r.x > r.y) { sy = r.y / r.x; }
			//else if (r.x < r.y) { sx = r.x / r.y; }

			//for (int i = 0; i < size; i++)
			//{
			//	var theta1 = theta * i;
			//	var theta2 = theta * (i+1);

			//	var x1 = new Vector2(radius * Mathf.Cos(theta1), radius * Mathf.Sin(theta1));
			//	var x2 = new Vector2(radius * Mathf.Cos(theta2), radius * Mathf.Sin(theta2));

			//	x1 += pos.center;
			//	x2 += pos.center;

			//	Line(x1, x2, Color.red, 2f);

			//}

			//var tc = Handles.color;
			//var tmat = Handles.matrix;

			//Handles.color = c;

			////var smat = Matrix4x4.Scale(new Vector3(1f,1.2f,1f));
			////var pmat = Matrix4x4.Translate(new Vector3(0f, -5f, 0f));

			////Handles.matrix *= pmat * smat;

			////Handles.matrix = Matrix4x4.TRS(default, Quaternion.identity,
			////new Vector3(sx * pos.width,sy * pos.height,1f));

			//var m1 = Matrix4x4.Translate(pos.center);

			//var m2 = Matrix4x4.Scale(new Vector3(sx, sy, 1f));

			//GUI.matrix = m1 * m2;

			//Handles.DrawWireDisc(pos.center, Vector3.forward, pos.width, 2f);

			////Handles.color = tc;
			//Handles.matrix = tmat;

			//for (var i = 0; i < n; i++)
			//{
			//	var a1 = (angle * i) * 1f * Mathf.Deg2Rad;
			//	var a2 = (angle * (i + 1)) * Mathf.Deg2Rad;
			//	var x1 = new Vector2(Mathf.Cos(a1), Mathf.Sin(a1));
			//	var x2 = new Vector2(Mathf.Cos(a2), Mathf.Sin(a2));

			//	x1 *= pos.width * 0.5f;
			//	x2 *= pos.width * 0.5f;

			//	x1 += pos.center;
			//	x2 += pos.center;

			//	Line(x1, x2, Color.red, 1f);
			//}

		}


		public static void Line(in Vector2 x1, in Vector2 x2, in Color c, in float width = 1f)
		{
			var tmat = GUI.matrix;
			var tcolor = GUI.color;
			GUI.color = c;
			{
				float angle = Vector3.Angle(x2 - x1, Vector2.right);
				if (x1.y > x2.y) { angle = -angle; }
				GUIUtility.ScaleAroundPivot(new Vector2((x2 - x1).magnitude, width), new Vector2(x1.x, x1.y + 0.5f));
				GUIUtility.RotateAroundPivot(angle, x1);
				//EditorGUI.DrawRect(new Rect(x1.x, x1.y, 1, 1), c);
				GUI.DrawTexture(new Rect(x1.x, x1.y, 1, 1), EditorGUIUtility.whiteTexture);
			}
			GUI.color = tcolor;
			GUI.matrix = tmat;
		}

		public static void Border(Rect pos, in Color c, in byte s = 1)
		{
			EditorGUI.DrawRect(pos.SliceLeft(s), c);
			EditorGUI.DrawRect(pos.SliceRight(s), c);
			EditorGUI.DrawRect(pos.SliceTop(s), c);
			EditorGUI.DrawRect(pos.SliceBottom(s), c);
		}
		
	}
}


namespace Smidgenomics.Unity.Portals.Editor
{
	using System;
	using UnityEngine;
	using UnityEditor;

	internal static partial class EGUI
	{
		public struct IndentScope : IDisposable
		{
			public IndentScope(int indent, bool absolute = true)
			{
				_indent = EditorGUI.indentLevel;
				_valid = true;
				_disposed = false;
				if (!absolute) { indent += EditorGUI.indentLevel; }
				EditorGUI.indentLevel = indent;
			}

			public void Dispose()
			{
				if (!_valid || _disposed) { return; }
				_disposed = true;
				EditorGUI.indentLevel = _indent;
			}

			private bool _disposed, _valid;
			private int _indent;
		}


		public struct MatrixScope : IDisposable
		{
			public MatrixScope(Matrix4x4 m)
			{
				_mat = GUI.matrix;
				_valid = true;
				_disposed = false;
				GUI.matrix = m;
			}

			public void Dispose()
			{
				if (!_valid || _disposed) { return; }
				_disposed = true;

				GUI.matrix = _mat;

			}

			private Matrix4x4 _mat;
			private bool _valid, _disposed;
		}
	}
}