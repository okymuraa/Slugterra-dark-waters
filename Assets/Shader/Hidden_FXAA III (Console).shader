Shader "Hidden/FXAA III (Console)" {
	Properties {
		_MainTex ("-", 2D) = "white" {}
		_EdgeThresholdMin ("Edge threshold min", Float) = 0.125
		_EdgeThreshold ("Edge Threshold", Float) = 0.25
		_EdgeSharpness ("Edge sharpness", Float) = 4
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
