Shader "494/SceneTransitionEffect" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Symbol("Symbol", 2D) = "white" {}
		_Progress("Transition Progress (1 = Complete coverage. 0 = No coverage)", Range(0, 1)) = 0
	}
	SubShader{
	Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"
			
		/* Data provided by script */
		int _screen_resolution_x = 1;
		int _screen_resolution_y = 1;
		float _x_pixel_center = 0;
		float _y_pixel_center = 0;

		/* Material inspector fields */
		uniform sampler2D _MainTex;
		uniform sampler2D _Symbol;
		uniform float _Progress;
		uniform float _debug_coloring;

		float4 frag(v2f_img i) : COLOR{

			half2 uv_custom = i.uv;
			
			// Convert to screen coordinates
			uv_custom = half2(uv_custom.x * _screen_resolution_x, uv_custom.y * _screen_resolution_y);

			half2 center = half2(_x_pixel_center, _y_pixel_center);

			float color_factor = 1.0;
			
			// Calculations

			float progress = clamp(_Progress, 0.0, 1.0);

			float max_horizontal_axis = max(_screen_resolution_x - _x_pixel_center, _x_pixel_center);
			float max_vertical_axis = max(_screen_resolution_y - _y_pixel_center, _y_pixel_center);

			float max_diagonal = sqrt(max_horizontal_axis*max_horizontal_axis + max_vertical_axis * max_vertical_axis);

			// Figure out where the ring is.
			half pixel_distance_from_center = distance(uv_custom, half2(_x_pixel_center, _y_pixel_center));

			if (pixel_distance_from_center > progress * max_diagonal) {
				//float distortion_factor = 1.0 - (distance_from_ring / ring_radius);

				// Convert UV to centered coordinate system.
				//half2 uv_custom_centered = uv_custom - half2(x_pos, y_pos);

				// Multiply to push uv out slightly.
				//uv_custom_centered = uv_custom_centered - normalize(uv_custom_centered) * (distortion_factor * distortion_strength);

				// Convert back to lower-left coordinate system.
				//uv_custom = uv_custom_centered + half2(x_pos, y_pos);
				color_factor = 0.0;
			}
			

			uv_custom = half2(uv_custom.x / _screen_resolution_x, uv_custom.y / _screen_resolution_y);

			// Grab the texel
			float4 c = tex2D(_MainTex, uv_custom) * color_factor;

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