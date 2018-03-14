/* https://www.alanzucconi.com/2015/07/08/screen-shaders-and-postprocessing-effects-in-unity3d/ */

Shader "494/Color Ring Effect" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ColorStrength("Strength of the distortion effect", Range(0, 1)) = 0.25
		_ring_distance_from_center("_ring_distance_from_center", Range(0, 1)) = 0.2
		_ring_radius("Ring Radius", Range(0, 1)) = 0.1
		_debug_coloring("Debug Black Effect Area Coloring", Range(0, 1)) = 0
	}
	SubShader{
	Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform float _ColorStrength;
		uniform float _ring_distance_from_center;
		uniform float _ring_radius;
		uniform float _debug_coloring;

		float EaseInOut (float t)
		{
			float sqt = t*t;
			return sqt / (2.0f * (sqt - t) + 1.0f);
		}

		float color_factor = 0.0;

		float4 frag(v2f_img i) : COLOR{

			half2 uv_custom = i.uv;

			// Distortion Ring around center of screen.
			half distance_from_center = distance(i.uv, half2(0.5, 0.5));

			float distance_from_ring = abs(_ring_distance_from_center - distance_from_center);

			float color_factor = 0.0;

			if (distance_from_ring < _ring_radius) {
				float distortion_factor = 1.0 - (distance_from_ring / _ring_radius);
				// Apply an ease
				distortion_factor = EaseInOut(distortion_factor);

				
				color_factor = distortion_factor;
			}
		

			// Grab the texel
			float4 c = tex2D(_MainTex, uv_custom);

			c = lerp(c, float4(1, 0, 0, _ColorStrength), color_factor);

			// Debug coloring to see the regions of the screen affected
			// by distortion.
			if(_debug_coloring > 0.5)
				c = c * color_factor;

			float lum = c.r*.3 + c.g*.59 + c.b*.11;
			float3 bw = float3(lum, lum, lum);

			float4 result = c;
			result.rgb = c.rgb;
			return result;
		}
		
		ENDCG
		}
	}
}