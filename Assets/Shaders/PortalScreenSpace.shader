// smidgens @ github

Shader "Hidden/Smidgenomics/Portals/ScreenSpace" {
	Properties{
		[HideInInspector] _Tex("Texture", 2D) = "white" {}
	}
		SubShader{

			Tags { "RenderType"="Opaque" }
			Cull Off

			Pass {
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 screenPos : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				uniform sampler2D _Tex;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.screenPos = ComputeScreenPos(o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// magic perspective divide, lt.dan
					float2 screenSpaceUV = i.screenPos.xy / i.screenPos.w;
					return tex2D(_Tex, screenSpaceUV);
				}

				ENDCG
			}
	}
}