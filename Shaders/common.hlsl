cbuffer VaporConstants : register(b0)
{
	float4x4 Model;
	float4x4 View;
	float4x4 Projection;
	float4x4 ModelView;
	float4x4 ModelViewProjection;
};

//cbuffer VaporModelConstants : register(b1)
//{
//	float4x4 ModelViewMatrix;
//	float4x4 ModelMatrix;
//};