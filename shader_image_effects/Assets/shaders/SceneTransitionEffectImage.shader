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

		bool WithinBox(float2 pixel_centered, float2 box_dim) {
			if (pixel_centered.x >= -box_dim.x * 0.5 && pixel_centered.x <= box_dim.x * 0.5 && pixel_centered.y >= -box_dim.y * 0.5 && pixel_centered.y <= box_dim.y * 0.5)
				return true;
			return false;
		}

		float2 UVWithinBox(float2 pixel_centered, float2 box_dim) {
			
			//half x_progress = (uv.x - 0.5 - -box_dim.x * 0.5) / box_dim.x;
			//half y_progress = (uv.y - 0.5 - -box_dim.y * 0.5) / box_dim.y;

			float x_progress = (pixel_centered.x - -box_dim.x * 0.5) / box_dim.x;
			float y_progress = (pixel_centered.y - -box_dim.y * 0.5) / box_dim.y;

			return float2(x_progress, y_progress);
		}

		float4 frag(v2f_img i) : COLOR {
			// Convert from UV to screen coordinates
			float2 pixel_screen = half2(i.uv.x * _screen_resolution_x, i.uv.y * _screen_resolution_y);
			
			// Calculate the size of the image box.
			float max_axis = max(_screen_resolution_x, _screen_resolution_y);
			float progress = clamp(_Progress, 0.0, 1.0);

			float box_width = progress * max_axis * maximum_size_factor;
			float box_height = progress * max_axis * maximum_size_factor;
			float2 box_dim = float2(box_width, box_height);

			// Center the pixel coordinates
			float2 pixel_centered = pixel_screen - float2(_screen_resolution_x * 0.5, _screen_resolution_y * 0.5);

			// Grab the texels
			float4 c = tex2D(_MainTex, i.uv);

			// Is pixel within the image box?
			if (WithinBox(pixel_centered, box_dim)) {
				float2 box_uv = UVWithinBox(pixel_centered, box_dim);

				float4 box_color = tex2D(_Symbol, box_uv);
				if (box_color.a > 0.1)
					c = box_color;
			}
			else
				c = float4(0, 0, 0, 1);

			return c;
		}
		
		ENDCG
		}
	}
}