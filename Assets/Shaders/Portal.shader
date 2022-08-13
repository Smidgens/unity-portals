// smidgens @ github

Shader "Hidden/Smidgenomics/Portals/Portal" {
	Properties {
		[HideInInspector] _Tex("Texture", 2D) = "white" {}
	}
	SubShader {

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _Tex;
		
			struct v2f {
				float4 pos : SV_POSITION;
				float4 clip_pos : TEXCOORD0;
			};

			v2f vert(appdata_base v) {
				v2f o;
				float4 clipSpacePosition = UnityObjectToClipPos(v.vertex);
				o.pos = clipSpacePosition;
				o.clip_pos = clipSpacePosition;
				return o;
			}

			// https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
			half4 frag(v2f i) : SV_Target {

				float4 clip_pos = i.clip_pos;
				float2 uv = ((clip_pos.xy / clip_pos.w) + float2(1.0, 1.0)) * 0.5;
				if (_ProjectionParams.x < 0.0) { uv.y = 1.0 - uv.y; }
				return tex2D(_Tex, uv);
			}
			ENDCG
		}
	}
}