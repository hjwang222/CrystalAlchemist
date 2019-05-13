//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.9.6                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Previews/PreviewXATXQ2"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_Outline_Size_1("_Outline_Size_1", Range(1, 16)) = 1
_Outline_Color_1("_Outline_Color_1", COLOR) = (1,1,1,1)
_Outline_HDR_1("_Outline_HDR_1", Range(0, 2)) = 1
_InverseColor_Fade_1("_InverseColor_Fade_1", Range(0, 1)) = 1
_OperationBlend_Fade_1("_OperationBlend_Fade_1", Range(0, 1)) = 1
_SpriteFade("SpriteFade", Range(0, 1)) = 1.0

// required for UI.Mask
[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
[HideInInspector]_Stencil("Stencil ID", Float) = 0
[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
[HideInInspector]_ColorMask("Color Mask", Float) = 15

}

SubShader
{

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off 

// required for UI.Mask
Stencil
{
Ref [_Stencil]
Comp [_StencilComp]
Pass [_StencilOp]
ReadMask [_StencilReadMask]
WriteMask [_StencilWriteMask]
}

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};

sampler2D _MainTex;
float _SpriteFade;
float _Outline_Size_1;
float4 _Outline_Color_1;
float _Outline_HDR_1;
float _InverseColor_Fade_1;
float _OperationBlend_Fade_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float4 OutLine(float2 uv,sampler2D source, float value, float4 color, float HDR)
{

value*=0.01;
float4 mainColor = tex2D(source, uv + float2(-value, value))
+ tex2D(source, uv + float2(value, -value))
+ tex2D(source, uv + float2(value, value))
+ tex2D(source, uv - float2(value, value));

color *= HDR;
mainColor.rgb = color;
float4 addcolor = tex2D(source, uv);
if (mainColor.a > 0.40) { mainColor = color; }
if (addcolor.a > 0.40) { mainColor = addcolor; mainColor.a = addcolor.a; }
return mainColor;
}
float4 InverseColor(float4 txt, float fade)
{
float3 gs = 1 - txt.rgb;
return lerp(txt, float4(gs, txt.a), fade);
}
float4 OperationBlend(float4 origin, float4 overlay, float blend)
{
float4 o = origin; 
o.a = overlay.a + origin.a * (1 - overlay.a);
o.rgb = (overlay.rgb * overlay.a + origin.rgb * origin.a * (1 - overlay.a)) * (o.a+0.0000001);
o.a = saturate(o.a);
o = lerp(origin, o, blend);
return o;
}
float4 frag (v2f i) : COLOR
{
float4 _MainTex_1 = tex2D(_MainTex, i.texcoord);
float4 _Outline_1 = OutLine(i.texcoord,_MainTex,_Outline_Size_1,_Outline_Color_1,_Outline_HDR_1);
float4 InverseColor_1 = InverseColor(_Outline_1,_InverseColor_Fade_1);
float4 OperationBlend_1 = OperationBlend(_MainTex_1, InverseColor_1, _OperationBlend_Fade_1); 
float4 FinalResult = OperationBlend_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}
