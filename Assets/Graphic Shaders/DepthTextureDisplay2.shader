Shader "Custom/Depth Texture Display 2" {

    SubShader 
    {
        Tags {"RenderType"="Opaque"}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
		    sampler2D doorDepthTexture0;
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert( appdata v )
            {
                v2f output;
                output.vertex = UnityObjectToClipPos (v.vertex);
                output.uv =  v.uv;
                return output;
            }    
 
            fixed4 frag (v2f i) : SV_TARGET
            {      
                float depth =  Linear01Depth(SAMPLE_DEPTH_TEXTURE(doorDepthTexture0, i.uv));

                return float4(depth, depth, depth, 1);
            }

            ENDCG
        }
    }
}