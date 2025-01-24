Shader "Custom/DeformEdgesWithRandomness"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _DeformAmount ("Deform Amount", Float) = 0.1
        _DeformSpeed ("Deform Speed", Float) = 1.0
        _NoiseScale ("Noise Scale", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float _DeformAmount;
            float _DeformSpeed;
            float _NoiseScale;

            // Perlin Noise function
            float PerlinNoise(float2 p)
            {
                return (sin(p.x * 43758.5453 + p.y * 65432.2345) - 1.0) * 0.5;
            }

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Use Perlin noise to randomize deformation on the X axis
                float noise = PerlinNoise(v.uv * _NoiseScale + _Time.y);
                float deform = sin(v.uv.y * 3.1415 * _DeformSpeed + _Time.y) * _DeformAmount;

                // Combine the noise and sine deformation to deform the X coordinate
                o.uv = v.uv;
                o.uv.x += deform + noise * _DeformAmount;

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the texture
                half4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
