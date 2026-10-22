Shader "tk2d/Depth Mask" {
	Properties {
	}

	SubShader {
		Tags { "Queue"="Transparent-100" "IgnoreProjector"="True" }
		Pass { ColorMask 0 ZWrite On Cull Off Lighting Off }
	}
	Fallback Off
}
