Texture2D ShaderTexture : register(t0);
SamplerState Sampler : register(s0);

struct PSIn
{
	float4 position : SV_POSITION;
	float4 uv : TEXCOORD;
};

float4 main(PSIn input) : SV_TARGET
{
	return ShaderTexture.Sample(Sampler, input.uv);
}