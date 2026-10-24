Shader "Mobile/VertexLit" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		sampler2D _MainTex;
		fixed4 _Color;
		struct Input {
			float2 uv_MainTex;
		};
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
