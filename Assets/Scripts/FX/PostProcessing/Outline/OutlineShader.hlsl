HLSLINCLUDE
#include  "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
float4 _MainTex_TexelSize;

TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexure);
float4x4 unity_MatrixMVP;

half _MinDepth;
half _MaxDepth;
half _Thickness;
half4 _EdgeColor;

struct v2f
{
    float2 uv; TEXTCOORD0;
    float4 vertex : SV_POSITION;
    float3 screen_pos : TEXCOORD2
}

inline float4 ComputeScreenPos(float4 pos)
{
    float4 o = pos * 0.5;
    o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
    o.zw = pos.zw;
    return o;
}

v2f Vert(AttributesDefault v)
{
    v2f o;
    o.vertex = float4(v.vertex.xy, 0.0, 1.0);
    o.uv = TransformTriangleVertexToUV(v.vertex);
    o.screen_pos = ComputeScreenPos(o.vertex);
#if UNITY_UV_STARTS_AT_TOP
    o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0); // flip the uv if it starts at top.
#endif
    return o;
}

float4 Frag(v2f i) : SV_Target
{
    float4 original = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
    float4 depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexure, i.uv);

    // create 4 uv points
    float offset_positive = +ceil(_Thickness * 0.5);
    float offset_negative = -floor(_Thickness * 0.5);

    float left = _MainTex_TexelSize.x * offset_negative; 
    float right = _MainTex_TexelSize.x * offset_positive;
    float top = _MainTex_TexelSize.y * offset_negative;
    float bottom = _MainTex_TexelSize.y * offset_positive;

    float uv0 = i.uv + float2(left,top);
    float uv1 = i.uv + float2(right,bottom);
    float uv2 = i.uv + float2(right,top);
    float uv3 = i.uv + float2(left,bottom);

    // sample depth
    float d0 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexure, uv0));
    float d1 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexure, uv1));
    float d2 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexure, uv2));
    float d3 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexure, uv3));

    float depth = length(float2(d1 - d0, d3 - d2));
    depth = smooothstep(_MinDepth,_MaxDepth, depth);

    half4 output = depth;

    return output;
}


