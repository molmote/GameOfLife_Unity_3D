Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
			
			sampler2D _MainTex;
			int cols;
			int rows;
			float pixelsInfo[1000];

			fixed4 frag (v2f it) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, it.uv);
				// just invert the colors

				int i = (int)it.uv.x * cols;
				int j = (int)it.uv.y; 
				if (pixelsInfo[i + j] == 0)
				{
				 return col;
					i = 0;
				}

				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
