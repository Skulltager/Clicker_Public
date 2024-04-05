Shader "Custom/Depth Shader Test" {

    SubShader 
    {
        Tags {"RenderType"="Opaque"}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
		    sampler2D _CameraDepthTexture;
		    sampler2D roomDepthTexture;
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 projPos : POSITION1;
                float4 screenUV : TEXCOORD1;
            };

            v2f vert( appdata v )
            {
                v2f output;
                output.vertex = UnityObjectToClipPos (v.vertex);
                output.uv =  v.uv;

                output.projPos = 0;
                COMPUTE_EYEDEPTH(output.projPos.z);
                output.screenUV = ComputeScreenPos(output.vertex);
                COMPUTE_EYEDEPTH(output.screenUV.z);
                return output;
            }    

            fixed4 frag (v2f i) : SV_TARGET
            {      
                float depthRoom = SAMPLE_DEPTH_TEXTURE_PROJ(roomDepthTexture, UNITY_PROJ_COORD(i.screenUV));
                float depthCamera = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenUV));
                float difference = depthRoom - depthCamera;
                if(difference <= 0.0001 && difference >= -0.0001)
                    return float4(0, 0, 1, 1);

                float depthFragment = i.vertex.z;
                difference = depthRoom - depthFragment;
                if(difference <= 0.0001 && difference >= -0.00001)
                    return float4(0, 1, 0, 1);

                return float4(difference, difference, difference, 1);
            }

            
            //v2f vert( appdata v )
            //{
            //    v2f output;
            //    output.vertex = UnityObjectToClipPos (v.vertex);
            //    output.uv =  v.uv;
            //
            //    output.projPos = 0;
            //    COMPUTE_EYEDEPTH(output.projPos.z);
            //    output.screenUV = ComputeScreenPos(output.vertex);
            //    COMPUTE_EYEDEPTH(output.screenUV.z);
            //    return output;
            //}    
            //
            //fixed4 frag (v2f i) : SV_TARGET
            //{      
            //    float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenUV)));
            //    depth =  depth - i.projPos.z;
            //    if(depth <= 0.00001 && depth >= -0.00001)
            //        return float4(0, 1, 0, 1);
            //    return float4(depth, depth, depth, 1);
            //}
            ENDCG
        }
    }
}