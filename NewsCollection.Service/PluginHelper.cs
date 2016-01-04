using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NewLife.Log;
using NewsCollection.Core;
using NewsCollection.Plugin;

namespace NewsCollection.Service
{
    public static class PluginHelper
    {
        public static List<ICollect> FindPlugins()
        {
            List<ICollect> plugins = new List<ICollect>();

            //获取插件目录(Plugins)下所有文件
            string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins"));

            //只需要dll结尾的动态链接库
            foreach (string file in files.Where(file => file.ToLower().EndsWith(".dll")))
            {
                try
                {
                    //载入dll
                    var callingAssembly = Assembly.LoadFrom(file);
                    var implementors = typeof (ICollect).GetInstantiableImplementors(callingAssembly);
                    foreach (
                        var collect in
                            implementors.Select(type => (ICollect) Activator.CreateInstance(type))
                                .Where(collect => collect != null))
                    {
                        plugins.Add(collect);
                        XTrace.WriteLine($"Find Plugin: {file}");
                    }
                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex);
                }
            }

            return plugins;
        }
    }
}