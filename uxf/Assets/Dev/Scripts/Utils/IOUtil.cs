/*******************************************
*��Ŀ���� ��Assets.zJCGame.Utils
*��Ŀ���� :
*�� �� �� : IOUtil
*��    �� : ASUS 
*����ʱ�� : 2017/4/21 13:13:03
*����ʱ�� : 2017/4/21 13:13:03
********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Game
{
    public class IOUtil
    {
        //public static string path = Application.dataPath;
        public static string path
        {
            get
            {
                string dir = "";
#if UNITY_EDITOR
                dir = Application.dataPath + "/";//·����/AssetsCaches/
#elif UNITY_IOS
            dir = Application.temporaryCachePath + "/";//·����Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches/
#elif UNITY_ANDROID
            dir = Application.persistentDataPath + "/";//·����/data/data/xxx.xxx.xxx/files/
#else
            dir = Application.streamingAssetsPath + "/";//·����/xxx_Data/StreamingAssets/
#endif
                return dir;
            }
        }

        /// <summary>
        /// ȡ�����ݴ��Ŀ¼
        /// </summary>
        public static string DataPath
        {
            get
            {
                string game = "gamedatafile";

                return Application.persistentDataPath + "/" + game + "/";
            }
        }

        public static string GetRelativePath()
        {
            return "file:///" + DataPath;

            //if (Application.isEditor)
            //    return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDir + "/";
            //else if (Application.isMobilePlatform || Application.isConsolePlatform)
            //    return "file:///" + DataPath;
            //else // For standalone player.
            //    return "file://" + Application.streamingAssetsPath + "/";
        }

        /// <summary>
        /// Ӧ�ó�������·��(�������streamingAssetsPath��
        /// </summary>
        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = "file://" + Application.streamingAssetsPath + "/";//Application.dataPath + "/" + AppConst.AssetDir + "/";
                    break;
            }
            return path;
        }

        public static void Write(string _name,string _data)
        {
            string fileName = path + _name;

            string dirName = path.TrimEnd('/');
            if (!Directory.Exists(dirName))
            {
                DirectoryInfo dir = Directory.CreateDirectory(dirName);
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
                if (fs != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(_data);
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(bytes, 0, bytes.Length);

                    fs.Flush();
                    fs.Close();
                }
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }
        }

        public static string Read(string _name)
        {
            string fileName = path + _name;

            string result = string.Empty;
            if (File.Exists(fileName))
            {
                FileStream fs = File.OpenRead(fileName);
                byte[] bytes ;
                int offset = 0;
                int len = 0;
                fs.Seek(0, SeekOrigin.Begin);
                do
                {
                    bytes = new byte[256];
                    len = fs.Read(bytes, 0, bytes.Length);
                    if (len == 0)
                        break;
                    offset += len;
                    result += Encoding.UTF8.GetString(bytes);
                }
                while (len > 0);

                fs.Close();
            }
            return result;
        }
        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="fileName">ȫ·��</param>
        /// <returns></returns>
        public static string ReadFile(string fileName)
        {
            string result;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            result = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            return result;
        }

        public static void Delete(string _name)
        {
            string fileName = path + _name;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public static FileInfo[] GetFiles()
        {
            string dirName = path;
            if (Directory.Exists(dirName))
            {
                DirectoryInfo dir = new DirectoryInfo(dirName);
                FileInfo[] files = dir.GetFiles("*.txt");
                return files;
            }
            return null;
        }
    }
}