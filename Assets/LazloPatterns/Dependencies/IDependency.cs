using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Lazlo
{
    public interface IDependency
    {
        Type type { get; }

        DependencySource source { get; }

        Component localReference { get; }

#if UNITY_GUID_BASED_REFERENCES
		GuidReference globalReference { get; }
#endif

        bool CanResolve(Component self);

        Component Resolve(Component self);

        bool TryResolve(Component self, out Component component);
    }
}