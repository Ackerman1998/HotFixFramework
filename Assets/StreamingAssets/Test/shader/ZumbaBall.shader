Shader "ZumbaBall"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Mask ("Culling Mask", 2D) = "white" {}
    _Shadow ("Shadow (RGB)", 2D) = "white" {}
    _Alpha ("Alpha", float) = 1
    [Toggle] _ShadowActive ("Shadow active", float) = 1
    [Toggle] _UVPlay ("UV", float) = 1
    _ScrollY("Scroll Speed", Float) = 1.0
  }
  SubShader
  {
    Tags
    { 
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      //Cull Off
      Tags
      { 
      }
      LOD 100
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
     
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _Mask_ST;
      uniform float4 _Shadow_ST;
      uniform float _ShadowActive;
      uniform float _UVPlay;
      uniform float _Alpha;
      uniform sampler2D _Mask;
      uniform sampler2D _Shadow;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float2 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float2 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      int u_xlati0;
      float4 u_xlat1;
      int u_xlati1;
      float2 u_xlat2;
      float2 u_xlat3;
      int u_xlatb3;
      float2 u_xlat4;
      float u_xlat6;
      int u_xlati6;
      float u_xlat7;
      float u_xlat9;
      int u_xlatb9;
      float _ScrollY;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          //uv动画，移动Main贴图的uv坐标
          //if (_UVPlay==1) {
          //    out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex) + frac(float2(0, _ScrollY) * _Time.y);
          //}
          //else {
              //out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex) + frac(float2(0, _ScrollY));
              out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          //}
          out_v.texcoord1.xy = TRANSFORM_TEX(in_v.texcoord1.xy, _Mask);
          u_xlat0.x = (conv_mxt4x4_0(unity_ObjectToWorld).y / conv_mxt4x4_0(unity_ObjectToWorld).x);
          u_xlat3.x = min(abs(u_xlat0.x), 1);
          u_xlat6 = max(abs(u_xlat0.x), 1);
          u_xlat3.x = (u_xlat3.x / u_xlat6);
          u_xlat6 = (u_xlat3.x * u_xlat3.x);
          u_xlat9 = ((u_xlat6 * (-0.0121323196)) + 0.053681381);
          u_xlat9 = ((u_xlat9 * u_xlat6) + (-0.117350303));
          u_xlat9 = ((u_xlat9 * u_xlat6) + 0.193892494);
          u_xlat9 = ((u_xlat9 * u_xlat6) + (-0.332675606));
          u_xlat6 = ((u_xlat9 * u_xlat6) + 0.999979317);
          u_xlat3.x = (u_xlat3.x * u_xlat6);
          u_xlat6 = ((u_xlat3.x * (-2)) + 1.57079601);
          u_xlatb9 = (1<abs(u_xlat0.x));
          u_xlat9 = (u_xlatb9)?(1):(float(0));
          u_xlat3.x = ((u_xlat9 * u_xlat6) + u_xlat3.x);
          u_xlati6 = int(((0<u_xlat0.x))?((-1)):(0));
          u_xlati0 = int(((u_xlat0.x<0))?((-1)):(0));
          u_xlati0 = ((-u_xlati6) + u_xlati0);
          u_xlat0.x = float(u_xlati0);
          u_xlat6 = (u_xlat0.x * u_xlat3.x);
          u_xlat0.x = ((u_xlat3.x * u_xlat0.x) + 3.1400001);
          u_xlatb3 = (conv_mxt4x4_0(unity_ObjectToWorld).x<0);
          u_xlat0.x = (u_xlatb3)?(u_xlat0.x):(u_xlat6);
          u_xlat1.x = cos(u_xlat0.x);
          u_xlat0.x = sin(u_xlat0.x);
          u_xlat3.xy = TRANSFORM_TEX((in_v.texcoord2.xy), _Shadow);
          u_xlat4.xy = (u_xlat3.xy + float2(-0.5, (-0.5)));
          u_xlat9 = (u_xlat0.x * u_xlat4.y);
          u_xlat4.x = (u_xlat4.x * 4);
          u_xlat7 = (u_xlat1.x * u_xlat4.y);
          u_xlat2.y = ((u_xlat4.x * u_xlat0.x) + u_xlat7);
          u_xlat2.x = ((u_xlat4.x * u_xlat1.x) + (-u_xlat9));
          u_xlat0.xw = (u_xlat2.xy + float2(0.5, 0.5));
          u_xlati1 = int(_ShadowActive);
          out_v.texcoord2.xy = ((int(u_xlati1)!=0))?(u_xlat0.xw):(u_xlat3.xy);
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float3 u_xlat16_0;
      float4 u_xlat10_0;
      int u_xlati0_d;
      float3 u_xlatb0;
      float3 u_xlat10_1;
      float u_xlat16_7;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_Mask, in_f.texcoord1.xy).xyz;
          u_xlat0_d.xyz = (u_xlat10_0.xyz + float3(-0.5, (-0.5), (-0.5)));
          u_xlatb0.xyz = bool4(u_xlat0_d.xyzx < float4(0, 0, 0, 0)).xyz;
          u_xlatb0.x = (u_xlatb0.y || u_xlatb0.x);
          u_xlatb0.x = (u_xlatb0.z || u_xlatb0.x);
          if(((int(u_xlatb0.x) * int(65535))!=0))
          {
              discard;
          }
          u_xlati0_d = int(_ShadowActive);
          if((u_xlati0_d!=0))
          {
              u_xlat10_0 = tex2D(_Shadow, in_f.texcoord2.xy);
              u_xlat10_1.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz;
              u_xlat16_7 = ((-u_xlat10_0.w) + 1);
              u_xlat16_0.xyz = (u_xlat10_0.www * u_xlat10_0.xyz);
              u_xlat0_d.xyz = ((u_xlat10_1.xyz * float3(u_xlat16_7, u_xlat16_7, u_xlat16_7)) + u_xlat16_0.xyz);
          }
          else
          {
              u_xlat0_d.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz;
          }
          u_xlat0_d.w = _Alpha;
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
