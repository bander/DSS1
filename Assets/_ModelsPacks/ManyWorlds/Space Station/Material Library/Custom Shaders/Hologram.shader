// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:2,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:66.7,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:5942,x:32985,y:32603,varname:node_5942,prsc:2|diff-3537-RGB,emission-8313-OUT,alpha-9575-OUT;n:type:ShaderForge.SFN_Tex2d,id:3340,x:32161,y:32881,varname:node_3340,prsc:2,tex:e0623c83595640842a512c3b1002aae5,ntxv:0,isnm:False|UVIN-2203-OUT,TEX-7240-TEX;n:type:ShaderForge.SFN_Time,id:7941,x:31929,y:33379,varname:node_7941,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:3946,x:32041,y:33115,varname:node_3946,prsc:2,uv:0;n:type:ShaderForge.SFN_Parallax,id:1511,x:32449,y:33137,varname:node_1511,prsc:2|UVIN-2203-OUT,HEI-5493-OUT,DEP-8731-OUT,REF-8271-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6324,x:31929,y:33540,ptovrint:False,ptlb:Aceleration A,ptin:_AcelerationA,varname:node_6324,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3705,x:32133,y:33379,varname:node_3705,prsc:2|A-7941-TSL,B-6324-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8731,x:32418,y:33291,ptovrint:False,ptlb:Parallax,ptin:_Parallax,varname:node_8731,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Slider,id:8271,x:32418,y:33364,ptovrint:False,ptlb:Parallax Side,ptin:_ParallaxSide,varname:node_8271,prsc:2,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Append,id:9691,x:32318,y:33491,varname:node_9691,prsc:2|A-3705-OUT,B-141-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8733,x:31929,y:33613,ptovrint:False,ptlb:Aceleration B,ptin:_AcelerationB,varname:_AcelerationA_copy,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:141,x:32133,y:33540,varname:node_141,prsc:2|A-7941-TSL,B-8733-OUT;n:type:ShaderForge.SFN_Add,id:2203,x:32249,y:33125,varname:node_2203,prsc:2|A-3946-UVOUT,B-9691-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:7240,x:31879,y:32816,ptovrint:False,ptlb:Main Tex,ptin:_MainTex,varname:node_7240,tex:e0623c83595640842a512c3b1002aae5,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4526,x:32161,y:32730,varname:node_4526,prsc:2,tex:e0623c83595640842a512c3b1002aae5,ntxv:0,isnm:False|UVIN-1511-UVOUT,TEX-7240-TEX;n:type:ShaderForge.SFN_Multiply,id:9575,x:32810,y:32809,varname:node_9575,prsc:2|A-1974-OUT,B-4987-A,C-2558-OUT;n:type:ShaderForge.SFN_Tex2d,id:4987,x:32194,y:32554,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_4987,prsc:2,tex:e0623c83595640842a512c3b1002aae5,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:3537,x:32459,y:32626,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3537,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:8313,x:32733,y:32935,varname:node_8313,prsc:2|A-3537-RGB,B-1988-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1988,x:32733,y:33085,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_1988,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Clamp01,id:1927,x:32459,y:32778,varname:node_1927,prsc:2|IN-4141-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2558,x:32559,y:32935,ptovrint:False,ptlb:Opacity Power,ptin:_OpacityPower,varname:node_2558,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:1974,x:32645,y:32795,ptovrint:False,ptlb:Invert,ptin:_Invert,varname:node_1974,prsc:2,on:False|A-1927-OUT,B-2459-OUT;n:type:ShaderForge.SFN_OneMinus,id:2459,x:32645,y:32652,varname:node_2459,prsc:2|IN-1927-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5493,x:32226,y:33310,ptovrint:False,ptlb:parallaxDist,ptin:_parallaxDist,varname:node_5493,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Lerp,id:4141,x:32333,y:32841,varname:node_4141,prsc:2|A-4526-A,B-3340-A,T-7162-OUT;n:type:ShaderForge.SFN_Vector1,id:7162,x:32365,y:33008,varname:node_7162,prsc:2,v1:0.5;proporder:7240-4987-8731-8271-5493-6324-8733-3537-1988-2558-1974;pass:END;sub:END;*/

Shader "ManyWorlds/hologramSlider" {
    Properties {
        _MainTex ("Main Tex", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Parallax ("Parallax", Float ) = 0
        _ParallaxSide ("Parallax Side", Range(-1, 1)) = 0
        _parallaxDist ("parallaxDist", Float ) = 0
        _AcelerationA ("Aceleration A", Float ) = 1
        _AcelerationB ("Aceleration B", Float ) = 1
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Emission ("Emission", Float ) = 1
        _OpacityPower ("Opacity Power", Float ) = 0
        [MaterialToggle] _Invert ("Invert", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float _AcelerationA;
            uniform float _Parallax;
            uniform float _ParallaxSide;
            uniform float _AcelerationB;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float4 _Color;
            uniform float _Emission;
            uniform float _OpacityPower;
            uniform fixed _Invert;
            uniform float _parallaxDist;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuse = (directDiffuse + indirectDiffuse) * _Color.rgb;
////// Emissive:
                float3 emissive = (_Color.rgb*_Emission);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                float4 node_7941 = _Time + _TimeEditor;
                float2 node_2203 = (i.uv0+float2((node_7941.r*_AcelerationA),(node_7941.r*_AcelerationB)));
                float2 node_1511 = (_Parallax*(_parallaxDist - _ParallaxSide)*mul(tangentTransform, viewDirection).xy + node_2203);
                float4 node_4526 = tex2D(_MainTex,TRANSFORM_TEX(node_1511.rg, _MainTex));
                float4 node_3340 = tex2D(_MainTex,TRANSFORM_TEX(node_2203, _MainTex));
                float node_1927 = saturate(lerp(node_4526.a,node_3340.a,0.5));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                return fixed4(finalColor,(lerp( node_1927, (1.0 - node_1927), _Invert )*_Mask_var.a*_OpacityPower));
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float _AcelerationA;
            uniform float _Parallax;
            uniform float _ParallaxSide;
            uniform float _AcelerationB;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float4 _Color;
            uniform float _Emission;
            uniform float _OpacityPower;
            uniform fixed _Invert;
            uniform float _parallaxDist;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuse = directDiffuse * _Color.rgb;
/// Final Color:
                float3 finalColor = diffuse;
                float4 node_7941 = _Time + _TimeEditor;
                float2 node_2203 = (i.uv0+float2((node_7941.r*_AcelerationA),(node_7941.r*_AcelerationB)));
                float2 node_1511 = (_Parallax*(_parallaxDist - _ParallaxSide)*mul(tangentTransform, viewDirection).xy + node_2203);
                float4 node_4526 = tex2D(_MainTex,TRANSFORM_TEX(node_1511.rg, _MainTex));
                float4 node_3340 = tex2D(_MainTex,TRANSFORM_TEX(node_2203, _MainTex));
                float node_1927 = saturate(lerp(node_4526.a,node_3340.a,0.5));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                return fixed4(finalColor * (lerp( node_1927, (1.0 - node_1927), _Invert )*_Mask_var.a*_OpacityPower),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"

}
