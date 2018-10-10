using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


/*----------------------------------------------------------------
* 项目名称 ：Assets.Dev.Scripts.Game.Managers
* 项目描述 ：
* 类 名 称 ：GameManager
* 类 描 述 ：
* 所在的域 ：CHENGHAIXIAO-PC
* 命名空间 ：Assets.Dev.Scripts.Game.Managers
* 机器名称 ：CHENGHAIXIAO-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：ASUS
* 创建时间 ：2018/5/16 13:27:43
* 更新时间 ：2018/5/16 13:27:43
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ ASUS 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/

namespace Game
{
    public class GameManager : Base, IManager
    {
        private List<string> downloadFiles = new List<string>();
        //本地资源列表
        private Dictionary<string, string> inDic = new Dictionary<string, string>();
        //服务器资源列表
        private Dictionary<string, string> outDic = new Dictionary<string, string>();
        //需要下载更新的资源列表
        private Dictionary<string, string> loadDic = new Dictionary<string, string>();
        //存放资源目录
        private string persistentDataPath;
        //streaming目录
        private string streamingAssetsPath;
        //服务器目录
        private string serverurlPath;

        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Start()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            DontDestroyOnLoad(gameObject);  //防止销毁自己

            persistentDataPath = IOUtil.DataPath;  //数据目录
            streamingAssetsPath = IOUtil.AppContentPath(); //游戏包资源目录
            serverurlPath = AppConst.WebUrl;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;
            
            //检测资源
            CheckExtractResource(); 
        }

        /// <summary>
        /// 检测哪些资源需要更新
        /// </summary>
        /// <param name="_infileData"></param>
        /// <param name="_outfileData"></param>
        private void CheckResourceFile(string _infileData, string _outfileData)
        {
            inDic.Clear();
            outDic.Clear();
            loadDic.Clear();

            if (!string.IsNullOrEmpty(_infileData))
            {
                string[] infiles = _infileData.Split('\n');
                for (int i = 0; i < infiles.Length; i++)
                {
                    if (string.IsNullOrEmpty(infiles[i]))
                        continue;
                    string[] keyValue = infiles[i].Split('|');
                    if (keyValue.Length != 2)
                        continue;
                    if (!inDic.ContainsKey(keyValue[0]))
                    {
                        inDic.Add(keyValue[0], keyValue[1]);
                    }
                    else
                    {
                        DebugUtil.Log(string.Format("in files.txt 文件key{ 0 }重复", keyValue[0]));
                    }

                }
            }

            if (!string.IsNullOrEmpty(_outfileData))
            {
                string[] outfiles = _outfileData.Split('\n');
                for (int i = 0; i < outfiles.Length; i++)
                {
                    if (string.IsNullOrEmpty(outfiles[i]))
                        continue;
                    string[] keyValue = outfiles[i].Split('|');
                    if (keyValue.Length != 2)
                        continue;

                    if (!outDic.ContainsKey(keyValue[0]))
                    {
                        outDic.Add(keyValue[0], keyValue[1]);
                    }
                    else
                    {
                        DebugUtil.Log(string.Format("out files.txt 文件key{ 0 }重复", keyValue[0]));
                    }
                }
            }

            foreach (string key in outDic.Keys)
            {
                string inValue = null;
                string outValue = null;
                inDic.TryGetValue(key, out inValue);
                if (outDic.TryGetValue(key, out outValue) && !outValue.Equals(inValue))
                {
                    if (!loadDic.ContainsKey(key))
                    {
                        loadDic.Add(key, outValue);
                    }
                    else
                    {
                        DebugUtil.Log(string.Format("load files.txt 文件key{ 0 }重复", key));
                    }
                }
            }

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void CheckExtractResource()
        {
            if (AppConst.DebugMode)
            {//debug模式，不需要处理资源
                OnResourceInited();
            }
            else
            {
                string filesDataPath = IOUtil.DataPath + "files.txt"; ;  //persistentDataPath 路径中的files.txt文件
                if (File.Exists(filesDataPath))
                {//如果存在则说明已经做过streamingAssetsPath拷贝工作
                    //更新资源
                    StartCoroutine(OnUpdateResource());
                }
                else
                {//没有做过拷贝工作
                    //拷贝资源
                    StartCoroutine(OnExtractResource());
                }

            }
        }

        #region 先检测persistentDataPath 中的资源有没有同步成 streamingAssetsPath中的资源(项目资源以persistentDataPath路径资源为准)

        IEnumerator OnExtractResource()
        {
            if (!Directory.Exists(persistentDataPath))
                Directory.CreateDirectory(persistentDataPath);

            string outfile = streamingAssetsPath + "files.txt";//读的文件
            string infile = persistentDataPath + "files.txt";//写入的文件
            string message = "正在解包文件:>files.txt";
            byte[] outfileByte;
            //读取file文件
            WWW www = new WWW(outfile);
            yield return www;
            if (www.error != null)
            {
                DebugUtil.Log(www.error);
                OnUpdateFailed(string.Empty);
                yield break;
            }
            yield return 0;

            outfileByte = www.bytes;
            string infileData = File.Exists(infile) ? IOUtil.ReadFile(infile) : null;
            string outfileData = www.text;

            CheckResourceFile(infileData, outfileData);

            //资源是否是最新
            bool isResourcesLatest = loadDic.Count <= 0;

            DebugUtil.Log("OnExtractResource Time start--- " + Time.time);
            if (!isResourcesLatest)
            {
                //打开解压缩界面

                string[] files = outfileData.Split('\n');

                yield return new WaitForEndOfFrame();

                foreach (string key in loadDic.Keys)
                {
                    if (string.IsNullOrEmpty(key)) continue;

                    string f = key;
                    string dataPatafile = (persistentDataPath + f).Trim();
                    string path = Path.GetDirectoryName(dataPatafile);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    infile = persistentDataPath + f;
                    outfile = streamingAssetsPath + f;
                    message = "正在解包文件:>" + f;
                    DebugUtil.Log("正在解包文件:>" + infile);
                    //GameApp.GetManager<MessageController>().SendMessage(null, MsgType.TYPE_UPDATE_MESSAGE, message);

                    WWW www2 = new WWW(outfile);
                    yield return www2;

                    if (www2.isDone)
                    {
                        File.WriteAllBytes(infile, www2.bytes);
                    }
                    yield return new WaitForEndOfFrame();
                }

                File.WriteAllBytes(persistentDataPath + "files.txt", outfileByte);
                www.Dispose();
            }
            DebugUtil.Log("OnExtractResource Time end--- " + Time.time);
            message = "解包完成!!!";
            //GameApp.GetManager<MessageController>().SendMessage(null, MsgType.TYPE_UPDATE_MESSAGE, message);
            yield return new WaitForSeconds(0.1f);

            message = string.Empty;
            //释放完成，开始启动更新资源
            StartCoroutine(OnUpdateResource());
        }

        #endregion

        #region 和服务器比较资源，检测是否需要更新
        /// <summary>
        /// 启动更新下载，此处可启动线程下载更新
        /// </summary>
        IEnumerator OnUpdateResource()
        {
            if (!AppConst.UpdateMode)
            {
                OnResourceInited();
                yield break;
            }
            //string dataPath = Util.DataPath;  //数据目录
            //string url = AppConst.WebUrl;
            string message = string.Empty;
            string outfile = serverurlPath + "files.txt";
            string infile = persistentDataPath + "files.txt";
            DebugUtil.Log("LoadUpdate---->>>" + outfile);
            byte[] outfileBytes;
            string infileData;
            string outfileData;
            WWW www = new WWW(outfile);
            yield return www;
            if (www.error != null)
            {
                OnUpdateFailed(string.Empty);
                yield break;
            }
            outfileData = www.text;
            outfileBytes = www.bytes;
            infileData = File.Exists(infile) ? IOUtil.ReadFile(infile) : null;

            CheckResourceFile(infileData, outfileData);

            if (loadDic.Count > 0)
            {
                foreach (string key in loadDic.Keys)
                {
                    if (string.IsNullOrEmpty(key)) continue;

                    string f = key;
                    string localfile = (persistentDataPath + f).Trim();
                    string path = Path.GetDirectoryName(localfile);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileUrl = serverurlPath + f;

                    DebugUtil.Log(fileUrl);
                    message = "下载资源中>>" + fileUrl;
                    //GameApp.GetManager<MessageController>().SendMessage(null, MsgType.TYPE_UPDATE_MESSAGE, message);
                    /*
                    www = new WWW(fileUrl); yield return www;
                    if (www.error != null) {
                        OnUpdateFailed(path);   //
                        yield break;
                    }
                    File.WriteAllBytes(localfile, www.bytes);
                     */
                    //这里都是资源文件，用线程下载
                    BeginDownload(fileUrl, localfile);
                    while (!(IsDownOK(localfile))) { yield return new WaitForEndOfFrame(); }

                }
            }

            File.WriteAllBytes(persistentDataPath + "files.txt", outfileBytes);
            www.Dispose();
            yield return new WaitForEndOfFrame();

            DebugUtil.Log(">>>>>>>更新完成!!>>>>>>>");
            message = "更新完成!!";
            //GameApp.GetManager<MessageController>().SendMessage(null, MsgType.TYPE_UPDATE_MESSAGE, message);

            OnResourceInited();
        }

        #endregion

        void OnUpdateFailed(string file)
        {
            string message = "更新失败!>" + file;
            //GameApp.GetManager<MessageController>().SendMessage(null, MsgType.TYPE_UPDATE_MESSAGE, message);
        }

        /// <summary>
        /// 是否下载完成
        /// </summary>
        bool IsDownOK(string file)
        {
            return downloadFiles.Contains(file);
        }

        /// <summary>
        /// 线程下载
        /// </summary>
        void BeginDownload(string url, string file)
        {     //线程下载
            object[] param = new object[2] { url, file };

            ThreadEvent ev = new ThreadEvent();
            ev.Key = MsgType.TYPE_UPDATE_DOWNLOAD;
            ev.evParams.AddRange(param);
            GameApp.threadManager.AddEvent(ev, OnThreadCompleted);   //线程下载
        }

        /// <summary>
        /// 线程完成
        /// </summary>
        /// <param name="data"></param>
        void OnThreadCompleted(NotiData data)
        {
            switch (data.evName)
            {
                case MsgType.TYPE_UPDATE_EXTRACT:  //解压一个完成
                    //
                    break;
                case MsgType.TYPE_UPDATE_DOWNLOAD: //下载一个完成
                    downloadFiles.Add(data.evParam.ToString());
                    break;
            }
        }

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited()
        {
            GameApp.resourceManager.Initialize(AppConst.AssetDir, delegate ()
            {
                DebugUtil.Log("Initialize OK!!!");
                this.OnInitialize();
            });
        }

        private void OnInitialize()
        {
            //GameApp.GetManager<LuaManager>().InitStart();
            //LuaManager.DoFile("Logic/Game");         //加载游戏
            //LuaManager.DoFile("Logic/Network");      //加载网络
            //NetManager.OnInit();                     //初始化网络
            //Util.CallMethod("Game", "OnInitOK");     //初始化完成

            //BaseUI.CloseUI<GameuiMgrUpdateAsset>();
            Debug.Log("游戏准备完成。。。");
            //导入lua脚本
            GameApp.DoLoadLuaFiles();
            
            Action luaOnStart = GameApp.GlobalTable.GetInPath<Action>("Main.GameStart");
            luaOnStart();
        }
        

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy()
        {
            Debug.Log("~GameManager was destroyed");
        }
    }
}


