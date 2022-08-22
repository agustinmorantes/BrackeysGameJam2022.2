namespace Lazlo
{
    public enum DependencySource
    {
        Self = 0,

        Parent = 1,

        Child = 2,

        Local = 3,

#if UNITY_GUID_BASED_REFERENCES
		Global = 4
#endif
    }
}