using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using XLua;


/*----------------------------------------------------------------
* 项目名称 ：Assets.Dev.Scripts.Game
* 项目描述 ：
* 类 名 称 ：GameApp
* 类 描 述 ：
* 所在的域 ：CHENGHAIXIAO-PC
* 命名空间 ：Assets.Dev.Scripts.Game
* 机器名称 ：CHENGHAIXIAO-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：ASUS
* 创建时间 ：2018/5/4 13:23:17
* 更新时间 ：2018/5/4 13:23:17
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ ASUS 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/

namespace Game
{
    public class GameApp
    {
        //Lua管理器
        internal static LuaEnv luaenv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 
        


        public static LuaTable GlobalTable
        {
            get
            {
                return luaenv.Global;
            }
        }

        public static void DoLoadLuaFiles()
        {
            if (luaenv == null)
            {
                luaenv = new LuaEnv();
            }
            
            LuaEnv.CustomLoader method = CustomLoaderMethod;

            //启动lua脚本main，加载所有lua文件
            luaenv.AddLoader(method);

            //luaenv.DoString(@" require('main')");
            string filename = "main";
            luaenv.DoString(CustomLoaderMethod(ref filename));
        }
        
        private static byte[] CustomLoaderMethod(ref string fileName)
        {
            string path = null;

            if (AppConst.DebugMode && !Application.isMobilePlatform)
            {
                path = Application.dataPath + "/LuaFramework/";
            }
            else
            {
                path = IOUtil.DataPath;
            }

            fileName = path+"lua/" + fileName.Replace('.', '/') + ".lua"; 
            
            if (File.Exists(fileName))
            {
                byte[] content = File.ReadAllBytes(fileName); 
                
                return content;
            }
            else
            {
                return null;
            }
        }

        public static void Update()
        {
            //lua GC
            if (Time.time - GameApp.lastGCTime > GCInterval && luaenv != null)
            {
                luaenv.Tick();
                GameApp.lastGCTime = Time.time;
            }
        }

        public static void Clear()
        {
            if (luaenv != null)
                luaenv.Dispose();
        }

        #region Managers
        static Dictionary<string, object> dicManagers = new Dictionary<string, object>();

        //managers;
        public static SMSceneManager sms = null;
        //public static DataCache dataCache;
        //public static GlobalCache globalCache;
        
        //场景管理器
        //private static SceneController _sceneController;
        //public static SceneController sceneController
        //{
        //    get
        //    {
        //        if (_sceneController == null)
        //        {
        //            _sceneController = GetManager<SceneController>();
        //        }
        //        return _sceneController;
        //    }
        //}

        //相机管理器
        //private static CameraController _cameraController;
        //public static CameraController cameraController
        //{
        //    get
        //    {
        //        if (_cameraController == null)
        //        {
        //            _cameraController = GetManager<CameraController>();
        //        }
        //        return _cameraController;
        //    }
        //}

        //消息管理器
        //private static MessageController _messageController;
        //public static MessageController messageController
        //{
        //    get
        //    {
        //        if (_messageController == null)
        //        {
        //            _messageController = GetManager<MessageController>();
        //        }
        //        return _messageController;
        //    }
        //}

        //地图管理器
        //private static MapPathController _mapPathController;
        //public static MapPathController mapPathController
        //{
        //    get
        //    {
        //        if (_mapPathController == null)
        //        {
        //            _mapPathController = GetManager<MapPathController>();
        //        }
        //        return _mapPathController;
        //    }
        //}

        //游戏cd管理器
        //private static GameCdController _gameCdController;
        //public static GameCdController gameCdController
        //{
        //    get
        //    {
        //        if (_gameCdController == null)
        //        {
        //            _gameCdController = GetManager<GameCdController>();
        //        }
        //        return _gameCdController;
        //    }
        //}

        //定时器管理器
        //private static TimerController _timerController;
        //public static TimerController timerController
        //{
        //    get
        //    {
        //        if (_timerController == null)
        //        {
        //            _timerController = GetManager<TimerController>();
        //        }
        //        return _timerController;
        //    }
        //}

        //资源管理器
        private static ResourcesManager _resourceManager;
        public static ResourcesManager resourceManager
        {
            get
            {
                if (_resourceManager == null)
                {
                    _resourceManager = GetManager<ResourcesManager>();
                }
                return _resourceManager;
            }
        }

        

        //Thread管理器
        private static ThreadManager _threadManager;
        public static ThreadManager threadManager
        {
            get
            {
                if (_threadManager == null)
                {
                    _threadManager = GetManager<ThreadManager>();
                }
                return _threadManager;
            }
        }

        //ObjectPool管理器
        //private static EffectController _effectlManager;
        //public static EffectController effectManager
        //{
        //    get
        //    {
        //        if (_effectlManager == null)
        //        {
        //            _effectlManager = GetManager<EffectController>();
        //        }
        //        return _effectlManager;
        //    }
        //}

        //Game管理器
        private static GameManager _gameManager;
        public static GameManager gameManager
        {
            get
            {
                if (_gameManager == null)
                {
                    _gameManager = GetManager<GameManager>();
                }
                return _gameManager;
            }
        }

        //Net管理器
        //private static NetManager _netManager;
        //public static NetManager netManager
        //{
        //    get
        //    {
        //        if (_netManager == null)
        //        {
        //            _netManager = GetManager<NetManager>();
        //        }
        //        return _netManager;
        //    }
        //}

        //messagebox
        //private static GameMsgBoxController _msgBoxManager;
        //public static GameMsgBoxController msgBoxManager
        //{
        //    get
        //    {
        //        if (_msgBoxManager == null)
        //        {
        //            _msgBoxManager = GetManager<GameMsgBoxController>();
        //        }
        //        return _msgBoxManager;
        //    }
        //}

        
        private static GameObject goGameManager;
        static GameObject AppGameManager
        {
            get
            {
                if (goGameManager == null)
                {
                    goGameManager = GameObject.Find("GameApp");
                }
                if (goGameManager == null)
                {
                    goGameManager = new GameObject();
                    goGameManager.name = "GameApp";
                }
                return goGameManager;
            }
        }

        /// <summary>
        /// 添加Unity对象
        /// </summary>
        public static T AddManager<T>() where T : class, new()
        {
            string typeName = typeof(T).Name;
            object result = null;
            dicManagers.TryGetValue(typeName, out result);
            if (result != null)
            {
                return (T)result;
            }
            T c = new T();
            dicManagers.Add(typeName, c);
            return default(T);
        }

        /// <summary>
        /// 添加Unity对象
        /// </summary>
        public static T AddManagerOnGameObject<T>(GameObject _go) where T : Component
        {
            string typeName = typeof(T).Name;
            object result = null;
            dicManagers.TryGetValue(typeName, out result);
            if (result != null)
            {
                return (T)result;
            }
            Component c = _go.GetComponent<T>();
            if (c == null)
                c = _go.AddComponent<T>();
            dicManagers.Add(typeName, c);
            return default(T);
        }

        /// <summary>
        /// 获取系统管理器
        /// </summary>
        private static T GetManager<T>() where T : class
        {
            string typeName = typeof(T).Name;
            if (!dicManagers.ContainsKey(typeName))
            {
                return default(T);
            }
            object manager = null;
            dicManagers.TryGetValue(typeName, out manager);
            return (T)manager;
        }

        /// <summary>
        /// 删除管理器
        /// </summary>
        public static void RemoveManager<T>() where T : class
        {
            string typeName = typeof(T).Name;
            if (!dicManagers.ContainsKey(typeName))
            {
                return;
            }
            object manager = null;
            dicManagers.TryGetValue(typeName, out manager);
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                GameObject.Destroy((Component)manager);
            }
            dicManagers.Remove(typeName);
        }

        #endregion

        public static void Init()
        {
            //if (dataCache != null)
            //    return;

            //dataCache = new DataCache();
            //globalCache = new GlobalCache();
            //sms = new SMSceneManager(SMSceneConfigurationLoader.LoadActiveConfiguration("Config"));
            //sms.LevelProgress = new SMLevelProgress(sms.ConfigurationName);

            //RegisterAllUI();
            

            //_messageController = AddManager<MessageController>();
            //_mapPathController = AddManager<MapPathController>();
            //_gameCdController = AddManager<GameCdController>();
            //_timerController = AddManager<TimerController>();
            //_msgBoxManager = AddManager<GameMsgBoxController>();


            _resourceManager = AddManagerOnGameObject<ResourcesManager>(AppGameManager);
            AddManagerOnGameObject<ThreadManager>(AppGameManager);
            AddManagerOnGameObject<GameManager>(AppGameManager);
            //AddManagerOnGameObject<NetManager>(AppGameManager);
            //AddManagerOnGameObject<EffectController>(AppGameManager);
            AddManagerOnGameObject<LogCallback>(AppGameManager);

        }

        public static void RegisterAllUI()
        {
           
        }

        public static void ClearMemory()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
        

    }
}


