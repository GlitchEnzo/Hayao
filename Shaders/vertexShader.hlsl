#include "common.hlsl"

// transform point from model space to projection space
//float4 main(float4 position : POSITION) : SV_POSITION
//{
//	//return position;
//
//	//return mul(position, testMatrix1);
//	//return mul(testMatrix1, position);
// 
//	//float4 pos = float4(position.xyz, 1.0);
//	//pos = mul(pos, Model);
//	//pos = mul(pos, View);
//	//pos = mul(pos, Projection);
//	//return pos;
//
//	float4 pos = float4(position.xyz, 1.0);
//	pos = mul(pos, ModelViewProjection);
//	return pos;
//
//    //return ProjectionMatrix * ViewMatrix * ModelMatrix * float4(position.xyz, 1.0);
//    //return ProjectionMatrix * ModelViewMatrix * float4(position.xyz, 1.0);
//}

struct VSOut
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

// transform point and pass color
VSOut main(float4 position : POSITION, float4 color : COLOR)
{
	float4 pos = float4(position.xyz, 1.0);
	pos = mul(pos, ModelViewProjection);

	VSOut output;
	output.position = pos;
	output.color = color;

	return output;
}