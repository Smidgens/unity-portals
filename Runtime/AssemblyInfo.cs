// smidgens @ github

using System.Runtime.CompilerServices;
using Smidgenomics.Unity.Portals;

#if UNITY_EDITOR
[assembly: InternalsVisibleTo(Config.ASSEMBLY_NAME + ".Editor")]
[assembly: InternalsVisibleTo(Config.ASSEMBLY_NAME + ".Tests")]
#endif