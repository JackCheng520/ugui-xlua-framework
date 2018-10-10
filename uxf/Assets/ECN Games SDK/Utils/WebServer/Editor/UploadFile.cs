using System.Collections;
using System.Net;
using System.Collections.Specialized;
using System;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using ICSharpCode;

namespace com.mango.framework.Utils.WebServer
{
    /// <summary>
    /// 上传文件到Web服务器.
    /// 
    /// 流程:
    /// 1、压缩本地需要上传的文件.
    /// 2、上传压缩文件到web服务器.
    /// 3、web服务器解压缩文件，并删除unity生成的.meta文件.
    /// </summary>
    public class UploadFile
    {
        [MenuItem("ECN Games SDK/Web Server/Upload Files To Web Server")]
        [MenuItem("Assets/ECN Games SDK/Web Server/Upload Files To Web Server")]
        //开始上传文件.
        static void StartUploadFiles()
        {
            UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("Warning...", "Please Select Upload Files!", "OK");
                return;
            }

            //删除目录.
            HttpFormatDir("http://localhost/PHP/UploadFile/rm_dir.php", PlayerSettings.productName);
            //return;
            //上传文件.
            foreach (UnityEngine.Object obj in selection)
            {
                if (File.Exists(AssetDatabase.GetAssetPath(obj)))
                {
                    NameValueCollection nvc = new NameValueCollection();
                    string savePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj)).Replace("Assets/", "");
                    nvc.Add("rootdirname", PlayerSettings.productName);
                    nvc.Add("savepath", PlayerSettings.productName + "/" + savePath);
                    HttpUploadFile("http://localhost/PHP/UploadFile/upload_file.php",
                         Application.dataPath + "/../" + AssetDatabase.GetAssetPath(obj), "file", "application/octet-stream", nvc);
                }
            }
        }

        /// <summary>
        /// 格式化web服务器对应的目录.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dirDetail">目录详细信息，每个目录已','分割</param>
        public static void HttpFormatDir(string url, string dirDetail)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "rmdir=" + dirDetail;

            byte[] data = encoding.GetBytes(postData);

            // Prepare web request...
            HttpWebRequest myRequest = WebRequest.Create(url) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            // Get response
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
            Debug.Log(reader.ReadToEnd());
        }

        //发送HTTP请求表单.
        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
                
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                Debug.Log(reader2.ReadToEnd());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }
    }
}
