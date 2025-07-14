Shader "Instanced/InterferencePattern"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Cull Off
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            
            // Core include for URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            float3 positions[32];
            float3 referenceSource;
            float3 referenceDirection;
            int count;
            float frequency;
            float phase;
            float amplitude;
            float speed;
            float time;

            float4 _Color;

            #pragma vertex vert
            #pragma fragment frag

            // Vertex input (object space)
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Vertex-to-fragment struct
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            // Wave function
            float wave_amplitude(float r)
            {
                float omega = frequency * TWO_PI;
                float k = omega / speed;
                return amplitude * sin(k * r - omega * time + phase);
            }

            // Vertex shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Convert object-space to homogeneous clip space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);

                // Convert to world-space position
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = worldPos;

                return OUT;
            }

            // Fragment shader
            float4 frag(Varyings IN) : SV_Target
            {
                float totalAmplitude = 0.0;

                // Sum contributions from point sources
                for (int idx = 0; idx < count; idx++)
                {
                    if (positions[idx].g == -1)
                        break;
                    float r = length(IN.worldPos - positions[idx]);
                    totalAmplitude += wave_amplitude(r);
                }

                // Add reference wave (plane wave from reference direction)
                float rRef = dot(referenceSource - IN.worldPos, referenceDirection);
                totalAmplitude += wave_amplitude(rRef);

                // Output intensity (abs for visualization)
                return float4(abs(totalAmplitude) * _Color.rgb, _Color.a);
            }
            ENDHLSL
        }
    }
}