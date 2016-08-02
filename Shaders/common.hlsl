cbuffer VaporConstants : register(b0)
{
	float4x4 ViewMatrix;
	float4x4 ProjectionMatrix;
};

cbuffer VaporModelConstants : register(b1)
{
	float4x4 ModelViewMatrix;
	float4x4 ModelMatrix;
};