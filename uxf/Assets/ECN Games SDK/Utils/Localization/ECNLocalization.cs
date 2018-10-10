namespace ECN
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using LitJson;

    public enum LANGUAGE
    {
        CN,
        EN,
        VIE,
        TW,
        KO
    }

    /// <summary>
    /// Firstly,load localization key-value from shared assets by Resources.Load.
    /// Secondly,load localization key-value by send request.
    /// </summary>
    public class ECNLocalization
    {
        private static Dictionary<string, string> localizationDic = null;
        private static LANGUAGE language;

        static public LANGUAGE Language
        {
            get 
            {
                LANGUAGE ret = LANGUAGE.CN;
                string curLanguage = PlayerPrefs.GetString("Localization-Language", "CN");
                if (curLanguage == LANGUAGE.CN.ToString()) ret = LANGUAGE.CN;
                else if (curLanguage == LANGUAGE.EN.ToString()) ret = LANGUAGE.EN;
                else if (curLanguage == LANGUAGE.VIE.ToString()) ret = LANGUAGE.VIE;
                else if (curLanguage == LANGUAGE.TW.ToString()) ret = LANGUAGE.TW;
                else if (curLanguage == LANGUAGE.KO.ToString()) ret = LANGUAGE.KO;
                return ret;
            }
            set 
            {
                language = value;
                PlayerPrefs.SetString("Localization-Language", language.ToString());
                PlayerPrefs.Save();
            }
        }

        public static string Get(string key)
        {
            LoadLocal();

            string ret = string.Empty;
            if (localizationDic != null)
            {
                foreach (KeyValuePair<string,string> kp in localizationDic)
                {
                    string temp1 = kp.Key.Trim();
                    temp1 = temp1.Replace("\"", "");

                    string temp2 = key.Trim();
                    if (temp1.Equals(temp2))
                    {
                        ret = kp.Value;
                        break;
                    }
                }
                if (ret.Length == 0) ret = key;
            } 
            ret = ret.Replace(@"\n", "\n");
            ret = ret.Replace("\"","");
            return ret;
        }

        static void LoadLocal()
        {
            if (localizationDic == null)
            {
                localizationDic = new Dictionary<string, string>();
                //加载本地化key文本.
                TextAsset localizationKey = Resources.Load<TextAsset>("Localization_KEY");

                StringReader reader1 = new StringReader(localizationKey.text);
                //加载本地化value文本.
                TextAsset localizationValue = Resources.Load<TextAsset>(string.Format("Localization_{0}", ECNLocalization.Language.ToString()));
                StringReader reader2 = new StringReader(localizationValue.text);

                string localKey = string.Empty;
                string value = string.Empty;
                //跳过第一行.
                reader1.ReadLine();
                reader2.ReadLine();
                while (((localKey = reader1.ReadLine()) != null && localKey.Length > 0) && ((value = reader2.ReadLine()) != null && value.Length > 0))
                {
                    localKey = localKey.Replace("\"", "");
                    if (localizationDic.ContainsKey(localKey)) continue;
                    localizationDic.Add(localKey, value);
                }
            }
        }

        /// <summary>
        /// Load localization key-value from server.
        /// </summary>
        public static void LoadFromNetwork(JsonData objs)
        {
            LoadLocal();

            //Store localization's md5.
            PlayerPrefs.SetString("MD5_LOCALIZATION", objs[1].ToString());
            //
            int count = objs[0].Count;
            for (int i = 0; i < count; i++)
            {
                string key = objs[0][i][0].ToString();
                string value = objs[0][i][1].ToString();
                if (localizationDic.ContainsKey(key))
                {
                    //Update localization.
                    localizationDic.Remove(key);
                    localizationDic.Add(key, value);
                }
                else
                {
                    localizationDic.Add(key, value);
                }
            }
        }
    }
}
