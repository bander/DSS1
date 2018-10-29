// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,dith:2,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:66.7,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:6041,x:32719,y:32712,varname:node_6041,prsc:2|emission-5685-OUT;n:type:ShaderForge.SFN_Tex2d,id:3934,x:32256,y:32743,ptovrint:False,ptlb:Main Tex,ptin:_MainTex,varname:node_3934,prsc:2,ntxv:0,isnm:False|UVIN-9987-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:9295,x:31828,y:32831,varname:node_9295,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1947,x:32022,y:32619,ptovrint:False,ptlb:height,ptin:_height,varname:node_1947,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Parallax,id:9987,x:32022,y:32800,varname:node_9987,prsc:2|UVIN-234-OUT,HEI-1947-A,DEP-2996-OUT,REF-1050-OUT;n:type:ShaderForge.SFN_Append,id:234,x:32129,y:32965,varname:node_234,prsc:2|A-2784-OUT,B-1294-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4389,x:31751,y:33039,ptovrint:False,ptlb:Velocity U,ptin:_VelocityU,varname:node_4389,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8210,x:31751,y:33121,ptovrint:False,ptlb:Velocity V,ptin:_VelocityV,varname:node_8210,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2784,x:31973,y:32965,varname:node_2784,prsc:2|A-9295-U,B-4389-OUT;n:type:ShaderForge.SFN_Multiply,id:1294,x:31973,y:33087,varname:node_1294,prsc:2|A-9295-V,B-8210-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2996,x:31793,y:32741,ptovrint:False,ptlb:Parallax,ptin:_Parallax,varname:node_2996,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Slider,id:1050,x:31671,y:32639,ptovrint:False,ptlb:Parallax Side,ptin:_ParallaxSide,varname:node_1050,prsc:2,min:-1,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:5555,x:32504,y:32733,varname:node_5555,prsc:2|A-3934-RGB,B-4136-RGB,C-8516-OUT;n:type:ShaderForge.SFN_Color,id:4136,x:32256,y:32586,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_4136,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:8516,x:32256,y:32922,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_8516,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3974,x:32434,y:33076,varname:node_3974,prsc:2|A-5555-OUT,B-1947-A;n:type:ShaderForge.SFN_SwitchProperty,id:5685,x:32495,y:32900,ptovrint:False,ptlb:Affect Mask OP,ptin:_AffectMaskOP,varname:node_5685,prsc:2,on:False|A-5555-OUT,B-3974-OUT;proporder:3934-1947-4389-8210-2996-1050-4136-8516-5685;pass:END;sub:END;*/

Shader "ManyWorlds/hologram2" {
    Properties {
        _MainTex ("Main Tex", 2D) = "white" {}
        _height ("height", 2D) = "white" {}
        _VelocityU ("Velocity U", Float ) = 0
        _VelocityV ("Velocity V", Float ) = 0
        _Parallax ("Parallax", Float ) = 1
        _ParallaxSide ("Parallax Side", Range(-1, 1)) = 1
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Intensity ("Intensity", Float ) = 1
        [MaterialToggle] _AffectMaskOP ("Affect Mask OP", Float ) = 0.5
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
            Blend One One
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _height; uniform float4 _height_ST;
            uniform float _VelocityU;
            uniform float _VelocityV;
            uniform float _Parallax;
            uniform float _ParallaxSide;
            uniform float4 _Color;
            uniform float _Intensity;
            uniform fixed _AffectMaskOP;
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
                
////// Lighting:
////// Emissive:
                float4 _height_var = tex2D(_height,TRANSFORM_TEX(i.uv0, _height));
                float2 node_9987 = (_Parallax*(_height_var.a - _ParallaxSide)*mul(tangentTransform, viewDirection).xy + float2((i.uv0.r*_VelocityU),(i.uv0.g*_VelocityV)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9987.rg, _MainTex));
                float3 node_5555 = (_MainTex_var.rgb*_Color.rgb*_Intensity);
                float3 emissive = lerp( node_5555, (node_5555*_height_var.a), _AffectMaskOP );
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"

}
