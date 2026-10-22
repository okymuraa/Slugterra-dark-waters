Shader "ShurikenMagic/TransparentRim" {
	Properties {
		_RimColor ("Rim Color", Color) = (0.5,0.5,0.5,0.5)
		_InnerColor ("Inner Color", Color) = (0.5,0.5,0.5,0.5)
		_InnerColorPower ("Inner Color Power", Range(0, 1)) = 0.5
		_RimPower ("Rim Power", Range(0, 5)) = 2.5
		_AlphaPower ("Alpha Rim Power", Range(0, 8)) = 4
		_AllPower ("All Power", Range(0, 10)) = 1
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back Lighting Off ZWrite Off
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"

		fixed4 _RimColor, _InnerColor; float _InnerColorPower, _RimPower, _AlphaPower, _AllPower;
		struct v2f { float4 pos : SV_POSITION; float3 n : TEXCOORD0; float3 v : TEXCOORD1; UNITY_FOG_COORDS(2) };
		v2f vert (appdata_base v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex);
			o.n = UnityObjectToWorldNormal(v.normal); o.v = WorldSpaceViewDir(v.vertex);
			UNITY_TRANSFER_FOG(o, o.pos); return o;
		}
		fixed4 frag (v2f i) : SV_Target {
			float rim = 1.0 - saturate(dot(normalize(i.v), normalize(i.n)));
			fixed3 col = _RimColor.rgb * pow(rim, _RimPower) * _AllPower + _InnerColor.rgb * _InnerColorPower * 2.0;
			fixed4 c = fixed4(col, saturate(pow(rim, _AlphaPower) * _AllPower));
			UNITY_APPLY_FOG(i.fogCoord, c);
			return c;
		}
		ENDCG
		}
	}
	Fallback Off
}
