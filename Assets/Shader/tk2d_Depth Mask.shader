Shader "tk2d/Depth Mask" {
	Properties {
	}

	SubShader {
		Tags { "Queue"="Transparent-100" "IgnoreProjector"="True" }
		Pass {
			ColorMask 0
			ZWrite On
			Cull Off
			Lighting Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_t v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				return fixed4(0, 0, 0, 0);
			}
			ENDCG
		}
	}
	Fallback Off
}
