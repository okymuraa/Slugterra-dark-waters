Shader "Hidden/BloomPro" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DsTex1 ("DsTexture 1 (RGB)", 2D) = "black" {}
		_DsTex2 ("DsTexture 2 (RGB)", 2D) = "black" {}
		_DsTex3 ("DsTexture 3 (RGB)", 2D) = "black" {}
		_DsTex4 ("DsTexture 4 (RGB)", 2D) = "black" {}
		_DsTex5 ("DsTexture 5 (RGB)", 2D) = "black" {}
		_BloomThreshold ("Bloom Threshold", Float) = 0.8
		_BloomIntensity ("Bloom Intensity", Float) = 5
		_BloomTexFactors1 ("Bloom Tex Factors 1", Vector) = (0.166,0.166,0.166,0.166)
		_BloomTexFactors2 ("Bloom Tex Factors 2", Vector) = (0.166,0.166,0,0)
		_BloomTint ("Bloom Tint", Color) = (1,1,1,1)
	}

	// Post-effect replaced by passthrough (original code not recoverable)
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
		Pass {
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 frag (v2f_img i) : SV_Target { return tex2D(_MainTex, i.uv); }
			ENDCG
		}
	}
	Fallback Off
}
