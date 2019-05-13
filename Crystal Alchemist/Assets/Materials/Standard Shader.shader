//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.9.6                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Customs/Standard Shader"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_Color ("Tint", Color) = (1,1,1,1)
[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
_Outline_Size_1("_Outline_Size_1", Range(1, 16)) = 1
_Outline_Color_1("_Outline_Color_1", COLOR) = (1,1,1,1)
_Outline_HDR_1("_Outline_HDR_1", Range(0, 2)) = 1
_InverseColor_Fade_1("_InverseColor_Fade_1", Range(0, 1)) = 1
_OperationBlend_Fade_1("_OperationBlend_Fade_1", Range(0, 1)) = 1
_SpriteFade("SpriteFade", Range(0, 1)) = 1.0

}

SubShader
{
Tags
{
"Queue" = "Transparent"
"IgnoreProjector" = "True"
"RenderType" = "Transparent"
"PreviewType" = "Plane"
"CanUseSpriteAtlas" = "True"

}

Cull Off
Lighting Off
ZWrite Off
Blend SrcAlpha OneMinusSrcAlpha


CGPROGRAM

#pragma surface surf Lambert vertex:vert  nolightmap nodynlightmap keepalpha noinstancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"
struct Input
{
float2 uv_MainTex;
float4 color;
};

float _SpriteFade;
float _Outline_Size_1;
float4 _Outline_Color_1;
float _Outline_HDR_1;
float _InverseColor_Fade_1;
float _OperationBlend_Fade_1;

void vert(inout appdata_full v, out Input o)
{
v.vertex.xy *= _Flip.xy;
#if defined(PIXELSNAP_ON)
v.vertex = UnityPixelSnap (v.vertex);
#endif
UNITY_INITIALIZE_OUTPUT(Input, o);
o.color = v.color * _Color * _RendererColor;
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
void surf(Input i, inout SurfaceOutput o)
{
float4 _MainTex_1 = tex2D(_MainTex, i.uv_MainTex);
float4 _Outline_1 = OutLine(i.uv_MainTex,_MainTex,_Outline_Size_1,_Outline_Color_1,_Outline_HDR_1);
float4 InverseColor_1 = InverseColor(_Outline_1,_InverseColor_Fade_1);
float4 OperationBlend_1 = OperationBlend(_MainTex_1, InverseColor_1, _OperationBlend_Fade_1); 
float4 FinalResult = OperationBlend_1;
o.Albedo = FinalResult.rgb* i.color.rgb;
o.Alpha = FinalResult.a * _SpriteFade * i.color.a;
o.Normal = UnpackNormal(float4(1,1,0,1));
clip(o.Alpha - 0.05);
}

ENDCG
}
Fallback "Sprites /Default"
}
