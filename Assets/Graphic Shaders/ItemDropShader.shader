// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Item Drop Shader"
{
    Properties
    {
		mainTex("Main Texture", 2D) = "white" {}
		alphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        tintColor ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 200

        CGINCLUDE
        uniform float baseLightStrength;
        uniform float4 tintColor;
        uniform float alphaCutoff;
        uniform sampler2D mainTex;
        ENDCG

        Pass
        {
			Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
			#include "UnityCG.cginc"
            
			uniform float4 _LightColor0;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
                float3 worldPosition : POSITION1;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f output;
				output.vertex = UnityObjectToClipPos(v.vertex);
				output.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                output.uv = v.uv;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 color = tex2D(mainTex, input.uv);
                clip(color.a - alphaCutoff);
                
				float4 lightColor = _LightColor0;
				color.rgb = color.rgb * lightColor.rgb * tintColor.rgb;
                return color;
            };
			ENDCG
        }
		Pass 
		{
			Tags{ "LightMode" = "ForwardAdd"}
			Blend One One

			CGPROGRAM

			#pragma vertex vert 
			#pragma fragment frag 
			#pragma multi_compile_fwdadd
			#include "AutoLight.cginc" 
			#include "UnityCG.cginc"

			uniform float4 _LightColor0;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
                float3 worldPosition : POSITION1;
                float2 uv : TEXCOORD0;
				LIGHTING_COORDS(2, 3)
            };

            v2f vert(appdata v)
            {
                v2f output;
				output.vertex = UnityObjectToClipPos(v.vertex);
				output.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                output.uv = v.uv;
				TRANSFER_VERTEX_TO_FRAGMENT(output);
                return output;
            }

			float4 frag(v2f input) : SV_Target
			{ 
                fixed4 color = tex2D(mainTex, input.uv);
                clip(color.a - alphaCutoff);

				float attenuation = (_WorldSpaceLightPos0.w > 0) * LIGHT_ATTENUATION(input) + (_WorldSpaceLightPos0.w == 0);
				float lightFactor = (_WorldSpaceLightPos0.w > 0) + (_WorldSpaceLightPos0.w == 0);
				float4 lightColor = _LightColor0 * attenuation * lightFactor;
                
				color.rgb = color.rgb * lightColor.rgb * tintColor.rgb;
				color.a = 1;

				return color;
			}
            ENDCG
        }
    }
    FallBack "Diffuse"
}
