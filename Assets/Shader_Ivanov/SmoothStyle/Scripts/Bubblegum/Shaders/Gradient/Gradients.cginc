#ifndef GRADIENT_UTILITY
#define GRADIENT_UTILITY

//Colors
uniform fixed _GradientPower;
uniform fixed _GradientMix;

uniform fixed3 _ColorFront;
uniform fixed3 _ColorBack;
uniform fixed3 _ColorLeft;
uniform fixed3 _ColorRight;
uniform fixed3 _ColorTop;
uniform fixed3 _ColorBottom;

//Directions
static const half3 Forward = half3(0, 0, 1);
static const half3 Backward = half3(0, 0, -1);
static const half3 Left = half3(1, 0, 0);
static const half3 Right = half3(-1, 0, 0);
static const half3 Up = half3(0, 1, 0);
static const half3 Down = half3(0, -1, 0);
static const fixed3 WhiteColor = fixed3(1, 1, 1);

//Ambience
uniform fixed _EnableAmbience;
uniform fixed3 _AmbientColor;
uniform fixed3 _FogColor;
uniform half _WorldBottom;
uniform half _WorldTop;

//Vertex gradient computation
fixed3 ComputeVertexMultiplyGradient(fixed3 inputNormal)
{
	fixed3 normal = normalize(mul(unity_ObjectToWorld, half4(inputNormal, 0))).xyz;

	//Use for multiply blending
	return
		pow (
			lerp(WhiteColor, _ColorFront, clamp (normal.z, -_GradientMix, 1)) *
			lerp(WhiteColor, _ColorBack, clamp(-normal.z, -_GradientMix, 1)) *
			lerp(WhiteColor, _ColorLeft, clamp(-normal.x, -_GradientMix, 1)) *
			lerp(WhiteColor, _ColorRight, clamp(normal.x, -_GradientMix, 1)) *
			lerp(WhiteColor, _ColorTop, clamp(normal.y, -_GradientMix, 1)) *
			lerp(WhiteColor, _ColorBottom, clamp(-normal.y, -_GradientMix, 1)),
			_GradientPower);
}

//Vertex gradient computation
fixed3 ComputeVertexAdditiveGradient(fixed3 inputNormal)
{
	fixed3 normal = normalize(mul(unity_ObjectToWorld, half4(inputNormal, 0))).xyz;

	//Use for additive blending
	return 
		_ColorFront * saturate (normal.z) +
		_ColorBack * saturate (-normal.z) +
		_ColorLeft * saturate (-normal.x) +
		_ColorRight * saturate (normal.x) +
		_ColorTop * saturate (normal.y) +
		_ColorBottom * saturate (-normal.y);
}

//Fragment gradient computation
fixed4 ComputeFragmentGradient(fixed4 colorWithYPos)
{
	fixed3 ambience = lerp(_FogColor, colorWithYPos.rgb * _AmbientColor, saturate((colorWithYPos.a - _WorldBottom) / (_WorldTop - _WorldBottom)));
	return fixed4(lerp(colorWithYPos.rgb, ambience, _EnableAmbience), 1);
}

#endif // GRADIENT_UTILITY