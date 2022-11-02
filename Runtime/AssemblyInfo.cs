// smidgens @ github

#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using Smidgenomics.Unity.Portals;
[assembly: InternalsVisibleTo(Config.ASSEMBLY_NAME + ".Editor")]
[assembly: InternalsVisibleTo(Config.ASSEMBLY_NAME + ".Tests")]
#endif

#if SMIDGENOMICS_DEV
[assembly: InternalsVisibleTo("Smidgenomics.Unity.Portals.Demo")]
#endif