Shader "494/SceneTransitionEffectImage" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Symbol("Symbol", 2D) = "white" {}
		_Progress("Transition Progress (1 = Complete coverage. 0 = No coverage)", Range(0, 1)) = 0
		_debug_coloring("Debug Effect Area Coloring", Range(0, 1)) = 0
	}
	SubShader{
			Tags{ "Queue" = "Overlay" }

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
		float maximum_size_factor = 1.25;
		int _texture_width = 1920;
		int _texture_height = 1920;


		/* Material inspector fields */
		uniform sampler2D _MainTex;
		uniform sampler2D _Symbol;
		uniform float _Progress;
		uniform float _debug_coloring;

		bool WithinBox(float2 uv, float2 box_dim) {
			if (uv.x - 0.5 >= -box_dim.x * 0.5 && uv.x - 0.5 <= box_dim.x * 0.5 && uv.y - 0.5 >= -box_dim.y * 0.5 && uv.y - 0.5 <= box_dim.y * 0.5)
				return true;
			return false;
		}

		float2 UVWithinBox(float2 uv, float2 box_dim) {
			
			half x_progress = (uv.x - 0.5 - -box_dim.x * 0.5) / box_dim.x;
			half y_progress = (uv.y - 0.5 - -box_dim.y * 0.5) / box_dim.y;

			return float2(x_progress, y_progress);
		}

		float4 frag(v2f_img i) : COLOR{

			half2 pixel_custom = i.uv;
			
			// Convert to screen coordinates
			pixel_custom = half2(pixel_custom.x * _screen_resolution_x, pixel_custom.y * _screen_resolution_y);

			half2 center = half2(_x_pixel_center, _y_pixel_center);

			float color_factor = 1.0;
			
			float progress = clamp(_Progress, 0.0, 1.0);

			float box_width = progress * maximum_size_factor;
			float box_height = progress * maximum_size_factor;
			float2 box_dim = float2(box_width, box_height);


			// Calculations

			float max_horizontal_axis = max(_screen_resolution_x - _x_pixel_center, _x_pixel_center);
			float max_vertical_axis = max(_screen_resolution_y - _y_pixel_center, _y_pixel_center);

			float max_diagonal = sqrt(max_horizontal_axis*max_horizontal_axis + max_vertical_axis * max_vertical_axis);

			// Figure out where the ring is.
			half pixel_distance_from_center = distance(pixel_custom, half2(_x_pixel_center, _y_pixel_center));

			if (pixel_distance_from_center > progress * max_diagonal) {
				//float distortion_factor = 1.0 - (distance_from_ring / ring_radius);

				// Convert UV to centered coordinate system.
				//half2 uv_custom_centered = uv_custom - half2(x_pos, y_pos);

				// Multiply to push uv out slightly.
				//uv_custom_centered = uv_custom_centered - normalize(uv_custom_centered) * (distortion_factor * distortion_strength);

				// Convert back to lower-left coordinate system.
				//uv_custom = uv_custom_centered + half2(x_pos, y_pos);
				//color_factor = 0.0;
			}
			

			half2 pixel_im = i.uv * half2(_texture_width, _texture_height);

			half2 pixel_centered = pixel_custom - half2(_screen_resolution_x, _screen_resolution_y);


			float distance_required = max(_texture_width, _texture_height);

			float2 uv_final = i.uv + normalize(float2(0.5, 0.5) - i.uv) * (1.0 - progress) * distance_required;

			float2 uv_centered = i.uv - float2(0.5, 0.5);
			uv_centered += uv_centered * -0.01;
			uv_final = uv_centered + float2(0.5, 0.5);
			
			
			//uv_centered = uv_centered + normalize(uv_centered - half2(0.5, 0.5f)) * progress;

			//uv_centered -= half2(0.5, 0.5);
			//uv_centered += 


			//uv_custom = half2(uv_custom.x / _screen_resolution_x, uv_custom.y / _screen_resolution_y);

			// Grab the texel
			
			float4 c = tex2D(_MainTex, i.uv);
			float4 c_im = tex2D(_Symbol, uv_final);
			
			if (uv_final.x > 1.0 || uv_final.x < 0.0 || uv_final.y > 1.0 || uv_final.y < 0.0)
				c = float4(0, 0, 0, 0);

			//if (c_im.a > 0)
			//	c = float4(0, 0, 0, 0);


			if (WithinBox(i.uv, box_dim)) {
				//c = float4(1, 0, 0, 1);
				float2 box_uv = UVWithinBox(i.uv, box_dim);

				float4 box_color = tex2D(_Symbol, box_uv);
				if (box_color.a > 0.1)
					c = box_color;

				
			}
			else
				c = float4(0, 0, 0, 1);

			

			// Debug coloring to see the regions of the screen affected
			// by distortion.
			//if(_debug_coloring > 0.5)
			//	c = c * color_factor;

			float lum = c.r*.3 + c.g*.59 + c.b*.11;
			float3 bw = float3(lum, lum, lum);

			float4 result = c;
			result.rgb = c.rgb;
			return c;
		}
		
		ENDCG
		}
	}
}