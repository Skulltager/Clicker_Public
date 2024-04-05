Shader "Custom/Skybox Wall Shader" {
    Properties {
        skyboxTexture ("Skybox Texture", Cube) = "" {}
        cloudTexture ("Cloud Texture", 2D) = "" {}
        cloudHeight ("Cloud Height", float) = 3
        cloudUVScale ("Cloud UV Scale", float) = 30
        cloudFadeMinDistance ("Cloud Fade Min Distance", float) = 0
        cloudFadeMaxDistance ("Cloud Fade Max Distance", float) = 30
    }

    SubShader {
        Tags {"Queue" = "Geometry-50" "RenderType"="Cutout"}

        CGINCLUDE
        float roomColor;
        float cloudHeight;
        float cloudUVScale;
        float cloudFadeMinDistance;
        float cloudFadeMaxDistance;

        samplerCUBE skyboxTexture;
        sampler2D cloudTexture;
        sampler2D _CameraDepthTexture;
        uniform int maxDoorDepth;
        uniform sampler2D roomColorTexture;
        uniform sampler2D doorColorTexture0;
        uniform sampler2D doorColorTexture1;
        uniform sampler2D doorColorTexture2;
        uniform sampler2D doorColorTexture3;
        uniform sampler2D doorColorTexture4;
        uniform sampler2D doorColorTexture5;
        ENDCG
        Pass {
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            
            bool ShouldDraw(float4 screenPosition, float depth)
            {
                float2 screenUV = screenPosition.xy / screenPosition.w;
                float roomIndexColor = tex2D(roomColorTexture, screenUV).r;

                // index 0
                float2 doorRoomColors = tex2D(doorColorTexture0, screenUV).rg;
                if(doorRoomColors.r == 1 || maxDoorDepth == 1)
                    return roomIndexColor == roomColor;
                    
                if(doorRoomColors.r == roomIndexColor)
                    roomIndexColor = doorRoomColors.g;

                // index 1
                doorRoomColors = tex2D(doorColorTexture1, screenUV).rg;
                if(doorRoomColors.r == 1 || maxDoorDepth == 2)
                    return roomIndexColor == roomColor;

                if(doorRoomColors.r == roomIndexColor)
                    roomIndexColor = doorRoomColors.g;
                    
                // index 2
                doorRoomColors = tex2D(doorColorTexture2, screenUV).rg;
                if(doorRoomColors.r == 1 || maxDoorDepth == 3)
                    return roomIndexColor == roomColor;

                if(doorRoomColors.r == roomIndexColor)
                    roomIndexColor = doorRoomColors.g;
                    
                // index 3
                doorRoomColors = tex2D(doorColorTexture3, screenUV).rg;
                if(doorRoomColors.r == 1 || maxDoorDepth == 4)
                    return roomIndexColor == roomColor;

                if(doorRoomColors.r == roomIndexColor)
                    roomIndexColor = doorRoomColors.g;
                    
                // index 4
                doorRoomColors = tex2D(doorColorTexture4, screenUV).rg;
                if(doorRoomColors.r == 1 || maxDoorDepth == 5)
                    return roomIndexColor == roomColor;

                if(doorRoomColors.r == roomIndexColor)
                    roomIndexColor = doorRoomColors.g;
                    
                // index 5
                doorRoomColors = tex2D(doorColorTexture5, screenUV).rg;
                if(doorRoomColors.r == 1 || maxDoorDepth == 6)
                    return roomIndexColor == roomColor;

                if(doorRoomColors.r == roomIndexColor)
                    roomIndexColor = doorRoomColors.g;
                    
                return false;

            }

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPosition : TEXCOORD0;
                float4 screenPosition : POSITION1;
            };
            
            v2f vert (appdata v) {
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);
                output.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                output.screenPosition = ComputeScreenPos(output.vertex);
                COMPUTE_EYEDEPTH(output.screenPosition.z);
                return output;
            }
            
            float4 frag (v2f input) : SV_Target {
                
                if(!ShouldDraw(input.screenPosition, input.vertex.z))
                    clip(-1);
                
                float3 cameraDirection = normalize(input.worldPosition - _WorldSpaceCameraPos.xyz);
                float4 color = texCUBE(skyboxTexture, cameraDirection);

                //float offsetFromClouds = cloudHeight - _WorldSpaceCameraPos.y;
                //float distanceToClouds = offsetFromClouds / cameraDirection.y;
                //if(distanceToClouds > 0)
                //{
                //    float cloudFade = 1 - saturate((distanceToClouds - cloudFadeMinDistance) / (cloudFadeMaxDistance - cloudFadeMinDistance));
                //    float2 cloudUV = distanceToClouds * cameraDirection.xz/ cloudUVScale;
                //    float4 cloudColor = tex2D(cloudTexture, cloudUV);
                //    cloudFade *= cloudColor.r;
                //    color.rgb = cloudColor.rrr * cloudFade + color.rgb * (1 - cloudFade);
                //}

                return color;
            }

            ENDCG
        }
    }

    FallBack "Skybox/6 Sided"
}