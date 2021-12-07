//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "VRChat/Mobile/Standard Lite" {
Properties {
_MainTex ("Albedo", 2D) = "white" { }
_Color ("Color", Color) = (1,1,1,1)
_MetallicGlossMap ("Metallic(R) Smoothness(A) Map", 2D) = "white" { }
_Metallic ("Metallic", Range(0, 1)) = 1
_Glossiness ("Smoothness", Range(0, 1)) = 1
_BumpMap ("Normal Map", 2D) = "bump" { }
[Toggle(_EMISSION)] _EnableEmission ("Enable Emission", Float) = 0
_EmissionMap ("Emission", 2D) = "white" { }
_EmissionColor ("Emission Color", Color) = (1,1,1,1)
[ToggleOff] _SpecularHighlights ("Specular Highlights", Float) = 0
}
SubShader {
 LOD 200
 Tags { "RenderType" = "Opaque" }
 Pass {
  Name "FORWARD"
  LOD 200
  Tags { "LIGHTMODE" = "FORWARDBASE" "RenderType" = "Opaque" }
  GpuProgramID 28569
Program "vp" {
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "VERTEXLIGHT_ON" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_LINEAR" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "FOG_EXP2" "_EMISSION" "UNITY_SINGLE_PASS_STEREO" "_SPECULARHIGHLIGHTS_OFF" "_GLOSSYREFLECTIONS_OFF" }
"// shader disassembly not supported on DXBC"
}
}
}
}
Fallback "VRChat/Mobile/Diffuse"
}