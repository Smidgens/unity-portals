// smidgens @ github

namespace Smidgenomics.Unity.Portals
{
	internal enum TPExitMode
	{
		Default,
		Scale
	}

	[System.Serializable]
	internal struct TeleportSettings
	{
		public static readonly TeleportSettings DEFAULTS = new TeleportSettings
		{
			exitMode = TPExitMode.Scale
		};

		public TPExitMode exitMode;
	}
}