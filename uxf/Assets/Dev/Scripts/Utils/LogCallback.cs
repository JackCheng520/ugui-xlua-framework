using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// log 查看工具
/// </summary>
public class LogCallback : MonoBehaviour
{
    #region class
    /// <summary>
    /// log信息
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 字符串解析
        /// </summary>
        public enum AnalyzeType : int
        {
            All,
            UI,
            Eff,
            Mod,
            Anim,
            Other,
        }
        /// <summary>
        /// log类型
        /// </summary>
        public enum Type : int
        {
            All,
            Log,
            Warning,
            Error,
        }

        /// <summary>
        /// 在list里的位置
        /// </summary>
        public int ID;
        /// <summary>
        /// 内容 人工输入的内容
        /// </summary>
        public string Content;
        /// <summary>
        /// 异常
        /// </summary>
        public string StackTrace;
        /// <summary>
        /// log类型
        /// </summary>
        public Type LogType;
        /// <summary>
        /// 解析类型
        /// </summary>
        public AnalyzeType Analyze;
        /// <summary>
        /// 字体类型
        /// </summary>
        private GUIStyle gs;

        public static string[] EnumToString(System.Type _enum)
        {
            return Enum.GetNames(_enum);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogInfo(int _id, string _content, string _stackTrace, Type _type, AnalyzeType _analyze)
        {
            this.ID = _id;
            this.Content = _content;
            this.StackTrace = _stackTrace;
            this.LogType = _type;
            this.Analyze = _analyze;

            gs = new GUIStyle();
            gs.alignment = TextAnchor.MiddleCenter;
            gs.wordWrap = true;
            gs.normal.textColor = Color.white;
            CheckFontSize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogInfo(int _id, string _content, string _stackTrace, Type _type)
        {
            this.ID = _id;
            this.Content = _content;
            this.StackTrace = _stackTrace;
            this.LogType = _type;

            gs = new GUIStyle();
            gs.alignment = TextAnchor.MiddleCenter;
            gs.wordWrap = true;
            gs.normal.textColor = Color.white;
            CheckFontSize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_content"></param>
        /// <param name="_stackTrace"></param>
        /// <param name="_type"></param>
        public LogInfo(string _content, string _stackTrace, Type _type)
        {
            this.Content = _content;
            this.StackTrace = _stackTrace;
            this.LogType = _type;

            gs = new GUIStyle();
            gs.alignment = TextAnchor.MiddleCenter;
            gs.wordWrap = true;
            gs.normal.textColor = Color.white;
            CheckFontSize();
        }

        /// <summary>
        /// 所有信息 画label
        /// </summary>
        public void DrawAll()
        {
            DrawContent();
            DrawStackTrace();
        }

        /// <summary>
        /// 内容 画label
        /// </summary>
        public void DrawContent()
        {
            GUI.contentColor = SetColor();
            GUILayout.Box(this.Content, gs);
            DrawLine();
        }

        /// <summary>
        /// 异常 画label
        /// </summary>
        public void DrawStackTrace()
        {
            GUI.contentColor = SetColor();
            GUILayout.Box(this.StackTrace, gs);
            DrawLine();
        }

        /// <summary>
        /// 内容 画button
        /// </summary>
        /// <returns></returns>
        public bool BtnContent()
        {
            GUI.contentColor = SetColor();
            bool b = GUILayout.Button(new GUIContent(this.Content, this.StackTrace), gs);
            DrawLine();
            return b;
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <returns></returns>
        private Color SetColor()
        {
            switch (this.LogType)
            {
                case Type.Log:
                    return Color.white;
                case Type.Warning:
                    return Color.yellow;
                case Type.Error:
                    return Color.red;
                default:
                    return Color.cyan;
            }
        }

        private void DrawLine()
        {
            GUI.contentColor = Color.white;
            GUILayout.Label("***********************************************************************************", gs);
        }

        //字体大小
        private void CheckFontSize()
        {
            float size = Screen.height / 640f * 22f;
            this.gs.fontSize = (int)size;
            ////Debug.Log("UI -- " + size);
        }
    }
    #endregion

    /// <summary>
    /// 窗口种类
    /// </summary>
    private enum WindowStatus
    {
        Logo,
        Main,
        Info,
        GM,
    }
    /// <summary>
    /// 所有不重复信息list
    /// </summary>
    private List<LogInfo> LogInfoList;
    /// <summary>
    /// 是否显示UI
    /// </summary>
    private static bool isShow;
    /// <summary>
    /// UI区域
    /// </summary>
    private Rect windowRect;
    /// <summary>
    /// 背景颜色深度
    /// </summary>
    private float BGNum = 0f;
    private readonly float BGMAXNUM = 7f;
    /// <summary>
    /// 当前窗口类型
    /// </summary>
    private WindowStatus mWS;
    private WindowStatus WS
    {
        set
        {
            if (value != mWS)
            {
                mWS = value;
                windowRect = new Rect(0, 0, Screen.width, Screen.height);
            }
        }
        get
        {
            return mWS;
        }
    }
    /// <summary>
    /// 筛选解析字符
    /// </summary>
    private int mAnalyzeInt = -1;
    private int AnalyzeInt
    {
        set
        {
            if (value != mAnalyzeInt)
            {
                mAnalyzeInt = value;
                tempList = AnalyzeUI();
            }
        }
        get
        {
            return mAnalyzeInt;
        }
    }
    private string[] analyzeStr;
    /// <summary>
    /// 筛选类型字符
    /// </summary>
    private int mTypeInt = -1;
    private int TypeInt
    {
        set
        {
            if (value != mTypeInt)
            {
                mTypeInt = value;
                tempList = AnalyzeUI();
            }
        }
        get
        {
            return mTypeInt;
        }
    }
    private string[] typeStr;
    /// <summary>
    /// 字符串解析内容 时时判断
    /// </summary>
    private string mInputStr = "";
    private string inputStr
    {
        set
        {
            if (value != mInputStr)
            {
                mInputStr = value;
                tempList = AnalyzeUI();
            }
        }
        get
        {
            return mInputStr;
        }
    }
    /// <summary>
    /// 解析后list
    /// </summary>
    private ArrayList tempList;
    /// <summary>
    /// 当前点击info的ID
    /// </summary>
    private int CurrentID = -1;
    /// <summary>
    /// 时时读取屏幕尺寸
    /// </summary>
    private Vector2 mScreenSize;
    private Vector2 ScreenSize
    {
        set
        {
            if (value != mScreenSize)
            {
                mScreenSize = value;
                windowRect = new Rect(0, 0, Screen.width, Screen.height);
            }
        }
        get
        {
            return mScreenSize;
        }
    }
    /// <summary>
    ///是否显示GUI tooltip
    /// </summary>
    private bool isShowGUITooltip;
    /// <summary>
    /// 是否显示fps
    /// </summary>
    private bool isShowFPS;
    /// <summary>
    /// 滑动区域vec2
    /// </summary>
    private Vector2 scrollPosition;
    /// <summary>
    /// 开启平台按键
    /// </summary>
    private KeyCode ShowKey = KeyCode.H;
    /// <summary>
    /// 字体类型
    /// </summary>
    private GUIStyle gs;
    private readonly int toolbarMinHeight = 15;

    //显示吃事件背板
    private static GameObject showLogInstance = null;


    //初始化变量
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LogInfoList = new List<LogInfo>();
        isShow = false;
        windowRect = new Rect(0, 0, Screen.width, Screen.height);
        BGNum = 3f;
        WS = WindowStatus.Logo;
        AnalyzeInt = 0;
        TypeInt = 0;

        analyzeStr = LogInfo.EnumToString(typeof(LogInfo.AnalyzeType));

        typeStr = LogInfo.EnumToString(typeof(LogInfo.Type));

        inputStr = "";
        tempList = new ArrayList();
        CurrentID = -1;
        ScreenSize = new Vector2(Screen.width, Screen.height);
        scrollPosition = Vector2.zero;
        isShowGUITooltip = false;
        isShowFPS = true;

        gs = new GUIStyle();
        gs.alignment = TextAnchor.MiddleCenter;
        gs.wordWrap = true;
        gs.normal.textColor = Color.white;
        CheckFontSize();
    }

    //调用初始化方法
    void Start()
    {
        //Debug.Log("89");
        //Debug.LogWarning("98");
        //Debug.LogError("998");
        //Debug.Log(LogInfo.AnalyzeType.UI.ToString() + "89");
        //Debug.LogWarning(LogInfo.AnalyzeType.UI.ToString() + "98");
        //Debug.LogError(LogInfo.AnalyzeType.UI.ToString() + "998");
        //Debug.Log(LogInfo.AnalyzeType.Eff.ToString() + "897");
        //Debug.LogWarning(LogInfo.AnalyzeType.Eff.ToString() + "968");
        //Debug.LogError(LogInfo.AnalyzeType.Eff.ToString() + "9958");
        //Debug.Log(LogInfo.AnalyzeType.Mod.ToString() + "8194w3t7");
        //Debug.LogWarning(LogInfo.AnalyzeType.Mod.ToString() + "936wfr8");
        //Debug.LogError(LogInfo.AnalyzeType.Mod.ToString() + "9952zSWf28");
        //Debug.Log(LogInfo.AnalyzeType.Anim.ToString() + "8194wgras3t7");
        //Debug.LogWarning(LogInfo.AnalyzeType.Anim.ToString() + "936gae4rgwfr8");
        //Debug.LogError(LogInfo.AnalyzeType.Anim.ToString() + "9952zSa4t62wWf28");
        tempList = AnalyzeUI();
    }

    //在windows编辑器下
    void Update()
    {
        if (isShowFPS)
        {
            FPSLogic();
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(ShowKey))
            {
                Close();
            }
        }
    }

    //绘制所有UI
    void OnGUI()
    {
        ScreenSize = new Vector2(Screen.width, Screen.height);

        if (isShow)
        {
            DrawBG();
            DrawContent();
        }

        if (GUI.Button(new Rect(100, 20, 100, 60), "打开日志"))
        {
            Show();
        }
    }

    #region UI
    //画背景
    private void DrawBG()
    {
        for (int i = 0; i < (int)BGNum; i++)
        {
            GUI.Box(windowRect, "");
        }
    }

    //画背景
    private void DrawBG(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            GUI.Box(windowRect, "");
        }
    }

    //画主要内容 一个window
    private void DrawContent()
    {
        windowRect = GUILayout.Window(0, windowRect, DoWindow, new GUIContent("", ""), "");
    }

    //window 函数
    private void DoWindow(int _id)
    {
        switch (WS)
        {
            case WindowStatus.Logo:
                Logo();
                break;
            case WindowStatus.Main:
                MainUI();
                break;
            case WindowStatus.Info:
                Info();
                break;
            case WindowStatus.GM:
                GMTools();
                break;
            default:
                //Debug.LogError("platform tool is error!");
                break;
        }
    }

    //绘制logo状态
    private void Logo()
    {
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Box(WS.ToString(), GUILayout.MinHeight(Screen.height / 10));
        if (isShowFPS)
            GUILayout.Box("FPS:" + fps.ToString("f1"), GUILayout.MinHeight(Screen.height / 10));
        //绘制拖动条
        //BGNum = GUILayout.HorizontalSlider(BGNum, 0f, BGMAXNUM, GUILayout.MinHeight(windowRect.width / 10));
        //绘制开关 是否开启GUI tooltip
        //isShowGUITooltip = GUILayout.Toggle(isShowGUITooltip, "是否显示tips[GUI Tooltip]");
        //isShowFPS = GUILayout.Toggle(isShowFPS, "是否显示FPS");
        GUILayout.FlexibleSpace();
        //进入下一个窗口状态
        if (GUILayout.Button("- Bug收集界面 -", GUILayout.MinHeight(Screen.height / 4)))
        {
            WS = WindowStatus.Main;
        }
        if (GUILayout.Button("- 打开GM工具 -", GUILayout.MinHeight(Screen.height / 4)))
        {
            WS = WindowStatus.GM;
        }
        GUILayout.FlexibleSpace();
        //关闭窗口
        if (GUILayout.Button("- 关闭界面 -", GUILayout.MinHeight(Screen.height / 4)))
        {
            Close();
        }
        GUILayout.EndVertical();
    }

    //绘制主要UI
    private void MainUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Box(WS.ToString());
        //解析工具条
        AnalyzeInt = GUILayout.Toolbar(AnalyzeInt, analyzeStr, GUILayout.MinHeight(Screen.height / toolbarMinHeight));
        //类型工具条
        TypeInt = GUILayout.Toolbar(TypeInt, typeStr, GUILayout.MinHeight(Screen.height / toolbarMinHeight));
        GUILayout.BeginHorizontal();
        //输入框
        inputStr = GUILayout.TextField(inputStr, GUILayout.MinHeight(Screen.height / toolbarMinHeight));
        //清空输入框
        if (GUILayout.Button("X", GUILayout.MinHeight(Screen.height / toolbarMinHeight), GUILayout.MaxWidth(Screen.height / toolbarMinHeight)))
        {
            inputStr = "";
        }
        GUILayout.EndHorizontal();
        //log列队
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, "");
        for (int i = 0; i < tempList.Count; i++)
        {
            int index = int.Parse(tempList[i].ToString());
            if (LogInfoList[index].BtnContent())
            {
                WS = WindowStatus.Info;
                CurrentID = index;
                ////Debug.Log(LogInfoList[index].LogType);
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUI.contentColor = Color.white;
        //返回上一个窗口状态
        if (GUILayout.Button("B", GUILayout.MinWidth(windowRect.width / 8), GUILayout.MinHeight(windowRect.height - 10)))
        {
            WS = WindowStatus.Logo;
            CurrentID = -1;
        }
        GUILayout.EndHorizontal();
        //GUI tooltip 显示
        if (isShowGUITooltip)
        {
            //DrawBG(6);
            GUI.Box(windowRect, GUI.tooltip, gs);
        }
    }

    //绘制log详细信息
    private void Info()
    {
        if (CurrentID != -1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            //绘制标题
            GUILayout.Box(WS.ToString());
            //绘制详细信息
            LogInfoList[CurrentID].DrawAll();
            GUILayout.EndVertical();
            GUI.contentColor = Color.white;
            //绘制返回按钮
            if (GUILayout.Button("B", GUILayout.MinWidth(windowRect.width / 8), GUILayout.MinHeight(windowRect.height - 10)))
            {
                WS = WindowStatus.Main;
                CurrentID = -1;
            }
            GUILayout.EndHorizontal();
        }
    }

    //绘制gm工具
    private void GMTools()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Box("功能未完成");
        GUILayout.EndVertical();
        //绘制返回按钮
        if (GUILayout.Button("B", GUILayout.MinWidth(windowRect.width / 8), GUILayout.MinHeight(windowRect.height - 10)))
        {
            WS = WindowStatus.Logo;
        }
        GUILayout.EndHorizontal();
    }

    //窗口关闭
    private void Close()
    {
        Show();
        //以下是初始化一系列参数
        WS = WindowStatus.Logo;
        AnalyzeInt = 0;
        TypeInt = 0;
        CurrentID = -1;

    }

    //解析and筛选
    private ArrayList AnalyzeUI()
    {
        ArrayList list = new ArrayList();

        for (int i = 0; i < LogInfoList.Count; i++)
        {
            string content = LogInfoList[i].Content.Trim();
            content = content.ToLower();
            string input = inputStr.Trim();
            input = input.ToLower();

            if (isInput(content, input))
            {
                //如果当前的解析状态==所有
                if ((LogInfo.AnalyzeType)AnalyzeInt == LogInfo.AnalyzeType.All)
                {
                    isType(list, i);
                }
                //如果此对象的解析状态== 当前
                else if (LogInfoList[i].Analyze == (LogInfo.AnalyzeType)AnalyzeInt)
                {
                    isType(list, i);
                }
            }
        }

        return list;
    }
    //筛选logtype
    private void isType(ArrayList _list, int _index)
    {
        //如果当前的类型状态==所有
        if ((LogInfo.Type)TypeInt == LogInfo.Type.All)
        {
            _list.Add(_index);
        }
        //如果此对象的类型状态== 当前
        else if (LogInfoList[_index].LogType == (LogInfo.Type)TypeInt)
        {
            _list.Add(_index);
        }
    }
    //筛选输入字符串
    private bool isInput(string _content, string _input)
    {
        //内容包含 or 输入为空
        if (_content.Contains(_input) || string.IsNullOrEmpty(_input))
        {
            return true;
        }
        return false;
    }
    #endregion

    //字体大小
    private void CheckFontSize()
    {
        float size = Screen.height / 960f * 22f;
        this.gs.fontSize = (int)size;
        ////Debug.Log("UI -- " + size);
    }

    #region list
    //加入list方法 做过筛选 主要是针对logtype和解析类型
    private void ListAddLogInfo(string _logStr, string _stackTrace, LogType _type)
    {
        string content = _logStr.Trim();
        LogInfo.AnalyzeType analyze = LogInfo.AnalyzeType.Other;
        LogInfo.Type type;
        //设置全为other 如果可以解析就改变
        for (int i = 1; i < analyzeStr.Length; i++)
        {
            string s = analyzeStr[i].Trim();
            if (content.StartsWith(s))
            {
                analyze = (LogInfo.AnalyzeType)i;
                break;
            }
        }
        //筛选等意义类型状态
        switch (_type)
        {
            case LogType.Log:
                type = LogInfo.Type.Log;
                break;
            case LogType.Warning:
                type = LogInfo.Type.Warning;
                break;
            case LogType.Error:
                type = LogInfo.Type.Error;
                break;
            default:
                type = LogInfo.Type.Error;
                break;
        }
        //筛选已有log
        foreach (LogInfo li in LogInfoList)
        {
            if (_logStr.Equals(li.Content)
                && type == li.LogType)
            {
                return;
            }
        }
        //将合格对象加入list
        if (isAdd())
            LogInfoList.Add(new LogInfo(LogInfoList.Count, _logStr, _stackTrace, type, analyze));
    }
    //是否添加
    private bool isAdd()
    {
        return true;
    }
    //清空list
    private void ClearList()
    {
        if (LogInfoList!=null)
        {
        LogInfoList.Clear();
        }
        
    }
    #endregion

    #region callback
    //脚本开启时监听callback 并回调给自己的函数
    void OnEnable()
    {
      //  Application.logMessageReceived+=HandleLog;
        Application.RegisterLogCallback(HandleLog);
    }
    //脚本关闭时 关闭监听
    void OnDisable()
    {
//        Application.logMessageReceived-=HandleLog;
//        Application.RegisterLogCallback(null);
        ClearList();
    }
    //回调方法
    void HandleLog(string _logStr, string _stackTrace, LogType _type)
    {
        ListAddLogInfo(_logStr, _stackTrace, _type);
        tempList = AnalyzeUI();

        
    }
    #endregion

    #region fps
    private float updateInterval = 1f;
    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft = 0f;
    private static float fps = 0f;

    private void FPSLogic()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
    #endregion

    void OnDestroy()
    {
        ClearList();
    }

    //公共方法
    public static bool Show()
    {
        isShow = !isShow;
        return isShow;
    }
}
