Shader "Custom/TerrainShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Specular ("Specular", Color) = (0.2, 0.2, 0.2, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _BumpMap;
        fixed4 _Specular;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            o.Specular = _Specular;
            o.Smoothness = 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}