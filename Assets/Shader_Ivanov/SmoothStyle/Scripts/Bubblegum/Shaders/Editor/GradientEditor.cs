using UnityEngine;
using UnityEditor;

namespace Bubblegum.Shaders
{
	/// <summary>
	/// Editor script for the gradient shaders
	/// </summary>
	public class GradientEditor : ShaderGUI
	{
		#region VARIABLES

		/// <summary>
		/// No color constant
		/// </summary>
		private static readonly Color noColor = new Color (0f, 0f, 0f, 0f);

		/// <summary>
		/// Color keys
		/// </summary>
		private const string 
			AMBIENT_COLOR_TOP = "_AmbientColor", 
			AMBIENT_COLOR_BOTTOM = "_FogColor",
			AMBIENT_POSITION_BOTTOM = "_WorldBottom",
			AMBIENT_POSITION_TOP = "_WorldTop"
			;

		#endregion

		/// <summary>
		/// Draw the inspector
		/// </summary>
		/// <param name="materialEditor"></param>
		/// <param name="properties"></param>
		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			base.OnGUI (materialEditor, properties);

			//Ambient
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical ("Box");
			EditorGUILayout.LabelField ("Static Ambience", EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			//Positions
			float topPosition = Shader.GetGlobalFloat (AMBIENT_POSITION_TOP);
			float bottomPosition = Shader.GetGlobalFloat (AMBIENT_POSITION_BOTTOM);

			if (topPosition == bottomPosition)
				topPosition = bottomPosition + 100f;

			Shader.SetGlobalFloat (AMBIENT_POSITION_TOP, EditorGUILayout.FloatField ("Ambient Position Top", topPosition));
			Shader.SetGlobalFloat (AMBIENT_POSITION_BOTTOM, EditorGUILayout.FloatField ("Ambient Position Bottom", bottomPosition));

			//Colors
			Color topColor = Shader.GetGlobalColor (AMBIENT_COLOR_TOP);
			Color bottomColor = Shader.GetGlobalColor (AMBIENT_COLOR_BOTTOM);

			if (topColor == noColor)
				topColor = Color.white;

			if (bottomColor == noColor)
				bottomColor = Color.grey;

			Shader.SetGlobalColor (AMBIENT_COLOR_TOP, EditorGUILayout.ColorField ("Ambient Color", topColor));
			Shader.SetGlobalColor (AMBIENT_COLOR_BOTTOM, EditorGUILayout.ColorField ("Fog Color", bottomColor));

			EditorGUILayout.Space ();
			EditorGUILayout.EndVertical ();
		}
	}
}