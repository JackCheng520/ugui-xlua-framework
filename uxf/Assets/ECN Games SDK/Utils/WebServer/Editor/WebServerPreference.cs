using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.mango.framework.Utils.WebServer
{
    public class WebServerPreference : EditorWindow
    {
        //修改Web服务器地址.
        private bool canEditWebURL = false;
        //修改项目资源路径.
        private bool canEditProjectAssetsURL = false;

        [MenuItem("ECN Games SDK/Web Server/Web Server Preference")]
        [MenuItem("Assets/ECN Games SDK/Web Server/Web Server Preference")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            WebServerPreference window = (WebServerPreference)EditorWindow.GetWindow(typeof(WebServerPreference));
            window.title = "WebServerPreference";
            window.Show();
            window.maxSize = new Vector2(700, 400);
            window.minSize = new Vector2(700, 400);
        }

        String ConfigPath()
        {
            if (!Directory.Exists(Application.dataPath + "/" + "ECN Games SDK/Utils/WebServer/Resources"))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + "ECN Games SDK/Utils/WebServer/Resources");
            }
            if (!Directory.Exists(Application.dataPath + "/" + "ECN Games SDK/Utils/WebServer/Resources/Config"))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + "ECN Games SDK/Utils/WebServer/Resources/Config");
            }
            return "Assets/ECN Games SDK/Utils/WebServer/Resources/Config/WebServerPreference.asset";
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (canEditWebURL)
            {
                EditorGUILayout.PrefixLabel("Web Server IP:");
                WebServerPreConfig preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                if (preConfig == null)
                {
                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WebServerPreConfig>(), ConfigPath());
                }
                preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                preConfig.webServerIP = EditorGUILayout.TextField("", preConfig.webServerIP);
                if (GUILayout.Button("ok"))
                {
                    canEditWebURL = false;
                    if (preConfig) EditorUtility.SetDirty(preConfig);
                }
            }
            else
            {
                EditorGUILayout.PrefixLabel("Web Server IP:");
                WebServerPreConfig preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                if (preConfig == null)
                {
                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WebServerPreConfig>(), ConfigPath());
                }
                preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                EditorGUILayout.LabelField("", preConfig.webServerIP);
                if (GUILayout.Button("Edit"))
                {
                    canEditWebURL = true;
                }
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (canEditProjectAssetsURL)
            {
                EditorGUILayout.PrefixLabel("Project Assets URL:");
                WebServerPreConfig preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                if (preConfig == null)
                {
                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WebServerPreConfig>(), ConfigPath());
                }
                preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                preConfig.projectAssetsURL = EditorGUILayout.TextField("", preConfig.projectAssetsURL);
                if (GUILayout.Button("ok"))
                {
                    canEditProjectAssetsURL = false;
                    if (preConfig) EditorUtility.SetDirty(preConfig);
                }
            }
            else
            {
                EditorGUILayout.PrefixLabel("Project Assets URL:");
                WebServerPreConfig preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                if (preConfig == null)
                {
                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WebServerPreConfig>(), ConfigPath());
                }
                preConfig = AssetDatabase.LoadAssetAtPath(ConfigPath(), typeof(WebServerPreConfig)) as WebServerPreConfig;
                EditorGUILayout.LabelField("", preConfig.projectAssetsURL);
                if (GUILayout.Button("Edit"))
                {
                    canEditProjectAssetsURL = true;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        void OnInspectorUpdate()
        {
            // Call Repaint on OnInspectorUpdate as it repaints the windows
            // less times as if it was OnGUI/Update
            Repaint();
        }
    }
}
