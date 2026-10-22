Shader "Mobile/Diffuse_projectorOutline" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ProjTexture ("ProjTexture (RGB)", 2D) = "gray" {}
		_UseProjTexture ("UseProjTexture", Float) = 0
		_ScaleX ("ScaleX", Float) = 0.00559
		_ScaleY ("ScaleY", Float) = 0.00559
		_OffsetX ("OffsetX", Float) = 0.405
		_OffsetY ("OffsetY", Float) = 0.131
		_ProjTexturePower ("ProjTexturePower", Float) = 1
		_Color ("Outline Color", Color) = (1,0,0,1)
		_Outline ("Outline width", Range(0, 0.03)) = 0.004
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

		Pass {
			Name "OUTLINE"
			Cull Front ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			float _Outline;
			fixed4 _Color;
			struct v2f { float4 pos : SV_POSITION; };
			v2f vert (appdata_base v) {
				v2f o; float3 n = normalize(v.normal);
				o.pos = UnityObjectToClipPos(v.vertex + float4(n * _Outline, 0));
				return o;
			}
			fixed4 frag (v2f i) : SV_Target { return _Color; }
			ENDCG
		}

	}
	Fallback "Mobile/VertexLit"
}
