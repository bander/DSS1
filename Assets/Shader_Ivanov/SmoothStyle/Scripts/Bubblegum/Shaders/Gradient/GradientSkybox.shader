Shader "Bubblegum/Gradient/Skybox Gradient"
{
    Properties
    {
		_ColorTop ("Color Top", Color) = (1, 1, 0, 0)
        _ColorBottom ("Color Bottom", Color) = (1, 0, 1, 0)
		_Equator ("Equator", Float) = 0
		_Blend ("Blend", Float) = 1.0
    }

    SubShader
    {
        Tags 
		{
			"IgnoreProjector" = "True"
			"Queue" = "Background"
			"RenderType" = "Opaque"
			"PreviewType" = "Skybox"
		}

        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }

            CGPROGRAM

            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag

			#include "UnityCG.cginc"

			half4 _ColorBottom;
			half4 _ColorTop;
			half _Blend;
			half _Equator;

			struct appdata
			{
				float4 position : POSITION;
				float3 texcoord : TEXCOORD0;
			};
    
			struct v2f
			{
				float4 position : SV_POSITION;
				float3 texcoord : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.position);
				o.texcoord = v.texcoord;

				return o;
			}
    
			fixed4 frag (v2f i) : COLOR
			{
				return lerp (_ColorBottom, _ColorTop, saturate (i.texcoord.y * _Blend + _Equator));
			}

            ENDCG
        }
    }

	FallBack "Skybox/Procedural"
}