// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PlaneFog"
{
	Properties
	{
		_TopColor("Top Color", Color) = (1,0.6827586,0,0)
		_BottomColor("Bottom Color", Color) = (1,0,0,0)
		_Distance("Distance", Range( 0 , 100)) = 0
		_Exponent("Exponent", Range( 0.5 , 10)) = 0.5
		_FadeOffset("Fade Offset", Range( 0 , 1)) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float4 _TopColor;
		uniform float4 _BottomColor;
		uniform float _Distance;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Exponent;
		uniform float _FadeOffset;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 tex2DNode36_g1 = tex2D( _CameraDepthTexture, ase_screenPosNorm.xy );
			#ifdef UNITY_REVERSED_Z
				float staticSwitch38_g1 = ( 1.0 - tex2DNode36_g1.r );
			#else
				float staticSwitch38_g1 = tex2DNode36_g1.r;
			#endif
			float3 appendResult39_g1 = (float3(ase_screenPosNorm.x , ase_screenPosNorm.y , staticSwitch38_g1));
			float4 appendResult42_g1 = (float4((appendResult39_g1*2.0 + -1.0) , 1.0));
			float4 temp_output_43_0_g1 = mul( unity_CameraInvProjection, appendResult42_g1 );
			float4 appendResult49_g1 = (float4(( ( (temp_output_43_0_g1).xyz / (temp_output_43_0_g1).w ) * float3( 1,1,-1 ) ) , 1.0));
			float smoothstepResult15 = smoothstep( ase_worldPos.y , ( ase_worldPos.y - _Distance ) , (mul( unity_CameraToWorld, appendResult49_g1 )).y);
			float temp_output_27_0 = pow( smoothstepResult15 , _Exponent );
			float4 lerpResult32 = lerp( _TopColor , _BottomColor , temp_output_27_0);
			o.Emission = lerpResult32.rgb;
			o.Alpha = ( temp_output_27_0 * _FadeOffset );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
1927;29;1906;1014;840.0684;313.0526;1;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;13;-734.0818,75.60191;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;19;-815.291,248.0675;Float;False;Property;_Distance;Distance;2;0;Create;True;0;0;False;0;0;0;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;26;-1259.497,-338.2684;Float;False;Reconstruct World Position From Depth;4;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-461.5424,201.3376;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;18;-652.9039,-67.60751;Float;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-560.0935,345.8391;Float;False;Property;_Exponent;Exponent;3;0;Create;True;0;0;False;0;0.5;0;0.5;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;15;-325.9166,53.25415;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;27;-81.95654,152.8807;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-149.0684,289.9474;Float;False;Property;_FadeOffset;Fade Offset;6;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-272.0935,-197.1609;Float;False;Property;_BottomColor;Bottom Color;1;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-277.0935,-402.1609;Float;False;Property;_TopColor;Top Color;0;0;Create;True;0;0;False;0;1,0.6827586,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;157.9316,227.9474;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;103.2701,-91.30707;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;532.4055,-36.28546;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;PlaneFog;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;13;2
WireConnection;20;1;19;0
WireConnection;18;0;26;0
WireConnection;15;0;18;0
WireConnection;15;1;13;2
WireConnection;15;2;20;0
WireConnection;27;0;15;0
WireConnection;27;1;28;0
WireConnection;35;0;27;0
WireConnection;35;1;34;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;32;2;27;0
WireConnection;2;2;32;0
WireConnection;2;9;35;0
ASEEND*/
//CHKSM=8E97F3E5B037E4DE7FDA32A74818DC79C6EC1A29