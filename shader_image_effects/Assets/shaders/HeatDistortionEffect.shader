Shader "494/HeatDistortionEffect" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DistortionStrength("Strength of the distortion effect", Range(0, 0.1)) = 0.003
		_DistortionSpeed("Speed of the distortion effect", Range(0, 5)) = 1.5
		_DistortionInverseHorizontalWavelength("Inverse horizontal wave length of the heat waves", Range(0, 100)) = 25
		_DistortionInverseVerticalWavelength("Inverse vertical wave length of the heat waves", Range(0, 100)) = 25
	}
	SubShader{
	Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform float _DistortionStrength;
		uniform float _DistortionSpeed;
		uniform float _DistortionInverseHorizontalWavelength;
		uniform float _DistortionInverseVerticalWavelength;

		float EaseInOut (float t)
		{
			float sqt = t*t;
			return sqt / (2.0f * (sqt - t) + 1.0f);
		}

		float4 frag(v2f_img i) : COLOR{

			half2 uv_custom = i.uv;
			uv_custom = half2(i.uv.x + sin(_Time.z*_DistortionSpeed + i.uv.y * _DistortionInverseVerticalWavelength + i.uv.x * _DistortionInverseHorizontalWavelength) * _DistortionStrength, i.uv.y + sin(_Time.z*_DistortionSpeed + i.uv.y * _DistortionInverseVerticalWavelength + i.uv.x * _DistortionInverseHorizontalWavelength) * _DistortionStrength);

			// Grab the texel
			float4 c = tex2D(_MainTex, uv_custom);

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