shader "Bubblegum/Gradient/Gradient Simple" {
	Properties{

		[Header (Colors)]
		[MaterialToggle] _EnableAmbience ("Enable Ambience", Float ) = 0

		_GradientPower("Gradient Power", Range(1, 3)) = 1
		_GradientMix("Gradient Mix", Range(0, 1)) = 0

		_ColorFront("Forward Color", Color) = (1, 0, 0, 1)
		_ColorBack("Backward Color", Color) = (0, 1, 0, 1)
		_ColorLeft("Left Color", Color) = (0, 0, 1, 1)
		_ColorRight("Right Color", Color) = (1, 1, 0, 1)
		_ColorTop("Top Color", Color) = (1, 1, 1, 1)
		_ColorBottom("Bottom Color", Color) = (0, 0, 0, 1)
	}

	SubShader{
		Tags {"RenderType"="Opaque"}

		//Non Lightmap
		Pass{
			Tags{ "LightMode" = "Vertex" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "Gradients.cginc"

			struct vertexInput
			{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : POSITION;
				fixed4 color : COLOR0;
				UNITY_FOG_COORDS(1)
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
				o.color = fixed4(ComputeVertexMultiplyGradient(v.normal), mul(unity_ObjectToWorld, v.vertex).y);
				
				return o;
			}

			fixed4 frag(vertexOutput i) : COLOR
			{
				fixed4 mainColor = ComputeFragmentGradient(i.color);
				UNITY_APPLY_FOG(i.fogCoord, mainColor);

				return mainColor;
			}

			ENDCG
		}

		Pass{
			Tags{ "LightMode" = "VertexLM" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "Gradients.cginc"

			struct vertexInput 
			{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				float2 uv0 : TEXCOORD1;
			};

			struct vertexOutput 
			{
				float4 pos : POSITION;
				fixed4 color : COLOR0;
				float2 uv0 : TEXCOORD1;
				UNITY_FOG_COORDS(0)
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0 = v.uv0.xy * unity_LightmapST.xy + unity_LightmapST.zw;

				UNITY_TRANSFER_FOG(o, o.pos);
				o.color = fixed4(ComputeVertexMultiplyGradient(v.normal), mul(unity_ObjectToWorld, v.vertex).y);

				return o;
			}

			fixed4 frag(vertexOutput i) : COLOR
			{
				fixed4 mainColor = ComputeFragmentGradient(i.color);
				mainColor.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv0));
				UNITY_APPLY_FOG(i.fogCoord, mainColor);

				return mainColor;
			}

			ENDCG
		}
	}
	FallBack "Mobile/VertexLit"
	CustomEditor "Bubblegum.Shaders.GradientEditor"
}