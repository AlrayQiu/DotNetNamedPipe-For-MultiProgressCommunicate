
#define humble
using Rcl;

[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]
namespace Tlarc
{
        internal static class Ros2Def
        {
#if humble
                internal static RclContext context;
                internal static IRclNode node;
#endif
        }
        internal static class TlarcSystem
        {
#if DEBUG
                internal static string ConfigurationPath = "./declarations/";
                internal static string RootPath = "./";
#else
                internal static string ConfigurationPath = Environment.ProcessPath + "/../../../share/SharedMemory1/declarations/";
                internal static string RootPath = Environment.ProcessPath + "/../../../share/SharedMemory1/";
#endif
        }
}