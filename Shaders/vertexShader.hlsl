#include "common.hlsl"

//cbuffer TestBuffer  : register(b2)
//{
//	float4x4 testMatrix1;
//}

//static matrix Identity =
//{
//	{ 1, 0, 0, 0 },
//	{ 0, 1, 0, 0 },
//	{ 0, 0, 1, 0 },
//	{ 0, 0, 0, 1 }
//};

float4 main(float4 position : POSITION) : SV_POSITION
{
	//return position;

	//return mul(position, testMatrix1);
	//return mul(testMatrix1, position);
 
	//float4 pos = float4(position.xyz, 1.0);
	//pos = mul(pos, Model);
	//pos = mul(pos, View);
	//pos = mul(pos, Projection);
	//return pos;

	float4 pos = float4(position.xyz, 1.0);
	pos = mul(pos, ModelViewProjection);
	return pos;

    //return ProjectionMatrix * ViewMatrix * ModelMatrix * float4(position.xyz, 1.0);
    //return ProjectionMatrix * ModelViewMatrix * float4(position.xyz, 1.0);
}