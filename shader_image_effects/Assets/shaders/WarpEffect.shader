Shader "494/WarpEffect" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_debug_coloring("Debug Effect Area Coloring", Range(0, 1)) = 0
	}
	SubShader{
	Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		/*
			Data locations for point "n":
			[n+0] X Position
			[n+1] Y Position
			[n+2] Distortion Strength
			[n+3] Distortion Ring Radius
			[n+4] Spawn Time
			[n+5] Distance
			[n+6] Duration_sec
		*/
			
		/* Data provided by script */
		float _DistortionRings[210]; // (max of 30 simoultaneous rings) x (7 pieces of data per ring) = 210
		int _NumDistortionRings = 0;
		int _number_data_members = 0;
		float _current_time = 0.0;
		int _screen_resolution_x = 1;
		int _screen_resolution_y = 1;

		uniform sampler2D _MainTex;
		uniform float _debug_coloring;

		float EaseInOut (float t)
		{
			float sqt = t*t;
			return sqt / (2.0f * (sqt - t) + 1.0f);
		}

		float4 frag(v2f_img i) : COLOR{

			half2 uv_custom = i.uv;
			
			// Convert to screen coordinates
			uv_custom = half2(uv_custom.x * _screen_resolution_x, uv_custom.y * _screen_resolution_y);

			float color_factor = 1.0;

			for (int index = 0; index < _NumDistortionRings; index++) {
				// Grab properties of each ring
				float x_pos = _DistortionRings[index * _number_data_members];
				float y_pos = _DistortionRings[index * _number_data_members + 1];
				float distortion_strength = _DistortionRings[index * _number_data_members + 2];
				float ring_radius = _DistortionRings[index * _number_data_members + 3];
				float instantiation_time = _DistortionRings[index * _number_data_members +4];
				float desired_distance = _DistortionRings[index * _number_data_members + 5];
				float duration_sec = _DistortionRings[index * _number_data_members + 6];

				// Calculations
				float ring_progress = clamp((_current_time - instantiation_time) / duration_sec, 0.0, 1.0);
				float ring_distance_from_center = ring_progress * desired_distance;

				// Figure out where the ring is.
				half pixel_distance_from_center = distance(uv_custom, half2(x_pos, y_pos));
				float distance_from_ring = abs(ring_distance_from_center - pixel_distance_from_center);

				if (distance_from_ring < ring_radius && ring_progress < 1.0) {
					float distortion_factor = 1.0 - (distance_from_ring / ring_radius);
					// Apply an ease
					distortion_factor = EaseInOut(distortion_factor);

					// Convert UV to centered coordinate system.
					half2 uv_custom_centered = uv_custom - half2(x_pos, y_pos);

					// Multiply to push uv out slightly.
					uv_custom_centered = uv_custom_centered - normalize(uv_custom_centered) * (distortion_factor * distortion_strength);

					// Convert back to lower-left coordinate system.
					uv_custom = uv_custom_centered + half2(x_pos, y_pos);
					color_factor = 0.0;
					break;
				}
			}

			uv_custom = half2(uv_custom.x / _screen_resolution_x, uv_custom.y / _screen_resolution_y);

			// Grab the texel
			float4 c = tex2D(_MainTex, uv_custom);
			
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