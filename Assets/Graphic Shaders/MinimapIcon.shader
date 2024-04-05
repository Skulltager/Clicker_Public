Shader "Custom/Minimap Icon"
{
    Properties
    {
		mainTex("Main Texture", 2D) = "white" {}
		alphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 200

        CGINCLUDE
        uniform sampler2D mainTex;
        uniform float alphaCutoff;
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            };
             
            v2f vert(appdata v)
            {
                v2f output;
				output.vertex = UnityObjectToClipPos(v.vertex);
                output.uv = v.uv;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 color = tex2D(mainTex, input.uv);
                clip(color.a - alphaCutoff);
                return color;
            };
			ENDCG
        }
    }
    FallBack "Diffuse"
}
