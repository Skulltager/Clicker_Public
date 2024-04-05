Shader "Custom/Depth Texture Display" {

    SubShader 
    {
        Tags {"RenderType"="Opaque"}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
		    sampler2D roomDepthTexture;
            
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
                float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(roomDepthTexture, i.uv));

                return float4(depth, depth, depth, 1);
            }

            ENDCG
        }
    }
}