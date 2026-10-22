Shader "Custom/MiniMapFadeBorder" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"

		fixed4 _Color;
		struct appdata { float4 vertex : POSITION; fixed4 color : COLOR; };
		struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; UNITY_FOG_COORDS(0) };
		v2f vert (appdata v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.color = v.color * _Color; UNITY_TRANSFER_FOG(o, o.pos); return o; }
		fixed4 frag (v2f i) : SV_Target { fixed4 c = i.color; UNITY_APPLY_FOG(i.fogCoord, c); return c; }
		ENDCG
		}
	}
	Fallback Off
}
