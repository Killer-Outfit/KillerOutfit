Shader "Custom/Explosion"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white" {}

        _Color ("Color", Color) = (1,1,1,1)
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
		_DissolveAmount("DissolveAmount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _MainTex;
        sampler2D _DissolveTex;
		half _DissolveAmount;
		fixed4 _Color;
		fixed4 _OutlineColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half dissolve_value = tex2D(_DissolveTex, IN.uv_MainTex).r * 2;
			clip(dissolve_value - _DissolveAmount);
			o.Emission = _OutlineColor.rgb * step(dissolve_value - _DissolveAmount, 0.05f);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb * 20;
			o.Metallic = -5;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
