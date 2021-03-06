﻿Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SunLightColor("Sun Light Color", Color) = (0, 0, 0)
		_SunLightPosition("Sun Light Position", Vector) = (0.0, 0.0, 0.0)
		_Amplitude("Amplitude", Range(0.0, 2.0)) = 0.05
		_Speed("Speed", float) = 2.0
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

			uniform float3 _SunLightColor;
			uniform float3 _SunLightPosition;
			uniform float _Amplitude;
			uniform float _Speed;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Displacement of the water		
				float4 displacement = _Amplitude * float4(0.0f, sin(v.vertex.x + _Time.y * _Speed), 0.0f, 0.0f);

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex + displacement);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz + displacement.xyz));

				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				v.vertex += displacement;
				v.vertex = mul(UNITY_MATRIX_P, v.vertex);

				// Transform vertex in world coordinates to camera coordinates, and pass colour
				o.vertex = v.vertex;
				o.color = v.color;

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				// Our interpolated normal might not be of length 1
				float3 interpNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 1;
				float3 L = normalize(_SunLightPosition - v.worldVertex.xyz);
				float LdotN = dot(L, interpNormal);
				float3 dif = fAtt * _SunLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = 1;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				// Using classic reflection calculation:
				float3 R = normalize((2.0 * LdotN * interpNormal) - L);
				float3 spe = fAtt * _SunLightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);

				// Combine Phong illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = v.color.a;

				return returnColor;
			}
			ENDCG
        }
    }
}
