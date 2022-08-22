using UnityObject = UnityEngine.Object;

namespace Lazlo
{
    public static class UnityObjectUtility
    {
        /// <summary>
        /// Checks whether a Unity object is not actually a null reference,
        /// but a rather destroyed native instance.
        /// </summary>
        public static bool IsDestroyed(this UnityObject target)
        {
            return !ReferenceEquals(target, null) && target == null;
        }

        /// <summary>
        /// Checks whether an object is null or Unity pseudo-null
        /// without having to cast to UnityEngine.Object manually
        /// </summary>
        public static bool IsUnityNull(this object obj)
        {
            return obj == null || obj is UnityObject uo && uo == null;
        }

        /// <summary>
        /// Converts a Unity pseudo-null to a real null, allowing for coalesce operators.
        /// e.g.: destroyedObject.AsUnityNull() ?? otherObject
        /// </summary>
        public static T AsUnityNull<T>(this T obj) where T : UnityObject
        {
            if (obj == null)
            {
                return null;
            }

            return obj;
        }
    }
}