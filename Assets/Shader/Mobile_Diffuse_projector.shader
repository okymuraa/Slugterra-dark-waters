Shader "Mobile/Diffuse_projector" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ProjTexture ("ProjTexture (RGB)", 2D) = "gray" {}
		_UseProjTexture ("UseProjTexture", Float) = 0
		_ScaleX ("ScaleX", Float) = 0.00559
		_ScaleY ("ScaleY", Float) = 0.00559
		_OffsetX ("OffsetX", Float) = 0.405
		_OffsetY ("OffsetY", Float) = 0.131
		_ProjTexturePower ("ProjTexturePower", Float) = 1
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		sampler2D _MainTex; sampler2D _ProjTexture;
		float _UseProjTexture, _ScaleX, _ScaleY, _OffsetX, _OffsetY, _ProjTexturePower;
		struct Input { float2 uv_MainTex; float3 worldPos; };
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			if (_UseProjTexture > 0.5) {
				float2 puv = float2(IN.worldPos.x * _ScaleX + _OffsetX, IN.worldPos.z * _ScaleY + _OffsetY);
				c.rgb *= lerp(fixed3(1,1,1), tex2D(_ProjTexture, puv).rgb * 2.0, saturate(_ProjTexturePower));
			}
			o.Albedo = c.rgb; o.Alpha = c.a;
		}
		ENDCG

	}
	Fallback "Mobile/VertexLit"
}
