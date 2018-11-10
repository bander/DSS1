// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FogedStandart"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_HeightFogColor("HeightFogColor", Color) = (0,0,0,0)
		_HeightFogStart("HeightFogStart", Float) = 0
		_HeightFogEnd("HeightFogEnd", Float) = 0
		_HeightFogExp("HeightFogExp", Range( 0.5 , 2)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 2.0
		#pragma only_renderers d3d11 glcore gles gles3 metal 
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _HeightFogStart;
		uniform float _HeightFogEnd;
		uniform float _HeightFogExp;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _HeightFogColor;
		uniform float4 _EmissionColor;

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float smoothstepResult14 = smoothstep( _HeightFogStart , _HeightFogEnd , ase_worldPos.y);
			float temp_output_28_0 = pow( smoothstepResult14 , _HeightFogExp );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			o.Albedo = ( temp_output_28_0 * ( _Color * tex2D( _MainTex, uv_MainTex ) ) ).rgb;
			o.Emission = ( ( _HeightFogColor * ( 1.0 - temp_output_28_0 ) ) + ( _EmissionColor * temp_output_28_0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
1;23;1918;1026;2079.52;1258.565;1.58959;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;8;-1144.077,-1069.397;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;10;-1157.07,-853.1306;Float;False;Property;_HeightFogEnd;HeightFogEnd;5;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1148.353,-925.3192;Float;False;Property;_HeightFogStart;HeightFogStart;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-931.9942,-1060.298;Float;False;Property;_HeightFogExp;HeightFogExp;6;0;Create;True;0;0;False;0;0.5;1;0.5;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;14;-889.5477,-905.2108;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;28;-603.8516,-803.6829;Float;False;2;0;FLOAT;0;False;1;FLOAT;2.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;19;-453.8679,-953.0464;Float;False;Property;_EmissionColor;EmissionColor;2;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-759.9742,-629.8958;Float;False;Property;_HeightFogColor;HeightFogColor;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;17;-392.6778,-735.3986;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-1080.363,-414.4803;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1117.936,-207.0598;Float;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-136.7887,-832.621;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-133.5859,-607.5452;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-713.0436,-369.262;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;68.26837,-640.2799;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-196.4282,-426.1852;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;271.8611,-536.728;Float;False;True;0;Float;ASEMaterialInspector;0;0;Lambert;FogedStandart;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;False;True;True;True;True;True;False;False;False;False;False;False;False;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;8;2
WireConnection;14;1;9;0
WireConnection;14;2;10;0
WireConnection;28;0;14;0
WireConnection;28;1;12;0
WireConnection;17;0;28;0
WireConnection;38;0;19;0
WireConnection;38;1;28;0
WireConnection;35;0;11;0
WireConnection;35;1;17;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;37;0;35;0
WireConnection;37;1;38;0
WireConnection;36;0;28;0
WireConnection;36;1;3;0
WireConnection;0;0;36;0
WireConnection;0;2;37;0
ASEEND*/
//CHKSM=19E55AF958641D502F1E9DEB7839B17B74BFFEB3