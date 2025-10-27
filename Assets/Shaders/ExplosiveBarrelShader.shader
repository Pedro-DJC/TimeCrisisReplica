Shader "Unlit/ExplosiveBarrelShader"
{
     Properties
    {
        _BaseColor ("Base Color", Color) = (1,0,0,1)
        _Intensity ("Glow Intensity", Range(0,5)) = 2
        _InflateAmount ("Inflate Amount", Range(0,1)) = 0.3
        _Speed ("Animation Speed", Range(0,10)) = 6

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _TimeY;
            float _InflateAmount;
            float _Speed;
            float _Intensity;
            float4 _BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                float inflate = sin(_Time.y * _Speed) * _InflateAmount;
                float3 dir = normalize(v.vertex.xyz);
                v.vertex.xyz += dir * inflate;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = abs(sin(_Time.y * _Speed));
                float3 colorA = float3(1, 0, 0); 
                float3 colorB = float3(1, 0.5, 0); 
                float3 colorC = float3(1, 1, 0);

                float3 mix1 = lerp(colorA, colorB, t);
                float3 mix2 = lerp(colorB, colorC, t * 0.5);
                float3 finalColor = lerp(mix1, mix2, t);

                finalColor *= _Intensity;

                return float4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}
