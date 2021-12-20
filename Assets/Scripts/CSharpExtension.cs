using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[XLua.ReflectionUse]
[XLua.LuaCallCSharp]
/// <summary>
/// CSharp拓展方法
/// </summary>
public static partial class CSharpExtension 
{
    /// <summary>
    /// 加后缀 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="endName"></param>
    /// <returns></returns>
    public static string AddEndName(this string path, string endName)
    {
        path = path + "." + endName;
        return path;
    }
    /// <summary>
    /// 检测是否在ios环境下
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string CheckEnvironmentPath(this string path) {
#if UNITY_IOS
        path = "file://"+path;
#endif
        return path;
    }
    /// <summary>
    /// string to int
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public static int IntParse(this string nums) {
        return int.Parse(nums);
    }
    /// <summary>
    /// 字符串转为enum类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this string name) {
        return (T)Enum.Parse(typeof(T),name);
    }
    /// <summary>
    /// 返回字符第一次出现的下标
    /// </summary>
    /// <param name="nums"></param>
    /// <param name="_cstr"></param>
    /// <returns></returns>
    public static int FirstIndexOf(this string nums,char _cstr) {
        if (nums.Length==0||string.IsNullOrEmpty(nums)) {
            return 0;
        }
        char [] cc_Arr = nums.ToCharArray();
        for (int i=0;i<cc_Arr.Length;i++) {
            if (char.Equals(_cstr,cc_Arr[i])) {
                return i;
            }
        }
        return 0;
    }
    /// <summary>
    /// 替换字符串中所有的\为/
    /// </summary>
    /// <returns></returns>
    public static string ReplaceSpecial(this string msg) {
        msg = msg.Replace(@"\", @"/");
        return msg;
    }
    /// <summary>
    /// check game version code
    /// </summary>
    /// <param name="sourceVersion"></param>
    /// <param name="targetVersion"></param>
    /// <returns></returns>
    public static bool CheckIsNewVersion(this string sourceVersion, string targetVersion)
    {
        string[] sVerList = sourceVersion.Split('.');
        string[] tVerList = targetVersion.Split('.');

        if (sVerList.Length >= 3 && tVerList.Length >= 3)
        {
            try
            {
                int sV0 = int.Parse(sVerList[0]);
                int sV1 = int.Parse(sVerList[1]);
                int sV2 = int.Parse(sVerList[2]);
                int tV0 = int.Parse(tVerList[0]);
                int tV1 = int.Parse(tVerList[1]);
                int tV2 = int.Parse(tVerList[2]);

                if (tV0 > sV0)
                {
                    return true;
                }
                else if (tV0 < sV0)
                {
                    return false;
                }

                if (tV1 > sV1)
                {
                    return true;
                }
                else if (tV1 < sV1)
                {
                    return false;
                }

                if (tV2 > sV2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        return false;
    }

    public static bool IsNull(this UnityEngine.Object o) // 或者名字叫IsDestroyed等等
    {
        return o == null;
    }
}