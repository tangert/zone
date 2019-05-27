// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ColorCycle"
{
     Properties {
         _Color ("Color1", Color) = ( 1.0, 0.0, 0.0, 1.0 )
         _Color2 ("Color2", Color) = ( 0.0, 0.0, 1.0, 1.0 )
         _Speed ("Speed", float) = 0.2
     }
        SubShader {
         Pass {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
 
             #include "UnityCG.cginc"
             
             uniform fixed4 _Color;
             uniform fixed4 _Color2;
             uniform float  _Speed;
 
             struct vertOut {
                 float4 pos:SV_POSITION;
                 fixed4 color;
             };
 
             vertOut vert(appdata_base v) {
                 vertOut o;
                 o.pos = UnityObjectToClipPos (v.vertex);
                 o.color = lerp(_Color, _Color2, abs(fmod(_Time.a * _Speed, 2.0) - 1.0));
                 return o; 
             }
 
             fixed4 frag(vertOut i) : COLOR0 {
                    return i.color;
             }
 
             ENDCG
         }
     }
    //Properties
    //{
    //    _Color ("Color", Color) = (1,1,1,1)
    //    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    //    _Glossiness ("Smoothness", Range(0,1)) = 0.5
    //    _Metallic ("Metallic", Range(0,1)) = 0.0
    //}
    //SubShader
    //{
    //    Tags { "RenderType"="Opaque" }
    //    LOD 200

    //    CGPROGRAM
    //    // Physically based Standard lighting model, and enable shadows on all light types
    //    #pragma surface surf Standard fullforwardshadows

    //    // Use shader model 3.0 target, to get nicer looking lighting
    //    #pragma target 3.0

    //    sampler2D _MainTex;

    //    struct Input
    //    {
    //        float2 uv_MainTex;
    //    };

    //    half _Glossiness;
    //    half _Metallic;
    //    fixed4 _Color;

    //    // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
    //    // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
    //    // #pragma instancing_options assumeuniformscaling
    //    UNITY_INSTANCING_BUFFER_START(Props)
    //        // put more per-instance properties here
    //    UNITY_INSTANCING_BUFFER_END(Props)

    //    void surf (Input IN, inout SurfaceOutputStandard o)
    //    {
    //        // Albedo comes from a texture tinted by color
    //        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
    //        o.Albedo = c.rgb;
    //        // Metallic and smoothness come from slider variables
    //        o.Metallic = _Metallic;
    //        o.Smoothness = _Glossiness;
    //        o.Alpha = c.a;
    //    }
    //    ENDCG
    //}
    //FallBack "Diffuse"
}
