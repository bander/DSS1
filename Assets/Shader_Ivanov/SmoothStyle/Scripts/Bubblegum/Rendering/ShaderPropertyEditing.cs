using UnityEngine;

namespace Bubblegum.Rendering
{

	/// <summary>
	/// Sets the value for the selected property as global shader values
	/// </summary>
	[ExecuteInEditMode]
	public class ShaderPropertyEditing : MonoBehaviour
	{
		#region PUBLIC_VARIABLES

		/// <summary>
		/// Get the property
		/// </summary>
		public string Property { get { return property; } }

		/// <summary>
		/// The key to the selected property
		/// </summary>
		[SerializeField, Tooltip ("The key to the selected property")]
		private string property = "_Shininess";

		/// <summary>
		/// The type of the property
		/// </summary>
		[SerializeField, Tooltip ("The type of the property")]
		private PropertyType propertyType;

		/// <summary>
		/// The color to set
		/// </summary>
		[SerializeField, Tooltip ("The color to set"), DisplayIf ("propertyType", PropertyType.COLOR)]
		public Color colorValue;

		/// <summary>
		/// The float value to set
		/// </summary>
		[SerializeField, Tooltip ("The float value to set"), DisplayIf ("propertyType", PropertyType.FLOAT)]
		public float floatValue;

		/// <summary>
		/// The vector value to set
		/// </summary>
		[SerializeField, Tooltip ("The vector value to set"), DisplayIf ("propertyType", PropertyType.VECTOR)]
		public Vector4 vectorValue;

		#endregion // PUBLIC_VARIABLES

		#region ENUMERATORS

		/// <summary>
		/// The type of the property
		/// </summary>
		public enum PropertyType { FLOAT, COLOR, VECTOR }

		/// <summary>
		/// The last float value we had
		/// </summary>
		private float lastFloatValue;

		/// <summary>
		/// The last color value we had
		/// </summary>
		private Color lastColorValue;

		/// <summary>
		/// The last value of the vector
		/// </summary>
		private Vector4 lastVectorValue;

		/// <summary>
		/// If we need a force update
		/// </summary>
		private bool forceUpdate = true;

		#endregion // ENUMERATORS

		#region MONOBEHAVIOUR_METHODS

		/// <summary>
		/// Update this instance
		/// </summary>
		void Update ()
		{
			switch (propertyType)
			{
				case PropertyType.COLOR:
					if (forceUpdate || lastColorValue != colorValue)
					{
						lastColorValue = colorValue;
						Shader.SetGlobalColor (property, colorValue);
					}
					break;

				case PropertyType.FLOAT:
					if (forceUpdate || lastFloatValue != floatValue)
					{
						lastFloatValue = floatValue;
						Shader.SetGlobalFloat (property, floatValue);
					}
					break;

				case PropertyType.VECTOR:
					if (forceUpdate || lastVectorValue != vectorValue)
					{
						lastVectorValue = vectorValue;
						Shader.SetGlobalVector (property, vectorValue);
					}
					break;
			}

			forceUpdate = false;
		}

		/// <summary>
		/// Set the color value
		/// </summary>
		/// <param name="color"></param>
		public void SetColorValue (Color color)
		{
			colorValue = color;
		}

		/// <summary>
		/// Set the float value
		/// </summary>
		/// <param name="value"></param>
		public void SetFloatValue (float value)
		{
			floatValue = value;
		}

		/// <summary>
		/// Set the vector value
		/// </summary>
		/// <param name="value"></param>
		public void SetVectorValue (Vector4 value)
		{
			vectorValue = value;
		}

		#endregion // MONOBEHAVIOUR_METHODS
	}
}