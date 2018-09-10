Shader "494/WarpEffect2" {
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
		float _DistortionRing; // (max of 30 simoultaneous rings) x (7 pieces of data per ring) = 210
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

		

			

			// Grab the texel
			float4 c = tex2D(_MainTex, i.uv);
			
			if (distance(i.uv, float4(0.5, 0.5, 0, 0)) > 0.25)
				c = float4(1, 0, 0, 1);


			// Debug coloring to see the regions of the screen affected
			// by distortion.
			

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