shader "Bubblegum/Gradient/Gradient Textured" {
	Properties{
		[Header (Texture)]
		_MainTexture ("Main Texture", 2D) = "white" {}

		[Header(Colors)]
		[MaterialToggle] _EnableAmbience("Enable Ambience", Float) = 0

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

		//Non lightmap
		Pass{
			Tags{ "LightMode" = "Vertex" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "Gradients.cginc"
			#include "UnityCG.cginc"

			sampler2D _MainTexture;
			float4 _MainTexture_ST;

			struct vertexInput{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				float4 uv0 : TEXCOORD0;
			};

			struct vertexOutput{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR0;
				UNITY_FOG_COORDS(1)
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv0, _MainTexture);
				UNITY_TRANSFER_FOG(o, o.pos);

				o.color = fixed4 (ComputeVertexMultiplyGradient(v.normal), mul(unity_ObjectToWorld, v.vertex).y);

				return o;
			}

			fixed4 frag(vertexOutput i) : COLOR
			{
				fixed4 mainColor = tex2D(_MainTexture, i.uv) * ComputeFragmentGradient(i.color);
				UNITY_APPLY_FOG(i.fogCoord, mainColor);

				return mainColor;
			}

			ENDCG
		}

		//Lightmap
		Pass{
			Tags{ "LightMode" = "VertexLM"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "Gradients.cginc"

			sampler2D _MainTexture;
			float4 _MainTexture_ST;

			struct vertexInput {
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			struct vertexOutput {
				float4 pos : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				fixed4 color : COLOR0;
				UNITY_FOG_COORDS(2)
			};

			vertexOutput vert (vertexInput v)
			{
				vertexOutput o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv0 = TRANSFORM_TEX (v.uv0, _MainTexture);
				o.uv1 = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

				UNITY_TRANSFER_FOG (o, o.pos);
				o.color = fixed4(ComputeVertexMultiplyGradient(v.normal), mul(unity_ObjectToWorld, v.vertex).y);

				return o;
			}

			fixed4 frag(vertexOutput o) : COLOR
			{
				fixed4 mainColor = tex2D(_MainTexture, o.uv0) * ComputeFragmentGradient(o.color);
				mainColor.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, o.uv1));
				UNITY_APPLY_FOG(o.fogCoord, mainColor);

				return mainColor;
			}

			ENDCG
		}
	}
	FallBack "Mobile/VertexLit"
	CustomEditor "Bubblegum.Shaders.GradientEditor"
}