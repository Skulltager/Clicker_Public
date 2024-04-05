﻿Shader "Custom/Door Shader"
{
    Properties
    {
		mainTex("Main Texture", 2D) = "white" {}
        tintColor ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Cutout" }
        LOD 200

        CGINCLUDE
        float firstRoomColor;
        float secondRoomColor;
        uniform float baseLightStrength;
        uniform float4 tintColor;
        uniform float alphaCutoff;
        uniform sampler2D mainTex;
        uniform sampler2D roomColorTexture;
        uniform sampler2D roomDepthTexture;
        uniform sampler2D doorColorTexture0;
        uniform sampler2D doorColorTexture1;
        uniform sampler2D doorColorTexture2;
        uniform sampler2D doorColorTexture3;
        uniform sampler2D doorColorTexture4;
        uniform sampler2D doorColorTexture5;
        uniform sampler2D doorDepthTexture0;
        uniform sampler2D doorDepthTexture1;
        uniform sampler2D doorDepthTexture2;
        uniform sampler2D doorDepthTexture3;
        uniform sampler2D doorDepthTexture4;
        uniform sampler2D doorDepthTexture5;
        uniform int maxDoorDepth;

		//uniform float4 _LightColor0;
        ENDCG

        Pass
        {
			//Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"
            

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 screenPosition : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f output;
				output.vertex = UnityObjectToClipPos(v.vertex);
                output.worldNormal = UnityObjectToWorldNormal(v.normal);
                output.screenPosition = ComputeScreenPos(output.vertex);
                COMPUTE_EYEDEPTH(output.screenPosition.z);
                output.uv = v.uv;
                return output;
            }

            bool ShouldDraw(float depth, float4 screenPosition)
            {
	            float roomDepthSample = SAMPLE_DEPTH_TEXTURE_PROJ(roomDepthTexture, UNITY_PROJ_COORD(screenPosition));
                if (roomDepthSample < depth)
                    return true;
                
                float2 screenUV = screenPosition.xy / screenPosition.z;
                bool chainFound = true;
                
                float roomIndexColor = tex2D(roomColorTexture, screenUV).r;

                // index 0
				float doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture0, UNITY_PROJ_COORD(screenPosition));

                if (doorDepth < depth || maxDoorDepth == 1)
                    return chainFound && (roomIndexColor == firstRoomColor || roomIndexColor == secondRoomColor);

                float2 doorRoomColors = tex2D(doorColorTexture0, screenUV).rg;
                if(chainFound)
                {
                    if(doorRoomColors.r == roomIndexColor)
                        roomIndexColor = doorRoomColors.g;
                    else if(doorRoomColors.g != roomIndexColor)
                        chainFound = false;
                }
                else
                {
                    if(doorRoomColors.g == roomIndexColor)
                        chainFound = true;
                }

                // Index 1
				doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture1, UNITY_PROJ_COORD(screenPosition));
                
                if (doorDepth < depth || maxDoorDepth == 2)
                    return chainFound && (roomIndexColor == firstRoomColor || roomIndexColor == secondRoomColor);
                    
                doorRoomColors = tex2D(doorColorTexture1, screenUV).rg; 
                
                if(chainFound)
                {
                    if(doorRoomColors.r == roomIndexColor)
                        roomIndexColor = doorRoomColors.g;
                    else if(doorRoomColors.g != roomIndexColor)
                        chainFound = false;
                }
                else
                {
                    if(doorRoomColors.g == roomIndexColor)
                        chainFound = true;
                }
                
                // Index 2
				doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture2, UNITY_PROJ_COORD(screenPosition));
                
                if (doorDepth < depth || maxDoorDepth == 3)
                    return chainFound && (roomIndexColor == firstRoomColor || roomIndexColor == secondRoomColor);
                    
                doorRoomColors = tex2D(doorColorTexture2, screenUV).rg; 
                
                if(chainFound)
                {
                    if(doorRoomColors.r == roomIndexColor)
                        roomIndexColor = doorRoomColors.g;
                    else if(doorRoomColors.g != roomIndexColor)
                        chainFound = false;
                }
                else
                {
                    if(doorRoomColors.g == roomIndexColor)
                        chainFound = true;
                }
                
                // Index 3
				doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture3, UNITY_PROJ_COORD(screenPosition));
                
                if (doorDepth < depth || maxDoorDepth == 4)
                    return chainFound && (roomIndexColor == firstRoomColor || roomIndexColor == secondRoomColor);
                    
                doorRoomColors = tex2D(doorColorTexture3, screenUV).rg; 
                
                if(chainFound)
                {
                    if(doorRoomColors.r == roomIndexColor)
                        roomIndexColor = doorRoomColors.g;
                    else if(doorRoomColors.g != roomIndexColor)
                        chainFound = false;
                }
                else
                {
                    if(doorRoomColors.g == roomIndexColor)
                        chainFound = true;
                }

                // Index 4
				doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture4, UNITY_PROJ_COORD(screenPosition));
                
                if (doorDepth < depth || maxDoorDepth == 5)
                    return chainFound && (roomIndexColor == firstRoomColor || roomIndexColor == secondRoomColor);
                    
                doorRoomColors = tex2D(doorColorTexture4, screenUV).rg; 
                
                if(chainFound)
                {
                    if(doorRoomColors.r == roomIndexColor)
                        roomIndexColor = doorRoomColors.g;
                    else if(doorRoomColors.g != roomIndexColor)
                        chainFound = false;
                }
                else
                {
                    if(doorRoomColors.g == roomIndexColor)
                        chainFound = true;
                }
                // Index 5
				doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture5, UNITY_PROJ_COORD(screenPosition));
                
                if (doorDepth < depth || maxDoorDepth == 6)
                    return chainFound && (roomIndexColor == firstRoomColor || roomIndexColor == secondRoomColor);
                    
                doorRoomColors = tex2D(doorColorTexture5, screenUV).rg; 
                
                if(chainFound)
                {
                    if(doorRoomColors.r == roomIndexColor)
                        roomIndexColor = doorRoomColors.g;
                    else if(doorRoomColors.g != roomIndexColor)
                        chainFound = false;
                }
                else
                {
                    if(doorRoomColors.g == roomIndexColor)
                        chainFound = true;
                }

                return false;
            }
            
            fixed4 frag(v2f input) : SV_Target
            {
                if(!ShouldDraw(input.vertex.z, input.screenPosition))
                    clip(-1);
            
                fixed4 color = tex2D(mainTex, input.uv);
                return color * tintColor.rgba;
            };
			ENDCG
        }
		//Pass 
		//{
		//	Tags{ "LightMode" = "ForwardAdd"}
		//	Blend One One
        //
		//	CGPROGRAM
        //
		//	#pragma vertex vert 
		//	#pragma fragment frag 
		//	#pragma multi_compile_fwdadd
		//	#include "AutoLight.cginc" 
		//	#include "UnityCG.cginc"
        //
		//	uniform float4 _LightColor0;
        //
        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float3 normal : NORMAL;
        //        float2 uv : TEXCOORD0;
        //    };
        //
        //    struct v2f
        //    {
		//		float4 vertex : SV_POSITION;
        //        float3 worldPosition : POSITION1;
        //        float2 uv : TEXCOORD0;
        //        float3 worldNormal : TEXCOORD1;
		//		LIGHTING_COORDS(2, 3)
        //    };
        //
        //    v2f vert(appdata v)
        //    {
        //        v2f output;
		//		output.vertex = UnityObjectToClipPos(v.vertex);
		//		output.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
        //        output.uv = v.uv;
		//		output.worldNormal = UnityObjectToWorldNormal(v.normal);
		//		TRANSFER_VERTEX_TO_FRAGMENT(output);
        //        return output;
        //    }
        //
		//	fixed4 frag(v2f input) : SV_Target
		//	{ 
        //        fixed4 color = tex2D(mainTex, input.uv);
        //
		//		float attenuation = (_WorldSpaceLightPos0.w > 0) * LIGHT_ATTENUATION(input) + (_WorldSpaceLightPos0.w == 0);
		//		float3 differenceToLightSource = _WorldSpaceLightPos0.xyz - input.worldPosition;
		//		float lightFactor = (_WorldSpaceLightPos0.w > 0) * max(0, dot(input.worldNormal, normalize(differenceToLightSource))) + (_WorldSpaceLightPos0.w == 0) * max(0, dot(input.worldNormal, _WorldSpaceLightPos0));
		//		float4 lightColor = _LightColor0 * attenuation * lightFactor;
        //        
		//		color.rgb = color.rgb * lightColor.rgb * tintColor.rgb;
		//		color.a = 1;
        //
		//		return color;
		//	}
        //    ENDCG
        //}
    }
    FallBack "Diffuse"
}
