﻿Shader "Hidden/RenderCubes"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		// Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
			#pragma vertex vert
			#pragma fragment frag
			// #pragma debug
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			}; 

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			int cols;
			int rows;
			float pixelsInfo[1000];
			 
			sampler2D _MainTex;

			fixed4 frag (v2f it) : SV_Target 
			{
				fixed4 color = tex2D(_MainTex, it.uv);
				// just invert the colors
				color = 1 - color;
				 
				//int x = (int)it.uv.x * cols;
				//int j = (int)it.uv.y; 
				return 1 - color; 
			}
			ENDCG
		}
	}
}
