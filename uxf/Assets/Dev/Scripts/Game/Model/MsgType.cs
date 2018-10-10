/*******************************************
*项目名称 ：消息类型
*项目描述 :
*类 名 称 : MsgType
*作    者 : ASUS 
*创建时间 : 2017/5/10 11:32:27
*更新时间 : 2017/5/10 11:32:27
********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class MsgType
    {
        //更新消息
        public const string TYPE_UPDATE_MESSAGE = "UpdateMessage";
        //更新解包
        public const string TYPE_UPDATE_EXTRACT = "UpdateExtract";
        //更新下载
        public const string TYPE_UPDATE_DOWNLOAD = "UpdateDownload";
        //更新进度
        public const string TYPE_UPDATE_PROGRESS = "UpdateProgress";
        

    }
}