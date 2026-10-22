Shader "Hidden/NFAA" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurTex ("Base (RGB)", 2D) = "white" {}
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
