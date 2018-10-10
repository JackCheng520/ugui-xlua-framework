/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System;
using System.IO;
using UnityEngine;
using XLua;

[CSharpCallLua]
public class Helloworld : MonoBehaviour
{
    LuaEnv luaenv = new LuaEnv();


    // Use this for initialization
    private void Start()
    {
        //luaenv.DoString(@"require('CarClass')");

        LuaEnv.CustomLoader method = CustomLoaderMethod;

        //添加自定义装载机Loader  
        luaenv.AddLoader(method);
        luaenv.DoString(@" require('HelloWorld')");
        
        Action<string> luaStart = luaenv.Global.GetInPath<Action<string>>("HelloWorld.Start");
        luaStart("Hello World");
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnDestroy()
    {
        luaenv.Dispose();
    }


    private byte[] CustomLoaderMethod(ref string fileName)
    {
        Debug.Log(fileName);
        fileName = "LuaFiles/" + fileName;
        //找到指定文件  
        fileName = Application.dataPath + "/Dev/" + fileName.Replace('.', '/') + ".lua";
        if (File.Exists(fileName))
        {
            return File.ReadAllBytes(fileName);
        }
        else
        {
            return null;
        }
    }
}
