#include "common.hlsl"

float4x4 test;

cbuffer TestBuffer // : register(b1)
{
	float4x4 testMatrix1;
}

static matrix Identity =
{
	{ 1, 0, 0, 0 },
	{ 0, 1, 0, 0 },
	{ 0, 0, 1, 0 },
	{ 0, 0, 0, 1 }
};

float4 main(float4 position : POSITION) : SV_POSITION
{
	//return position;
	//return mul(position, testMatrix1);
	return mul(testMatrix1, position);
}