#include "common.hlsl"

struct VSIn
{
	float4 position : POSITION;
	float4 color : COLOR;
};

struct VSOut
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

VSOut main(VSIn input)
{
	float4 pos = float4(input.position.xyz, 1.0);
	pos = mul(pos, ModelViewProjection);

	VSOut output;
	output.position = pos;
	output.color = input.color;

	return output;
}