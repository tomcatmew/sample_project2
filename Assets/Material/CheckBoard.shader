Shader "Unlit/CheckBoard"
{
    //A simple checkboard shader for the floor
    //Edit Checkboard color in editor
    Properties
    {
        _Scale("Pattern Size", Range(0,10)) = 1
        _EvenColor("Color 1", Color) = (0,0,0,1)
        _OddColor("Color 2", Color) = (1,1,1,1)
    }
        SubShader
    {
        Pass
        {
            Tags {"LightMode" = "ForwardBase" "RenderType" = "Opaque" "Queue" = "Geometry"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "AutoLight.cginc"
            float _Scale;
            float4 _EvenColor;
            float4 _OddColor;

            struct v2f
            {
                //float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };
            v2f vert(appdata_base v)
            {
                // Shadow feature inherited from Unity built-in diffuse shader
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                //o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                //calculate the position in clip space to render the object
                o.pos = UnityObjectToClipPos(v.vertex);
                //calculate the position of the vertex in the world
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //Checkboard feature color implementation
                //scale the position to adjust for shader input and floor the values so we have whole numbers
                float3 adjustedWorldPos = floor(i.worldPos / _Scale);
                //add different dimensions 
                float chessboard = adjustedWorldPos.x + adjustedWorldPos.y + adjustedWorldPos.z;
                //divide it by 2 and get the fractional part, resulting in a value of 0 for even and 0.5 for odd numbers.
                chessboard = frac(chessboard * 0.5);
                //multiply it by 2 to make odd values white instead of black
                chessboard *= 2;
                //interpolate between color for even fields (0) and color for odd fields (1)
                float4 col = lerp(_EvenColor, _OddColor, chessboard);

                // Shadow feature inherited from Unity built-in diffuse shader
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }
        //UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
        FallBack "Standard" 
}
