Shader "Custom/Skybox Ceiling Shader" {
    Properties {
        _MainTex ("Skybox Texture", Cube) = "" {}
    }

    SubShader {
        Tags {"Queue" = "Geometry-50" "RenderType"="Cutout"}

        CGINCLUDE
        float roomColor;
        samplerCUBE _MainTex;
        sampler2D roomColorTexture;
        sampler2D roomDepthTexture;
        sampler2D doorColorTexture0;
        sampler2D doorColorTexture1;
        sampler2D doorColorTexture2;
        sampler2D doorColorTexture3;
        sampler2D doorColorTexture4;
        sampler2D doorColorTexture5;
        sampler2D doorColorTexture6;
        sampler2D doorColorTexture7;
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
                float3 worldPosition : TEXCOORD0;
            };
            
            v2f vert (appdata v) {
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);
                output.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                return output;
            }
            
            float4 frag (v2f input) : SV_Target {
                
                float4 color = texCUBE(_MainTex, normalize(input.worldPosition - _WorldSpaceCameraPos.xyz));
                return color;
            }

            ENDCG
        }
    }

    FallBack "Skybox/6 Sided"
}