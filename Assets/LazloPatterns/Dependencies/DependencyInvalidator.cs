#if UNITY_EDITOR
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace Lazlo
{
    public static class DependencyInvalidator
    {
        public static uint version { get; private set; }

        static DependencyInvalidator()
        {
            EditorApplication.hierarchyChanged += InvalidateAllDependencies;
        }

        public static void InvalidateAllDependencies()
        {
            version++;
        }
    }
}
#endif