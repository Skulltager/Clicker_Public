Shader "Custom/Door Color" {

    SubShader 
    {
        Tags {"RenderType"="Cutout"}
        
        CGINCLUDE
		float fromRoomColor;
		float toRoomColor;

        uniform int doorDepth;
        uniform sampler2D roomDepthTexture;
        uniform sampler2D roomColorTexture;
        uniform sampler2D doorDepthTexture;
        uniform sampler2D doorColorTexture;
        ENDCG

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
				float4 vertex : SV_POSITION;
                float3 worldPosition : POSITION1;
                float4 screenPosition : TEXCOORD0;
            };

            v2f vert (appdata v) {
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);
				output.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                output.screenPosition = ComputeScreenPos(output.vertex);
                COMPUTE_EYEDEPTH(output.screenPosition.z);
                return output;
            }

            float4 frag (v2f input) : SV_Target 
            {
                if(doorDepth == 0)
                {
		            float roomDepth = SAMPLE_DEPTH_TEXTURE_PROJ(roomDepthTexture, UNITY_PROJ_COORD(input.screenPosition));

                    if (roomDepth < input.vertex.z)
                        clip(-1);
                        
                    float2 screenUV = input.screenPosition.xy / input.screenPosition.z;
                    float roomColor = tex2D(roomColorTexture, screenUV).r;
                    if(roomColor != fromRoomColor)
                        clip(-1);
                }
                else
                {               
		            float doorDepth = SAMPLE_DEPTH_TEXTURE_PROJ(doorDepthTexture, UNITY_PROJ_COORD(input.screenPosition));

                    if (doorDepth == 1 || doorDepth < input.vertex.z )
                        clip(-1);
                        
                    float2 screenUV = input.screenPosition.xy / input.screenPosition.z;
                    float doorToColor = tex2D(doorColorTexture, screenUV).g;
                    if(doorToColor != fromRoomColor)
                        clip(-1);
                }


                return float4(fromRoomColor, toRoomColor, 0, 1);
            }

            ENDCG
        }
    }
}