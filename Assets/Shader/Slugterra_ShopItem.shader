Shader "Slugterra/ShopItem" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Colorfulness ("Colorfulness", Range(0, 1)) = 1
		_Opacity ("Opacity", Range(0, 1)) = 1
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range(0.002, 0.03)) = 0.005
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade
		sampler2D _MainTex; fixed4 _Color; float _Colorfulness, _Opacity;
		struct Input { float2 uv_MainTex; };
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * 2.0;
			fixed g = dot(c.rgb, fixed3(0.299, 0.587, 0.114));
			o.Albedo = lerp(fixed3(g,g,g), c.rgb, _Colorfulness); o.Alpha = _Opacity;
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
			fixed4 _OutlineColor; float _Opacity;
			struct v2f { float4 pos : SV_POSITION; };
			v2f vert (appdata_base v) {
				v2f o; float3 n = normalize(v.normal);
				o.pos = UnityObjectToClipPos(v.vertex + float4(n * _Outline, 0));
				return o;
			}
			fixed4 frag (v2f i) : SV_Target { return fixed4(_OutlineColor.rgb, _OutlineColor.a * _Opacity); }
			ENDCG
		}

	}
	Fallback "Mobile/Diffuse"
}
