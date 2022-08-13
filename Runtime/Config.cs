namespace Smidgenomics.Unity.Portals
{
	using UnityEngine;

	/// <summary>
	/// magic constants, lt.dan
	/// </summary>
	internal static class Config
	{
		public const string ROOT_NAMESPACE = nameof(Smidgenomics);
		public const string NAMESPACE = nameof(Portals);
		public const string ASSEMBLY_NAME = ROOT_NAMESPACE + ".Unity." + NAMESPACE;
		public readonly static string RES_PATH = (ROOT_NAMESPACE + "." + NAMESPACE).ToLower();

		public static class Defaults
		{
			public const float PORTAL_EXTENTS_X = 0.95f;
			public const float PORTAL_EXTENTS_Y = 1.8f;
			public readonly static Vector2 PORTAL_EXTENTS = new Vector2(PORTAL_EXTENTS_X, PORTAL_EXTENTS_Y);
		}

		public static class Shader
		{
			public static class Name
			{
				public const string
				PORTAL = _PREFIX + "ScreenSpace";

				private const string
				_PREFIX =
				"Hidden/"
				+ ROOT_NAMESPACE + "/"
				+ NAMESPACE + "/";
			}

		}

		public static class AddComponentMenu
		{
			public const string PORTAL = _PREFIX + "Portal";
			public const string PROJECTION = _PREFIX + "Portal Projection";
			public const string RENDERING = _PREFIX + "Portal Rendering";
			public const string TELEPORT = _PREFIX + "Portal Teleport";
			public const string PAYLOAD = _PREFIX + "Portal Payload";
			private const string _PREFIX = ROOT_NAMESPACE + "/Portals/";
		}

	}
}