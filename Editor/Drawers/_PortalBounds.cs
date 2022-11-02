// smidgens @ github


namespace Smidgenomics.Unity.Portals.Editor
{
	using System;
	using UnityEditor;
	using UnityEngine;
	using System.Reflection;
	using System.Collections.Generic;

	using SP = UnityEditor.SerializedProperty;

	[CustomPropertyDrawer(typeof(PortalBounds))]
	internal class _PortalBounds : PropertyDrawer
	{
		public const byte
		LINES = 4,
		INDENT = 15,
		MARGIN = 2;
		public static readonly float LINE_HEIGHT = EditorGUIUtility.singleLineHeight;

		public static readonly Color
		BORDER_COLOR = Color.black,
		PORTAL_LINK_COLOR = new Color(1f, 0.5735729f,0f),
		PORTAL_PREVIEW_COLOR = new Color(0f, 0.5883775f, 1f),
		PREVIEW_BG = GetGrayscale(0.15f);

		public override float GetPropertyHeight(SP property, GUIContent label)
		{
			return LINES * (LINE_HEIGHT + MARGIN);
		}

		public override void OnGUI(Rect pos, SP prop, GUIContent label)
		{
			var ctx = GetContext(prop);

			EditorGUI.BeginProperty(pos, label, prop);
			{
				using(new EGUI.IndentScope(0))
				{
					// prop label
					EditorGUI.LabelField(SliceLine(ref pos), label);
					// indent
					pos.SliceLeft(INDENT);

					Buttons(SliceLineBottom(ref pos), ctx);
					var previewRect = pos.SliceRight(pos.height);
					previewRect.SliceBottom(MARGIN);
					previewRect.SliceLeft(MARGIN);
					DrawPreview(previewRect, ctx);
					Field(SliceLine(ref pos), ctx.shape);
					Field(SliceLine(ref pos), ctx.extents);
				}
			}
			EditorGUI.EndProperty();
		}

		private struct DrawerContext
		{
			public SP
			link,
			shape,
			extents,
			prop;
		}

		private static void DrawPreview(in Rect pos, in DrawerContext ctx)
		{


			EditorGUI.DrawRect(pos, PREVIEW_BG);
			EGUI.Border(pos, BORDER_COLOR);
			var r = pos;
			r.Resize(-5f);
			r.Shift(-2f);
			if(ctx.link != null && ctx.link.objectReferenceValue)
			{
				var p = ctx.link.objectReferenceValue as Portal;
				var linkExtents = p.Bounds.extents;
				var link = GetExtentsBox(r, linkExtents);
				link.Shift(4f);
				EGUI.Border(link, PORTAL_LINK_COLOR);
			}
			var portal = GetExtentsBox(r, ctx.extents.vector2Value);

			//EGUI.Oval(pos);
			//EGUI.Oval(portal);
			EGUI.Border(portal, PORTAL_PREVIEW_COLOR);


		}

		private static Rect GetExtentsBox(in Rect bounds, in Vector2 extents)
		{
			var sx = 1f;
			var sy = 1f;
			var r = bounds;
			if (extents.x > extents.y) { sy = extents.y / extents.x; }
			else if (extents.x < extents.y) { sx = extents.x / extents.y; }
			var c = r.center;
			r.width *= sx;
			r.height *= sy;
			r.center = c;
			return r;
		}

		private static void Buttons(in Rect rr, in DrawerContext ctx)
		{
			if(ctx.link == null) { return; }
			var pos = rr;
			bool enabled = ctx.link.objectReferenceValue;
			if (ctx.link.objectReferenceValue)
			{
				var p = ctx.link.objectReferenceValue as Portal;
				var other = p.Bounds;
				var isSame =
				((int)other.shape) == ctx.shape.enumValueIndex
				&& other.extents == ctx.extents.vector2Value;
				if (isSame)
				{
					enabled = false;
				}
			}

			var r = pos;
			var te = GUI.enabled;
			GUI.enabled = enabled;

			if (MiniButton(r.SliceRight(60f), "this=link",3))
			{
				var so = new SerializedObject(ctx.link.objectReferenceValue);
				var lctx = GetContext(so.FindProperty(Portal.__FN.BOUNDS));
				CopyValues(lctx, ctx);
			}

			if (MiniButton(r.SliceRight(60f), "link=this", 1))
			{
				var so = new SerializedObject(ctx.link.objectReferenceValue);
				var lctx = GetContext(so.FindProperty(Portal.__FN.BOUNDS));
				CopyValues(ctx, lctx);
			}
			GUI.enabled = te;
		}

		private static void CopyValues(in DrawerContext a, in DrawerContext b)
		{
			b.shape.enumValueIndex = a.shape.enumValueIndex;
			b.extents.vector2Value = a.extents.vector2Value;
			b.prop.serializedObject.ApplyModifiedProperties();
		}

		private static bool MiniButton(in Rect pos, string txt, in byte t = 0)
		{
			var s = EditorStyles.miniButton;
			switch (t)
			{
				case 1: s = EditorStyles.miniButtonLeft;break;
				case 2: s = EditorStyles.miniButtonMid;break;
				case 3: s = EditorStyles.miniButtonRight;break;
			}
			return GUI.Button(pos, txt, s);
		}

		private static void Field(in Rect pos, in SP prop)
		{
			EditorGUI.PropertyField(pos, prop);
		}

		private static DrawerContext GetContext(in SP prop)
		{
			SP link = null;

			if(prop.serializedObject.targetObject.GetType() == typeof(Portal))
			{
				link = prop.serializedObject.FindProperty(Portal.__FN.LINK);
			}

			return new DrawerContext
			{
				link = link,
				prop = prop,
				shape = prop.FindPropertyRelative(nameof(PortalBounds.shape)),
				extents = prop.FindPropertyRelative(nameof(PortalBounds.extents)),
			};
		}

		private static Rect SliceRight(ref Rect pos, in float w)
		{
			var r = pos.SliceRight(w);
			pos.SliceRight(MARGIN);
			return r;
		}

		protected static Rect SliceLine(ref Rect pos)
		{
			var r = pos.SliceTop(LINE_HEIGHT);
			pos.SliceTop(MARGIN);
			return r;
		}

		protected static Rect SliceLineBottom(ref Rect pos)
		{
			pos.SliceBottom(MARGIN);
			return pos.SliceBottom(LINE_HEIGHT);
		}

		private static Color GetGrayscale(in float v)
		{
			return new Color(v, v, v);
		}

	}

}