Shader "Hidden/DOFPro" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB), Depth (A)", 2D) = "white" {}
		[HideInInspector] _COCTex ("COC Texture (RGBA)", 2D) = "white" {}
		[HideInInspector] _BlurOffsets ("Blur step size", Vector) = (1,0,0,0)
		_FocalDist ("Focal Dist", Float) = 0.1
		_FocalLength ("Focal Length", Float) = 0.02
		_BokehThreshold ("Bokeh Threshold", Float) = 0.5
		_BokehGain ("Bokeh Gain", Float) = 2
		_BokehBias ("Bokeh Bias", Float) = 0.5
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
