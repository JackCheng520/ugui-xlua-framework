using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Game
{
    /// <summary>
    /// 描述:资源下载配置类.
    /// </summary>
    public class DownLoadSetting
    {
        //版本信息文件名,服务端和客户端文件名一致.
        private static string versionFileName = "version";
        //资源路径映射表文件名.
        private static string dlcPathsMapFileName = "DLCPathsMap.txt";

        //资源路径映射表.
        private static Dictionary<string, string> assetPathsMap = null;

        //DownloadableContent目录层次结构.
        /************************************************************************/
        /*     DownloadableContent             
         *                      |             
         *                      IOS             
         *                      |             
         *                      Android             
         *                      |             
         *                      Windows             
         *
        /************************************************************************/
        //顶级目录.
        public static string rootDir = "GameBuildRes";
        //iOS资源目录
        public static string iOSDir = "ios";
        //Android资源目录.
        public static string androidDir = "android";
        //windows资源目录.
        public static string winDir = "windows";
        //本地化语言.
        public static string language = string.Empty;

        /// <summary>
        /// web服务器项目资源URL.
        /// </summary>
        public static string ProjectDLCURL()
        {
            return null;
            //WebServerPreConfig config = Resources.Load<WebServerPreConfig>("Config/WebServerPreference") as WebServerPreConfig;
            ////%20在URL路径中代表空格.
            //String projectDLCURL = config.projectAssetsURL.Replace(" ", "%20");
            //string projectDLCURL = DataCache.versionCheckVo.resourceDownloadUrl + "/resource/download/MMO/DownloadableContent/";
            
            ////string projectDLCURL = "http://cdn.mmo.menghuanzhanji.com/resource/download/MMO/DownloadableContent/";//外网
            ////string projectDLCURL = "http://ssl-download1.longlin.17m3.com/resource/download/MMO/DownloadableContent/";//阿里云
            ////string projectDLCURL = "http://sg.mmo.menghuanzhanji.com/resource/download/MMO/DownloadableContent/";//腾讯云，测试
            //return projectDLCURL;   
        }

        public static string LocalVersionFileName()
        {
            return versionFileName + ".txt";
        }

        public static string RemoteVersionFileName()
        {
            return versionFileName + "_" + Platform() + ".txt";
        }

        public static string AssetPathsMapFileName()
        {
            return dlcPathsMapFileName;
        }

        public static Dictionary<string,string> AssetPathsMap()
        {
            if (assetPathsMap == null)
            {
                assetPathsMap = new Dictionary<string, string>();
                //读取文件.
                FileStream fs = new FileStream(LocalDir() + AssetPathsMapFileName(), FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] assetPathInfo = line.Split((",").ToCharArray());
                    if (assetPathInfo.Length != 2) continue;
                    assetPathsMap.Add(assetPathInfo[0], assetPathInfo[1]);
                }
                sr.Close();
                fs.Close();
            }
            return assetPathsMap;
        }

        public static string RootDir()
        {
            string path = string.Empty;
#if UNITY_STANDALONE_WIN
            path = ShellPathNameConvert.ToLongPathName(Application.temporaryCachePath);
#else
            path = Application.persistentDataPath;
#endif
            return path;
        }

        /// <summary>
        /// 获取本地dlc资源缓存目录.
        /// </summary>
        public static string LocalDir()
        {
            return RootDir() + "/GameBuildRes/";
        }

        /// <summary>
        /// 平台判断.
        /// </summary>
        public static string Platform()
        {
            string platform = string.Empty;
#if UNITY_ANDROID
		    platform = "android";
#elif UNITY_IPHONE
		    platform = "ios";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            platform = "windows";
#endif
            return platform;
        }

    }
}