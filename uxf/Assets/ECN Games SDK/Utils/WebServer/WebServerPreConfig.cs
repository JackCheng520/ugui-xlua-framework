using UnityEngine;
using System.Collections;

namespace com.mango.framework.Utils.WebServer
{
    /// <summary>
    /// Web Server 偏好设置类.
    /// </summary>
    public class WebServerPreConfig : ScriptableObject
    {
        //web服务器IP地址.
        public string webServerIP = "";
        //项目资源路径.
        public string projectAssetsURL = "";
    }
}
