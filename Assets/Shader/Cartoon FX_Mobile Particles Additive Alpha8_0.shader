Shader "Cartoon FX/Mobile Particles Additive Alpha8" {
	Properties {
		_MainTex ("Particle Texture (Alpha8)", 2D) = "white" {}
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"

		sampler2D _MainTex; float4 _MainTex_ST; 
		struct appdata { float4 vertex : POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
		struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; UNITY_FOG_COORDS(1) };
		v2f vert (appdata v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.color = v.color;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex); UNITY_TRANSFER_FOG(o, o.pos); return o;
		}
		fixed4 frag (v2f i) : SV_Target {
			fixed4 c = i.color; c.a *= tex2D(_MainTex, i.uv).a;
			UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));
			return c;
		}
		ENDCG
		}
	}
	Fallback Off
}
