Shader "T4MShaders/ShaderModel2/MobileLM/T4M 2 Textures for Mobile" {
	Properties {
		_Splat0 ("Layer 1", 2D) = "white" {}
		_Splat1 ("Layer 2", 2D) = "white" {}
		_Control ("Control (RGBA)", 2D) = "white" {}
		_MainTex ("Never Used", 2D) = "white" {}
	}

	SubShader {
		Tags { "SplatCount"="2" "RenderType"="Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma exclude_renderers xbox360 ps3
		sampler2D _Splat0, _Splat1, _Control;
		struct Input { float2 uv_Control; float2 uv_Splat0; float2 uv_Splat1; };
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 s = tex2D(_Control, IN.uv_Control);
			fixed3 col = tex2D(_Splat0, IN.uv_Splat0).rgb * s.r + tex2D(_Splat1, IN.uv_Splat1).rgb * s.g;
			o.Albedo = col; o.Alpha = 0.0;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
