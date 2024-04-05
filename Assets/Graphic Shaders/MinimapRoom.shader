
Shader "Custom/MinimapRoom"
{
	Properties
	{
		textureArray("Texture Array", 2DArray) = "white" {}
		uvScaleFloors("UV Scale Floors", Range(0.0, 500.0)) = 1
		uvScaleWalls("UV Scale Walls", Range(0.0, 500.0)) = 1
		doorColor("Door Color", Color) = (1,1,1,1)
		floorColorFactor("Floor Color Factor", Float) = 1
		selectedRoomColorFactor("Selected Room Color Factor", Float) = 1
	}
	SubShader
	{ 
		Tags { "RenderType" = "Opaque" "DisableBatching" = "true"}
		LOD 200

		CGINCLUDE
		UNITY_DECLARE_TEX2DARRAY(textureArray);

		StructuredBuffer<int> biomeMap;
		StructuredBuffer<int> roomMap;

		uniform float uvScaleFloors;
		uniform float uvScaleWalls;
		uniform float4 doorColor;
		uniform float floorColorFactor;
		uniform float selectedRoomColorFactor;

		uniform int selectedRoom;
		uniform int biomeMapSize;
		uniform float doorHeight;
		uniform float cellSize;
		uniform float wallSize;
		uniform float wallHeight;

		int leftPadding;
		int bottomPadding;
		ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma require 2darray
			#include "UnityCG.cginc" 

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 localVertex : POSITION1;
				float3 worldVertex : POSITION2;
				float3 normal : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f output;
				output.worldVertex = mul(unity_ObjectToWorld, v.vertex).xyz;
				output.localVertex = v.vertex.xyz;
				output.vertex = UnityObjectToClipPos(v.vertex);
				output.normal = v.normal;
				return output;
			}

			float4 GetIndexColor(bool doorTexture, bool wallTexture, int index, float uvX, float uvY)
			{
				bool selected = roomMap[index] == selectedRoom;
				if (doorTexture)
					return selected ? float4(1,1,1,1) : doorColor;

				int biomeIndex = biomeMap[index] * 2 + wallTexture;
				float3 texCoord = float3(uvX, uvY, biomeIndex);
				float4 color = UNITY_SAMPLE_TEX2DARRAY(textureArray, texCoord);

				if (selected && wallTexture)
					color = float4(1,1,1,1);
				
				if (!wallTexture)
					color *= 0.5;

				return color;
			}

			float fmodNegative(float value, float divider)
			{
				float result = value >= 0 ? fmod(value, divider) : divider - fmod(-value, divider);
				return result;
			}

			fixed4 frag(v2f input) : SV_Target
			{
				bool doorTexture = input.localVertex.z >= doorHeight - 0.001f && input.localVertex.z <= doorHeight + 0.001f;
				bool wallTexture = input.localVertex.z >= wallHeight - 0.001f && input.localVertex.z <= wallHeight + 0.001f;

				int leftIndex = input.localVertex.x - leftPadding + wallSize - 1;
				int bottomIndex = input.localVertex.y - bottomPadding + wallSize - 1;
				int rightIndex = leftIndex + 1;
				int topIndex = bottomIndex + 1;

				float uvXInfluence = min(1, fmodNegative(input.localVertex.x + wallSize, 1) * (0.5 / wallSize));
				float uvYInfluence = min(1, fmodNegative(input.localVertex.y + wallSize, 1) * (0.5 / wallSize));

				float uvX = input.localVertex.x * uvScaleWalls;
				float uvY = input.localVertex.y * uvScaleWalls;

				float4 bottomLeftColor = GetIndexColor(doorTexture, wallTexture, leftIndex + bottomIndex * biomeMapSize, uvX, uvY);
				float4 bottomRightColor = GetIndexColor(doorTexture, wallTexture, rightIndex + bottomIndex * biomeMapSize, uvX, uvY);
				float4 topLeftColor = GetIndexColor(doorTexture, wallTexture, leftIndex + topIndex * biomeMapSize, uvX, uvY);
				float4 topRightColor = GetIndexColor(doorTexture, wallTexture, rightIndex + topIndex * biomeMapSize, uvX, uvY);

				float4 textureColor = bottomLeftColor * (1 - uvXInfluence) * (1 - uvYInfluence);
				textureColor += bottomRightColor * uvXInfluence * (1 - uvYInfluence);
				textureColor += topLeftColor * (1 - uvXInfluence) * uvYInfluence;
				textureColor += topRightColor * uvXInfluence * uvYInfluence;

				return textureColor;
			}
			ENDCG
		}
	}
}