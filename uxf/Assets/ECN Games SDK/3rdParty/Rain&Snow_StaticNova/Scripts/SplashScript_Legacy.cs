using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game;

namespace Rain_Snow_StaticNova.SplashScript_Legacy
{

    public class SplashScript_Legacy : MonoBehaviour
    {
        //雨滴飞溅效果prefab
        public GameObject splash;
        //玩家位置
        private Vector3 heroPosition;
        //地表layer
        private int layerMas = 1 << 8;
        //偏移距离
        private float randX;
        private float randZ;
        private float y = 10;
        //雨滴出现间隔
        private float showTime = 0.1f;
        //射线碰撞点
        private RaycastHit hit;
        //射线发射点
        private Vector3 targetPosition;

        void OnEnable()
        {
            InvokeRepeating("Show", 0, showTime);
        }

        void OnDisable()
        {
            CancelInvoke();
        }

        /// <summary>
        /// 显示雨滴飞溅
        /// </summary>
        void Show()
        {

            //heroPosition = GameApp.sceneController.hero.transform.position;

            //System.Random ran = new System.Random();
            //randX = (float)((decimal)(ran.Next(-50, 50)) / 10);
            //randZ = (float)((decimal)(ran.Next(-50, 50)) / 10);
         
            //targetPosition = heroPosition + new Vector3(randX, y, randZ);

            //if (Physics.Raycast(targetPosition, new Vector3(0, -1, 0), out hit, 100, layerMas))
            //{
            //    GameObject splashObj = GameObject.Instantiate(splash, hit.point, Quaternion.identity) as GameObject;
            //    splashObj.transform.SetParent(GameApp.weatherController.weather);
            //    Destroy(splashObj, 0.5f);
            //}
        }
    }
}
