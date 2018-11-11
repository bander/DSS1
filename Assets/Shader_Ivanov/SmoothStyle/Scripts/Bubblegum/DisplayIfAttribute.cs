using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum
{

	/// <summary>
	/// Attribute that will restrict displaying the variable its attached to
	/// </summary>
	public class DisplayIfAttribute : PropertyAttribute
	{
		#region PRIVATE_VARIABLES

		/// <summary>
		/// The field to check
		/// </summary>
		private string field;

		/// <summary>
		/// The value we are comparing against
		/// </summary>
		private object value;

		/// <summary>
		/// The value of the object we want
		/// </summary>
		private object[] values;

		/// <summary>
		/// The set comparison mode
		/// </summary>
		private ComparisonMode compareMode;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Display only if the value is in the range given
		/// </summary>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="compareMode"></param>
		public DisplayIfAttribute (string field, object value, ComparisonMode compareMode)
		{
			this.field = field;
			this.value = value;
            this.compareMode = compareMode;
		}

		/// <summary>
		/// Display only if the value is equal to any of the given value
		/// </summary>
		/// <param name="types"></param>
		public DisplayIfAttribute (string field, params object[] values)
		{
			this.field = field;
			this.values = values;
			compareMode = ComparisonMode.EQUALS;
		}

        #endregion

        #region PUBLIC_METHODS

		/// <summary>
		/// Debug the parameter
		/// </summary>
		public void DebugParameter ()
		{
			Debug.Log (field + " " + values + " " + compareMode + " " + value);
		}

#if UNITY_EDITOR

        /// <summary>
        /// Check if the property should be displayed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ShouldDisplay (SerializedObject obj)
		{
			SerializedProperty property = obj.FindProperty (field);

			if (property == null)
				throw new System.Exception ("Could not find the field with name " + field + " on " + obj.targetObject.name + " of type " + obj.targetObject.GetType ());

			//Compare with comparison
			if (values == null)
				switch (property.propertyType)
				{
					case SerializedPropertyType.Integer:
						return value.Compare (property.intValue, compareMode);

					case SerializedPropertyType.Float:
						return value.Compare (property.floatValue, compareMode);

					case SerializedPropertyType.Enum:
						return value.Compare (property.enumDisplayNames, compareMode);

					case SerializedPropertyType.ObjectReference:
						return value.Compare (property.objectReferenceValue, compareMode);

					default:
						throw new System.Exception ("Property type not supported " + property.propertyType + " on " + obj.targetObject.name + " of type " + obj.targetObject.GetType ());
				}

			//Compare against range
			else
			{
				foreach (object value in values)
				{
					switch (property.propertyType)
					{
						case SerializedPropertyType.Boolean:
							if (property.boolValue.Equals (value))
								return true;
							else
								break;

						case SerializedPropertyType.Integer:
							if (property.intValue.Equals (value))
								return true;
							else
								break;

						case SerializedPropertyType.Float:
							if (property.floatValue.Equals (value))
								return true;
							else
								break;

						case SerializedPropertyType.String:
							if (property.stringValue.Equals (value))
								return true;
							else
								break;

						case SerializedPropertyType.Enum:
							if (property.enumValueIndex.Equals ((int) value))
								return true;
							else
								break;

						default:
							throw new System.Exception ("Property type not supported " + property.propertyType + " on " + obj.targetObject.name + " of type " + obj.targetObject.GetType ());
					}
				}
			}

			return false;
		}

#endif

		#endregion
	}

#if UNITY_EDITOR

	/// <summary>
	/// Drawer for any variables using the DisplayIf attribute
	/// </summary>
	[CustomPropertyDrawer (typeof (DisplayIfAttribute), true)]
	public class DisplayIfAttributeDrawer : PropertyDrawer
	{
		#region DRAWER_METHODS

		/// <summary>
		/// Draw the inspector
		/// </summary>
		/// <param name="position"></param>
		/// <param name="property"></param>
		/// <param name="label"></param>
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty (position, label, property);

			//Get display attribute
			DisplayIfAttribute displayAttribute = (DisplayIfAttribute) attribute;
			bool enabled = displayAttribute.ShouldDisplay (property.serializedObject);

			//Enable/disable the property
			bool wasEnabled = GUI.enabled;
			GUI.enabled = enabled;

			EditorGUI.BeginChangeCheck ();

			if (enabled)
				EditorGUI.PropertyField (position, property, label, true);

			EditorGUI.EndChangeCheck ();

			//Ensure that the next property that is being drawn uses the correct settings
			GUI.enabled = wasEnabled;

			EditorGUI.EndProperty ();
		}

		/// <summary>
		/// Override the get height method
		/// </summary>
		/// <param name="property"></param>
		/// <param name="label"></param>
		/// <returns></returns>
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			//Get display attribute
			DisplayIfAttribute displayAttribute = (DisplayIfAttribute) attribute;
			bool enabled = displayAttribute.ShouldDisplay (property.serializedObject);
			int rows = 1;

			//Special case for expandable entries
			if (property.isArray)
				rows = property.arraySize + 1;
			else if (property.propertyType == SerializedPropertyType.Rect || property.propertyType == SerializedPropertyType.Vector4)
				rows = 6;

			if (enabled)
				return base.GetPropertyHeight (property, label) * (property.isExpanded ? rows : 1);
			else
				return 0f;
		}

		#endregion
	}

#endif
}
