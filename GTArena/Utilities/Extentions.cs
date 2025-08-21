using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GTArena.Utilities
{
    public static class Extentions
    {
        public static object Invoke<T>(this T obj, string method, object[] paramaters, BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            var methodInfo = obj.GetType().GetMethod(method, flags);
            return methodInfo.Invoke(obj, paramaters);
        }
    }
}
