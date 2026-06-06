Shader "ROFlexUI/Fonts/Bitmap Pixel Shadow"
{
    Properties
    {
        [PerRendererData] _MainTex("Font Atlas", 2D) = "white" {}

        _ShadowColor("Shadow Color", Color) = (0, 0, 0, 1)
        _ShadowOffset("Shadow Offset", Vector) = (1, -1, 0, 0)

        // Standard Unity UI stencil properties.
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)]
        _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector"= "True"
            "RenderType" = "Transparent"
        }
    
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Lighting Off
        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        // Standard transparent blending.
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct AppData
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct VertexToFragment
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;

            // Automatically populated by Unity:
            // x = 1 / texture width
            // y = 1 / texture height
            // z = texture width
            // w = texture height
            float4 _MainTex_TexelSize;

            fixed4 _ShadowColor;
            float2 _ShadowOffset;
            float4 _ClipRect;

            VertexToFragment vert(AppData input)
            {
                VertexToFragment output;

                output.worldPosition = input.vertex;
                output.vertex = UnityPixelSnap(
                    UnityObjectToClipPos(input.vertex)
                );
                output.uv = input.uv;
                output.color = input.color;

                return output;
            }

            fixed4 CompositeShadowBehindFace(
                fixed4 face,
                fixed4 shadow
            )
            {
                fixed outputAlpha = face.a + shadow.a * (1 - face.a);
                fixed3 outputRgb = lerp(shadow.rgb, face.rgb, face.a);

                return fixed4(outputRgb, outputAlpha);
            }

            fixed4 frag(VertexToFragment input) : SV_Target
            {
                fixed faceMask = tex2D(_MainTex, input.uv).a;
                float2 shadowUvOffset = _ShadowOffset.xy * _MainTex_TexelSize.xy;
                fixed shadowMask = tex2D(_MainTex, input.uv - shadowUvOffset).a;

                fixed4 face = fixed4(
                    input.color.rgb,
                    input.color.a * faceMask
                );

                fixed4 shadow = fixed4(
                    _ShadowColor.rgb,
                    _ShadowColor.a * shadowMask
                );
                fixed4 outputColor = CompositeShadowBehindFace(face, shadow);

                // Support RectMask2D clipping.
                #ifdef UNITY_UI_CLIP_RECT
                outputColor.a *= UnityGet2DClipping(
                    input.worldPosition.xy,
                    _ClipRect
                );
                #endif

                // Support alpha clipping when requested by Unity UI.
                #ifdef UNITY_UI_ALPHACLIP
                clip(outputColor.a - 0.001);
                #endif

                return outputColor;
            }

            ENDCG
        }
    }
}