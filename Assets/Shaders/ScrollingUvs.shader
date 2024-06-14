Shader "Unlit/ScrollingUvs"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WindowScrollSpeed ("Window Scroll Speed", int) = 0
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _WindowScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed xscrollValue = (_WindowScrollSpeed * _Time.y % 100) / 100;
                fixed2 newUv = i.uv + fixed2(xscrollValue, 0);
                fixed4 col = tex2D(_MainTex, newUv);
                return col;
            }
            ENDCG
        }
    }
}
