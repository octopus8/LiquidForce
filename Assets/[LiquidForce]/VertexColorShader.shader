Shader "Custom/VertexColorShaderWithLightingAndNormalMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {} // New property for the normal map
        _Color ("Tint Color", Color) = (1,1,1,1)
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
                float3 normal : NORMAL;
                float4 tangent : TANGENT; // Tangent data is needed for tangent space
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 tangentLightDir : TEXCOORD1; // Tangent space light direction
            };

            sampler2D _MainTex;
            sampler2D _NormalMap; // New sampler for the normal map
            float4 _MainTex_ST;
            fixed4 _Color;
            
            // The line below was removed to fix the error:
            // float4 _WorldSpaceLightPos0;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Calculate the light direction in world space
                float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                
                // Construct the tangent to world matrix
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                // Create a TBN matrix
                float3x3 TBN = float3x3(worldTangent, worldBinormal, worldNormal);

                // Transform the light direction from world space to tangent space
                o.tangentLightDir = mul(TBN, worldLightDir);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Unpack the normal from the normal map in tangent space
                float3 tangentNormal = UnpackNormal(tex2D(_NormalMap, i.uv));

                // Normalize the tangent space light direction
                float3 tangentLightDir = normalize(i.tangentLightDir);
                
                // Calculate the lighting intensity in tangent space
                float lightIntensity = saturate(dot(tangentNormal, tangentLightDir));
                
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                fixed4 finalColor = texColor * i.color * _Color * lightIntensity;
                
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Standard"
}