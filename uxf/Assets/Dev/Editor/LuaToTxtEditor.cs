using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 将lua文件转换成txt文件
/// </summary>
public class LuaToTxtEditor : Editor
{
    /// <summary>
    /// 游戏内存放的lua文件夹
    /// </summary>
    private static string luaFilePath = Application.dataPath + "/Dev/LuaFiles/";
    /// <summary>
    /// 游戏内存放的lua转成的txt文件夹
    /// </summary>
    private static string txtFilePath = Application.dataPath + "/Dev/Resources/TxtFiles/";


    [MenuItem("Tool/LuaToTxt(lua文件转txt文件)")]
    static void ChangeLuaFileToTxt()
    {
        //Debug.Log("luaFilePath:" + luaFilePath);
        //Debug.Log("txtFilePath:" + txtFilePath);

        if (!Directory.Exists(luaFilePath))
        {
            Directory.CreateDirectory(luaFilePath);
        }
        if (!Directory.Exists(txtFilePath))
        {
            Directory.CreateDirectory(txtFilePath);
        }

        string[] txtFiles = Directory.GetFiles(txtFilePath, "*.*", SearchOption.AllDirectories);
        if (txtFiles != null)
        {
            for (int i = 0; i < txtFiles.Length; i++)
            {
                if (File.Exists(txtFiles[i]))
                    File.Delete(txtFiles[i]);
            }
        }

        string[] luaFiles = Directory.GetFiles(luaFilePath, "*.lua", SearchOption.AllDirectories);
        if (luaFiles == null || luaFiles.Length == 0)
        {
            EditorUtility.DisplayDialog("提示", luaFilePath + "没有找到lua文件", "确定");
            return;
        }
        for (int i = 0; i < luaFiles.Length; i++)
        {
            FileInfo fi = new FileInfo(luaFiles[i]);
            fi.CopyTo(txtFilePath + fi.Name + ".txt");
        }
        //刷新
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("提示", "lua转txt文件成功!", "确定");
    }
}
