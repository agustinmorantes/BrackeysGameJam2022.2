using UnityEditor;
using UnityEngine;

namespace Lazlo
{
	[CustomPropertyDrawer(typeof(IDependency), true)]
	public sealed class DependencyDrawer : PropertyDrawer
	{
		private SerializedProperty sourceProperty;

		private SerializedProperty localReferenceProperty;

#if UNITY_GUID_BASED_REFERENCES
		private SerializedProperty globalReferenceProperty;
#endif

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
#if UNITY_GUID_BASED_REFERENCES
			sourceProperty = property.FindPropertyRelativeOrFail("_" + nameof(IDependency.source));

			var source = (DependencySource)sourceProperty.enumValueIndex;

			if (source == DependencySource.Global)
			{
				return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
			}
#endif

			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			EditorGUI.BeginChangeCheck();

			var controlPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			sourceProperty = property.FindPropertyRelativeOrFail("_" + nameof(IDependency.source));
			localReferenceProperty = property.FindPropertyRelativeOrFail("_" + nameof(Dependency<Component>.localReference));
#if UNITY_GUID_BASED_REFERENCES
			globalReferenceProperty = property.FindPropertyRelativeOrFail("_" + nameof(Dependency<Component>.globalReference));
#endif

			EditorGUI.showMixedValue = sourceProperty.hasMultipleDifferentValues;

			var sourcePosition = new Rect
			(
				controlPosition.x,
				controlPosition.y,
				Styles.sourceWidth,
				EditorGUIUtility.singleLineHeight
			);

			var fieldPosition = new Rect
			(
				sourcePosition.xMax + Styles.spaceBetweenSourceAndField,
				controlPosition.y,
				controlPosition.width - Styles.sourceWidth - Styles.spaceBetweenSourceAndField,
				EditorGUIUtility.singleLineHeight
			);

			// Draw source field

			EditorGUI.PropertyField(sourcePosition, sourceProperty, GUIContent.none);

			// Draw reference field
			
			if (sourceProperty.hasMultipleDifferentValues)
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUI.LabelField(fieldPosition, "\u2014", EditorStyles.textField);
				EditorGUI.EndDisabledGroup();
			}
			else
			{
				var source = (DependencySource)sourceProperty.enumValueIndex;

				if (source == DependencySource.Local)
				{
					EditorGUI.PropertyField(fieldPosition, localReferenceProperty, GUIContent.none);
				}
#if UNITY_GUID_BASED_REFERENCES
				else if (source == DependencySource.Global)
				{
					EditorGUI.PropertyField(fieldPosition, globalReferenceProperty, GUIContent.none);
				}
#endif
				else
				{
					EditorGUI.BeginDisabledGroup(true);

					if (property.serializedObject.isEditingMultipleObjects)
					{
						EditorGUI.LabelField(fieldPosition, "\u2014", EditorStyles.textField);
					}
					else
					{
						var dependency = (IDependency)property.GetUnderlyingValue();
						var self = (Component)property.serializedObject.targetObject;
						var component = dependency.Resolve(self);

						EditorGUI.ObjectField(fieldPosition, component, dependency.type, true);
					}

					EditorGUI.EndDisabledGroup();
				}
			}
			
			if (EditorGUI.EndChangeCheck())
			{
				DependencyInvalidator.InvalidateAllDependencies();
			}

			EditorGUI.EndProperty();
		}

		private static class Styles
		{
			public static readonly float sourceWidth = 76;

			public static readonly float spaceBetweenSourceAndField = 2;
		}
	}
}