#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <math.h>
#include <magnification.h>

float opacity = 1.0f;
float brightness = 0.0f;
float contrast = 1.0f;
float saturation = 1.0f;
float rots = sin(0);
float rotc = cos(0);
const float pi = acos(-1);
const float lumR = 0.3086f;
const float lumG = 0.6094f;
const float lumB = 0.0820f;
const float lumr1 = 0.213f;
const float lumr2 = 0.787f;
const float lumr3 = 0.143f;
const float lumg1 = 0.715f;
const float lumg2 = 0.285f;
const float lumg3 = 0.140f;
const float lumb1 = 0.072f;
const float lumb2 = 0.928f;
const float lumb3 = 0.283f;
float sr = (1.0f - saturation) * lumR;
float sg = (1.0f - saturation) * lumG;
float sb = (1.0f - saturation) * lumB;
float t = (1.0f - contrast) / 2.0f;
MAGCOLOREFFECT magColorEffect;

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason,LPVOID lpvReserved) {
	return TRUE;
}

void UpdateMagColorEffect() {
	magColorEffect = {
		opacity * contrast * (sr + saturation * (lumr1 + rotc * lumr2 + rots * -lumr1))		,	opacity * contrast * (sr + saturation * (lumr1 + rotc * -lumr1 + rots * lumr3))		,	opacity * contrast * (sr + saturation * (lumr1 + rotc * -lumr1 + rots * -lumr2))	, 0.0f, 0.0f,
		opacity * contrast * (sg + saturation * (lumg1 + rotc * -lumg1 + rots * -lumg1))	,	opacity * contrast * (sg + saturation * (lumg1 + rotc * lumg2 + rots * lumg3))		,	opacity * contrast * (sg + saturation * (lumg1 + rotc * -lumg1 + rots * lumg1))		, 0.0f, 0.0f,
		opacity * contrast * (sb + saturation * (lumb1 + rotc * -lumb1 + rots * lumb2))		,	opacity * contrast * (sb + saturation * (lumb1 + rotc * -lumb1 + rots * -lumb3))	,	opacity * contrast * (sb + saturation * (lumb1 + rotc * lumb2 + rots * lumb1))		, 0.0f, 0.0f,
												0.0f										,											0.0f										,											0.0f										, 1.0f, 0.0f,
											t + brightness									,										t + brightness									,										t + brightness									, 0.0f, 1.0f
	};
	MagSetFullscreenColorEffect(&magColorEffect);
}

void SetScreenRot(float deg) {
	float rad = deg * pi / 180.0f;
	rotc = cos(rad);
	rots = sin(rad);
	UpdateMagColorEffect();
}

void SetScreenOp(float newop) {
	opacity = newop;
	UpdateMagColorEffect();
}

void SetScreenBright(float newbright) {
	brightness = newbright;
	UpdateMagColorEffect();
}

void SetScreenCon(float newcon) {
	contrast = newcon;
	t = (1.0f - contrast) / 2.0f;
	UpdateMagColorEffect();
}

void SetScreenSat(float newsat) {
	saturation = newsat;
	sr = (1.0f - saturation) * lumR;
	sg = (1.0f - saturation) * lumG;
	sb = (1.0f - saturation) * lumB;
	UpdateMagColorEffect();
}