﻿Shader "Hexasphere/HexaGridNoExtrusionSpotlight" {
    Properties {
        _Color ("Main Color", Color) = (1,0.5,0.5,1)
		[HideInInspector] _ZWrite ("__zw", Float) = 1.0
        _Radius ("Cutoff Radius", Range(0.1, 500)) = 2
        [HideInInspector] _Center ("Cutoff Center", Vector) = (0,0,0,0)
    }


    SubShader {
        Tags { "Queue" = "Geometry-2" "RenderPipeline" = "UniversalPipeline" }
        Pass {
            ZWrite[_ZWrite]
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma enable_d3d11_debug_symbols
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            fixed4 _Color;
            float4 _Center;
            float _Radius;

            struct appdata {
                float4 vertex   : POSITION;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 pos      : SV_POSITION;
                float dist      : SV_CLIPDISTANCE;
                //SHADOW_COORDS(0)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v) {
                v2f o;
                //UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float dx = length(_Center.x - worldPos.x);
                float dy = length(_Center.y - worldPos.y);
                float dz = length(_Center.z - worldPos.z);
                float distSqr = dx*dx+dy*dy+dz*dz;
                o.dist = sqrt(distSqr);//clamp(_Radius - distSqr,0,1);
                //clip(_Radius - distSqr);

                o.pos = UnityObjectToClipPos(v.vertex);
                
                //TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = _Color;
                clip(_Radius - i.dist);

                fixed whitePulse = (cos((_Time * -35) + (i.dist * 0.5)) + 1) / 2;
                //(((_Time * 50) + i.dist) % _Radius) / _Radius;
                whitePulse = pow(whitePulse, 100);
                color.rgb = lerp(color.rgb, fixed3(1, 1, 1), fixed3(whitePulse, whitePulse, whitePulse));

                return color;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}