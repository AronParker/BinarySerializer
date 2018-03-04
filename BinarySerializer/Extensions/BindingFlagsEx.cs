using System.Reflection;

namespace BinarySerializer.Extensions
{
    public static class BindingFlagsEx
    {
        public const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags DeclaredInstance = BindingFlags.DeclaredOnly | Instance;
    }
}
