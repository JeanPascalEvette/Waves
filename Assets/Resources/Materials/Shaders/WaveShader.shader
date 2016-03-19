Shader "Custom/HeightDependentTint"
{
	Properties
	{
		_ColorMin("Tint Color At Min", Color) = (0,0,0,1)
		_ColorMax("Tint Color At Max", Color) = (1,1,1,1)
		_HeightMin("Height Min", Float) = -1
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

		fixed4 _ColorMin;
		fixed4 _ColorMax;
		float _HeightMin;

		struct Input
		{
			float2 uv_MainTex;
			float3 localPos;
		};
	#pragma surface surf Lambert vertex:vert

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			float h = IN.localPos.y / _HeightMin;
			fixed4 tintColor = lerp(_ColorMin.rgba, _ColorMax.rgba, h);
			o.Albedo = tintColor.rgb;
			o.Alpha = tintColor.a;
		}
	ENDCG
	}
}