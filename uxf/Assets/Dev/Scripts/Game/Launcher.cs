using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Launcher : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GameApp.Init();
            //BaseUI.ShowUI<GameUIMgrMainUI>();

            
        }

        // Update is called once per frame
        void Update()
        {
            //刷新lua文件
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Debug.Log("刷新lua文件----------------------------------------");

                Action luaOnReload = GameApp.GlobalTable.GetInPath<Action>("Main.ReLoad");
                luaOnReload();

                GameApp.gameManager.CheckExtractResource();

                //GameApp.DoLoadLuaFiles();

                
            }
        }

        //private void OnGUI()
        //{
        //    if (GUI.Button(new Rect(100, 100, 200, 100), "打开日志"))
        //    {
        //        LogCallback.Show();
        //    }
        //}

    }
}
