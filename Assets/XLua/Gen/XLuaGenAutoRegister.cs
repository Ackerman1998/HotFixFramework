#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(CSharpExtension), CSharpExtensionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.BaseClass), TutorialBaseClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.TestEnum), TutorialTestEnumWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClass), TutorialDerivedClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.ICalc), TutorialICalcWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClassExtensions), TutorialDerivedClassExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClass.TestEnumInner), TutorialDerivedClassTestEnumInnerWrap.__Register);
        
        
        
        }
        
        static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            
            translator.AddInterfaceBridgeCreator(typeof(Tutorial.CSCallLua.ItfD), TutorialCSCallLuaItfDBridge.__Create);
            
        }
        
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter(Init);
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
		delegate string __GEN_DELEGATE0( string path,  string endName);
		
		delegate string __GEN_DELEGATE1( string path);
		
		delegate int __GEN_DELEGATE2( string nums);
		
		delegate int __GEN_DELEGATE3( string nums,  char _cstr);
		
		delegate string __GEN_DELEGATE4( string msg);
		
		delegate bool __GEN_DELEGATE5( string sourceVersion,  string targetVersion);
		
		delegate bool __GEN_DELEGATE6( UnityEngine.Object o);
		
		delegate string __GEN_DELEGATE7( string path,  string endName);
		
		delegate string __GEN_DELEGATE8( string path);
		
		delegate int __GEN_DELEGATE9( string nums);
		
		delegate int __GEN_DELEGATE10( string nums,  char _cstr);
		
		delegate string __GEN_DELEGATE11( string msg);
		
		delegate bool __GEN_DELEGATE12( string sourceVersion,  string targetVersion);
		
		delegate bool __GEN_DELEGATE13( UnityEngine.Object o);
		
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
				{typeof(string), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE0(CSharpExtension.AddEndName)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE1(CSharpExtension.CheckEnvironmentPath)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE2(CSharpExtension.IntParse)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE3(CSharpExtension.FirstIndexOf)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE4(CSharpExtension.ReplaceSpecial)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE5(CSharpExtension.CheckIsNewVersion)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE7(CSharpExtension.AddEndName)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE8(CSharpExtension.CheckEnvironmentPath)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE9(CSharpExtension.IntParse)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE10(CSharpExtension.FirstIndexOf)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE11(CSharpExtension.ReplaceSpecial)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE12(CSharpExtension.CheckIsNewVersion)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Object), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE6(CSharpExtension.IsNull)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE13(CSharpExtension.IsNull)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
