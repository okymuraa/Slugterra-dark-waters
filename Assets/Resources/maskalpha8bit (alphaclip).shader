Shader "AppsMinistry/nGUI/UnlitTransparentColoredMaskedAlpha8bit (AlphaClip)" {
	Properties {
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_AlphaTex ("MaskTexture", 2D) = "white" {}
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off ZTest LEqual
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"

		sampler2D _MainTex; float4 _MainTex_ST; sampler2D _AlphaTex;
		struct appdata { float4 vertex : POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
		struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; UNITY_FOG_COORDS(1) };
		v2f vert (appdata v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.color = v.color;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex); UNITY_TRANSFER_FOG(o, o.pos); return o;
		}
		fixed4 frag (v2f i) : SV_Target {
			fixed4 c = tex2D(_MainTex, i.uv) * i.color; c.a *= tex2D(_AlphaTex, i.uv).a;
			UNITY_APPLY_FOG(i.fogCoord, c);
			return c;
		}
		ENDCG
		}
	}
	Fallback Off
}
