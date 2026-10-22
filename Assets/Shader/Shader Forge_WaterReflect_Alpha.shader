Shader "Shader Forge/WaterReflect_Alpha" {
	Properties {
		_MainTex ("Base Color", 2D) = "white" {}
		_ReflectCube ("ReflectCube", Cube) = "_Skybox" {}
		_Opacity ("Opacity", Range(0, 1)) = 1
		_Reflectivness ("Reflectivness", Range(0, 1)) = 1
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"

		sampler2D _MainTex; float4 _MainTex_ST; samplerCUBE _ReflectCube; float _Opacity, _Reflectivness;
		struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; float3 r : TEXCOORD1; UNITY_FOG_COORDS(2) };
		v2f vert (appdata_base v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			float3 n = UnityObjectToWorldNormal(v.normal); float3 vd = -WorldSpaceViewDir(v.vertex);
			o.r = reflect(vd, n); UNITY_TRANSFER_FOG(o, o.pos); return o;
		}
		fixed4 frag (v2f i) : SV_Target {
			fixed4 b = tex2D(_MainTex, i.uv);
			fixed3 col = lerp(b.rgb, texCUBE(_ReflectCube, i.r).rgb, _Reflectivness * 0.5);
			fixed4 c = fixed4(col, _Opacity * b.a);
			UNITY_APPLY_FOG(i.fogCoord, c);
			return c;
		}
		ENDCG
		}
	}
	Fallback Off
}
