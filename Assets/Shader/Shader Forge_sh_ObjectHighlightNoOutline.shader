Shader "Shader Forge/sh_ObjectHighlightNoOutline" {
	Properties {
		_Color ("Color", Color) = (0.5,0.5,0.5,1)
		_FresnelWidth ("FresnelWidth", Float) = 1
		_FlashingSpeed ("FlashingSpeed", Float) = 1
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend One One
		Cull Back Lighting Off ZWrite Off
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"

		fixed4 _Color; float _FresnelWidth, _FlashingSpeed;
		struct v2f { float4 pos : SV_POSITION; float3 n : TEXCOORD0; float3 v : TEXCOORD1; };
		v2f vert (appdata_base v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex);
			o.n = UnityObjectToWorldNormal(v.normal); o.v = WorldSpaceViewDir(v.vertex); return o;
		}
		fixed4 frag (v2f i) : SV_Target {
			float f = pow(1.0 - saturate(dot(normalize(i.v), normalize(i.n))), max(_FresnelWidth, 0.01));
			float flash = sin(_Time.y * _FlashingSpeed * 6.2832) * 0.5 + 0.5;
			return fixed4(_Color.rgb * f * flash, 1);
		}
		ENDCG
		}
	}
	Fallback Off
}
