Shader "Hidden/FxPro" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ChromAberrTex ("Chromatic Aberration (RGB)", 2D) = "black" {}
		_LensDirtTex ("Lens Dirt Texture", 2D) = "black" {}
		_DirtIntensity ("Lens Dirt Intensity", Float) = 0.1
		_ChromaticAberrationOffset ("Chromatic Aberration Offset", Float) = 1
		_BloomTex ("Bloom (RGBA)", 2D) = "black" {}
		_DOFTex ("DOF (RGB), COC(A)", 2D) = "black" {}
		_COCTex ("COC Texture (RGBA)", 2D) = "white" {}
		_SCurveIntensity ("S-Curve Intensity", Float) = 0.5
		_LensCurvatureBarrelPower ("Lens Curvature Barrel Power", Float) = 1.1
		_LensCurvatureZoom ("Lens Curvature Zoom", Float) = 1
		_FilmGrainTex ("Film Grain (RGB)", 2D) = "white" {}
		_FilmGrainIntensity ("Film Grain Intensity", Float) = 0.5
		_FilmGrainTiling ("Film Grain Tiling", Float) = 4
		_FilmGrainChannel ("Film Grain Channel", Vector) = (1,0,0,0)
		_VignettingIntensity ("Vignetting Intensity", Float) = 0.5
		_CloseTint ("Warm Tint Color", Color) = (1,0.5,0,1)
		_FarTint ("Warm Tint Color", Color) = (0,0,1,1)
		_CloseTintStrength ("Close Tint Strength", Float) = 0.5
		_FarTintStrength ("Far Tint Strength", Float) = 0.5
		_DesaturateDarksStrength ("Desaturate Darks Strength", Float) = 0.25
		_DesaturateFarObjsStrength ("Desaturate Far Objs Strength", Float) = 0.5
		_FogTint ("Fog Tint Color", Color) = (1,1,1,1)
		_FogStrength ("Fog Strength", Float) = 0.5
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
