using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Lazlo
{
	[Serializable]
	public
#if UNITY_2020_1_OR_NEWER
		sealed
#else
		abstract
#endif
		class Dependency<T> : IDependency where T : Component
	{
		Type IDependency.type => typeof(T);

		[SerializeField]
		private DependencySource _source = DependencySource.Self;

		public DependencySource source
		{
			get => _source;
			set => _source = value;
		}

		[SerializeField]
		private T _localReference;

		public T localReference
		{
			get => _localReference;
			set => _localReference = value;
		}

		Component IDependency.localReference => localReference;

#if UNITY_GUID_BASED_REFERENCES
		[SerializeField]
		private GuidReference _globalReference;

		public GuidReference globalReference
		{
			get => _globalReference;
			set => _globalReference = value;
		}
#endif

		[NonSerialized]
		private T raw;

		[NonSerialized]
		private bool _isCached = false;

		public void SetDirty()
		{
			raw = null;
			_isCached = false;
		}

#if UNITY_EDITOR
		[NonSerialized]
		private uint version;

		private void EditorInvalidate()
		{
			if (version != DependencyInvalidator.version && _isCached)
			{
				SetDirty();
				version = DependencyInvalidator.version;
			}
		}
#endif

		private T Fetch(Component self)
		{
			switch (source)
			{
				case DependencySource.Self:   return self.GetComponent<T>();
				case DependencySource.Child:  return self.GetComponentInChildren<T>(true);
				case DependencySource.Parent: return GetComponentInParentIncludingInactive(self);
				case DependencySource.Local:  return localReference;
#if UNITY_GUID_BASED_REFERENCES
				case DependencySource.Global: return globalReference.gameObject.AsUnityNull()?.GetComponent<T>();
#endif
				default:                      throw new NotImplementedException();
			}
		}

		private static T GetComponentInParentIncludingInactive(Component self)
		{
#if UNITY_2020_3_OR_NEWER
			return self.gameObject.GetComponentInParent<T>(true);
#else
			var target = self.transform;

			while (target != null)
			{
				var result = target.GetComponent<T>();

				if (!result.IsUnityNull()) // Do not use T == null to check for Unity fake null
				{
					return result;
				}

				target = target.parent;
			}

			return default;
#endif
		}

		public T Resolve(Component self)
		{
			// Not DRYing to TryResolve for speed

#if UNITY_EDITOR
			EditorInvalidate();
#endif

			if (!_isCached)
			{
				raw = Fetch(self);
				_isCached = true;
			}

			return raw;
		}

		public bool TryResolve(Component self, out T result)
		{
#if UNITY_EDITOR
			EditorInvalidate();
#endif

			if (!_isCached)
			{
				raw = Fetch(self);
				_isCached = true;
			}

			result = raw;

			return !ReferenceEquals(result, null);
		}

		public bool CanResolve(Component self)
		{
			return TryResolve(self, out var result);
		}

		Component IDependency.Resolve(Component self) => Resolve(self);

		bool IDependency.TryResolve(Component self, out Component component)
		{
			var result = TryResolve(self, out var _component);
			component = _component;
			return result;
		}

		public void Set(Component self, T value, bool preferExplicitSources = false)
		{
			if (value == null)
			{
				source = DependencySource.Local;
				localReference = null;
				return;
			}

			var selfGameObject = self.gameObject;
			var selfTransform = selfGameObject.transform;
			var valueGameObject = value.gameObject;
			var valueTransform = valueGameObject.transform;
#if UNITY_GUID_BASED_REFERENCES
			var valueGuidComponent = value.GetComponent<GuidComponent>();
#endif
			var eligibleSelves = valueGameObject.GetComponents<T>();
			var eligibleParents = valueGameObject.GetComponentsInParent<T>();
			var eligibleChildren = valueGameObject.GetComponentsInChildren<T>();

			var areOnSameScene = selfGameObject.scene == valueGameObject.scene;
			var areOnSameGameObject = valueGameObject == selfGameObject;
			var valueIsParentOfSelf = !areOnSameGameObject && selfTransform.IsChildOf(valueTransform);
			var valueIsChildOfSelf = !areOnSameGameObject && valueTransform.IsChildOf(selfTransform);
			var valueIsOnlySelf = areOnSameGameObject && eligibleSelves.Length == 1 && eligibleSelves[0] == value;
			var valueIsOnlyParentOfSelf = valueIsParentOfSelf && eligibleParents.Length == 1 && eligibleParents[0] == value;
			var valueIsOnlyChildOfSelf = valueIsChildOfSelf && eligibleChildren.Length == 1 && eligibleChildren[0] == value;

			var canUseLocalSource = areOnSameScene;
#if UNITY_GUID_BASED_REFERENCES
			var canUseGlobalSource = valueGuidComponent != null;
#endif
			var canUseSelfSource = valueIsOnlySelf;
			var canUseParentSource = valueIsOnlyParentOfSelf;
			var canUseChildSource = valueIsOnlyChildOfSelf;

			if (preferExplicitSources)
			{
				// If we prefer explicit references, we'll choose either Local or Global as the source.
				// We absolutely never want Self, Parent, Child or Singleton to avoid ambiguity.

				if (canUseLocalSource)
				{
					source = DependencySource.Local;
					localReference = value;
				}
#if UNITY_GUID_BASED_REFERENCES
				else if (canUseGlobalSource)
				{
					source = DependencySource.Global;
					globalReference = new GuidReference(valueGuidComponent);
				}
#endif
				else
				{
					Debug.LogError("Failed to assign dependency with explicit source.");
				}
			}
			else
			{
				// If we allow implicit references, we can pick Self, Parent, Child as the source.

				if (canUseSelfSource)
				{
					source = DependencySource.Self;
				}
				else if (canUseParentSource)
				{
					source = DependencySource.Parent;
				}
				else if (canUseChildSource)
				{
					source = DependencySource.Child;
				}
				else if (canUseLocalSource)
				{
					source = DependencySource.Local;
					localReference = value;
				}
#if UNITY_GUID_BASED_REFERENCES
				else if (canUseGlobalSource)
				{
					source = DependencySource.Global;
					globalReference = new GuidReference(valueGuidComponent);
				}
#endif
				else
				{
					Debug.LogError("Failed to assign dependency with implicit source.");
				}
			}
		}
	}
}