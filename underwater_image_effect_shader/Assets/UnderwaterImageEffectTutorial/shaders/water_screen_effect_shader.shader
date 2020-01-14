Shader "494/water_screen_effect_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            /* This is essentially the only function you need to worry about for this effect.
             * It controls both (1) where pixels are rendered, and (2) what color they are. */
            fixed4 frag (v2f i) : SV_Target
            {
                /* We use the standard, boring UV information for this "pixel" (the "i" parameter above)...
                 * ...and manipulate it a bit to make it represent UV information for pixels around us.
                 * This is what provides the "wave". Pixels represent end up representing one another on the horizontal axis. */
                float2 custom_uv = float2(i.uv.x + 0.007 * sin(_Time.z*1 + i.uv.y*10), i.uv.y);

                /* We grab the "incorrect" pixel from the "Normal" camera shot using our special custom UV. */
                fixed4 col = tex2D(_MainTex, custom_uv);


                /* Lerp the pixel to some intermediate color between the normal camera shot pixel and a pure blue pixel. 
                 * We use "i.uv.y" to make this lerp more pronounced for pixels lower on the screen.
                 * This creates a darker blue fade effect that reminds one of deep, dark waters. */
                float4 blue_color = float4(0, 0, 1, 1);
                float4 camera_color = col;
                col.rgb = lerp(camera_color, blue_color, 1.0 - i.uv.y);

                return col;
            }
            ENDCG
        }
    }
}
