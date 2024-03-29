using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Util;

namespace App.View.Param
{
    public class Parameter: BaseObject
    {
        private string serverName;

        [CategoryAttribute("服务器数据库连接参数"), DescriptionAttribute("数据库服务器名称"), Chinese("服务器名称")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        //private string dbName;

        //[CategoryAttribute("服务器数据库连接参数"), DescriptionAttribute("数据库名称"), Chinese("数据库名")]
        //public string DBName
        //{
        //    get { return dbName; }
        //    set { dbName = value; }
        //}

        private string dbUser;

        [CategoryAttribute("服务器数据库连接参数"), DescriptionAttribute("数据库连接用户名"), Chinese("用户名")]
        public string DBUser
        {
            get { return dbUser; }
            set { dbUser = value; }
        }
        private string password;

        [CategoryAttribute("服务器数据库连接参数"), DescriptionAttribute("数据库连接密码"), Chinese("密码")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //private string ip;

        //[CategoryAttribute("堆垛机通信参数"), DescriptionAttribute("地址IP"), Chinese("IP")]
        //public string IP
        //{
        //    get { return ip; }
        //    set { ip = value; }
        //}

        //private int port;

        //[CategoryAttribute("堆垛机通信参数"), DescriptionAttribute("通信端口"), Chinese("端口")]
        //public int Port
        //{
        //    get { return port; }
        //    set { port = value; }
        //}


        private string plc1ServerName;
        [CategoryAttribute("1号堆垛机PLC通信参数"), DescriptionAttribute("服务名称"), Chinese("服务名称")]
        public string PLC1ServerName
        {
            get { return plc1ServerName; }
            set { plc1ServerName = value; }
        }

        private string plc1ServerIp;
        [CategoryAttribute("1号堆垛机PLC通信参数"), DescriptionAttribute("服务地址IP"), Chinese("服务IP")]
        public string PLC1ServerIP
        {
            get { return plc1ServerIp; }
            set { plc1ServerIp = value; }
        }


        private string plc1GroupString;
        [CategoryAttribute("1号堆垛机PLC通信参数"), DescriptionAttribute("1号堆垛机PLC通讯连接名称"), Chinese("连接名称")]
        public string PLC1GroupString
        {
            get { return plc1GroupString; }
            set { plc1GroupString = value; }
        }

        private int plc1UpdateRate;
        [CategoryAttribute("1号堆垛机PLC通信参数"), DescriptionAttribute("1号堆垛机PLC刷新频率"), Chinese("刷新频率")]
        public int PLC1UpdateRate
        {
            get { return plc1UpdateRate; }
            set { plc1UpdateRate = value; }
        }


        private string plc2ServerName;
        [CategoryAttribute("2号堆垛机PLC通信参数"), DescriptionAttribute("服务名称"), Chinese("服务名称")]
        public string PLC2ServerName
        {
            get { return plc2ServerName; }
            set { plc2ServerName = value; }
        }

        private string plc2ServerIp;
        [CategoryAttribute("2号堆垛机PLC通信参数"), DescriptionAttribute("服务地址IP"), Chinese("服务IP")]
        public string PLC2ServerIP
        {
            get { return plc2ServerIp; }
            set { plc2ServerIp = value; }
        }


        private string plc2GroupString;
        [CategoryAttribute("2号堆垛机PLC通信参数"), DescriptionAttribute("2号堆垛机通讯连接名称"), Chinese("连接名称")]
        public string PLC2GroupString
        {
            get { return plc2GroupString; }
            set { plc2GroupString = value; }
        }

        private int plc2UpdateRate;
        [CategoryAttribute("2号堆垛机PLC通信参数"), DescriptionAttribute("2号堆垛机刷新频率"), Chinese("刷新频率")]
        public int PLC2UpdateRate
        {
            get { return plc2UpdateRate; }
            set { plc2UpdateRate = value; }
        }


        private string plc3ServerName;
        [CategoryAttribute("3号堆垛机PLC通信参数"), DescriptionAttribute("服务名称"), Chinese("服务名称")]
        public string PLC3ServerName
        {
            get { return plc3ServerName; }
            set { plc3ServerName = value; }
        }

        private string plc3ServerIp;
        [CategoryAttribute("3号堆垛机PLC通信参数"), DescriptionAttribute("服务地址IP"), Chinese("服务IP")]
        public string PLC3ServerIP
        {
            get { return plc3ServerIp; }
            set { plc3ServerIp = value; }
        }


        private string plc3GroupString;
        [CategoryAttribute("3号堆垛机PLC通信参数"), DescriptionAttribute("3号堆垛机通讯连接名称"), Chinese("连接名称")]
        public string PLC3GroupString
        {
            get { return plc3GroupString; }
            set { plc3GroupString = value; }
        }

        private int plc3UpdateRate;
        [CategoryAttribute("3号堆垛机PLC通信参数"), DescriptionAttribute("3号堆垛机刷新频率"), Chinese("刷新频率")]
        public int PLC3UpdateRate
        {
            get { return plc3UpdateRate; }
            set { plc3UpdateRate = value; }
        }


        private string plc4ServerName;
        [CategoryAttribute("4号堆垛机PLC通信参数"), DescriptionAttribute("服务名称"), Chinese("服务名称")]
        public string PLC4ServerName
        {
            get { return plc4ServerName; }
            set { plc4ServerName = value; }
        }

        private string plc4ServerIp;
        [CategoryAttribute("4号堆垛机PLC通信参数"), DescriptionAttribute("服务地址IP"), Chinese("服务IP")]
        public string PLC4ServerIP
        {
            get { return plc4ServerIp; }
            set { plc4ServerIp = value; }
        }


        private string plc4GroupString;
        [CategoryAttribute("4号堆垛机PLC通信参数"), DescriptionAttribute("4号堆垛机通讯连接名称"), Chinese("连接名称")]
        public string PLC4GroupString
        {
            get { return plc4GroupString; }
            set { plc4GroupString = value; }
        }

        private int plc4UpdateRate;
        [CategoryAttribute("4号堆垛机PLC通信参数"), DescriptionAttribute("4号堆垛机刷新频率"), Chinese("刷新频率")]
        public int PLC4UpdateRate
        {
            get { return plc4UpdateRate; }
            set { plc4UpdateRate = value; }
        }


        private string plc5ServerName;
        [CategoryAttribute("5号堆垛机PLC通信参数"), DescriptionAttribute("服务名称"), Chinese("服务名称")]
        public string PLC5ServerName
        {
            get { return plc5ServerName; }
            set { plc5ServerName = value; }
        }

        private string plc5ServerIp;
        [CategoryAttribute("5号堆垛机PLC通信参数"), DescriptionAttribute("服务地址IP"), Chinese("服务IP")]
        public string PLC5ServerIP
        {
            get { return plc5ServerIp; }
            set { plc5ServerIp = value; }
        }


        private string plc5GroupString;
        [CategoryAttribute("5号堆垛机PLC通信参数"), DescriptionAttribute("5号堆垛机通讯连接名称"), Chinese("连接名称")]
        public string PLC5GroupString
        {
            get { return plc5GroupString; }
            set { plc5GroupString = value; }
        }

        private int plc5UpdateRate;
        [CategoryAttribute("5号堆垛机PLC通信参数"), DescriptionAttribute("5号堆垛机刷新频率"), Chinese("刷新频率")]
        public int PLC5UpdateRate
        {
            get { return plc5UpdateRate; }
            set { plc5UpdateRate = value; }
        }



















        private string plc6ServerName;
        [CategoryAttribute("输送线PLC通信参数"), DescriptionAttribute("服务名称"), Chinese("服务名称")]
        public string PLC6ServerName
        {
            get { return plc6ServerName; }
            set { plc6ServerName = value; }
        }

        private string plc6ServerIp;
        [CategoryAttribute("输送线PLC通信参数"), DescriptionAttribute("服务地址IP"), Chinese("服务IP")]
        public string PLC6ServerIP
        {
            get { return plc6ServerIp; }
            set { plc6ServerIp = value; }
        }


        private string plc6GroupString;
        [CategoryAttribute("输送线PLC通信参数"), DescriptionAttribute("输送线PLC通讯连接名称"), Chinese("连接名称")]
        public string PLC6GroupString
        {
            get { return plc6GroupString; }
            set { plc6GroupString = value; }
        }

        private int plc6UpdateRate;
        [CategoryAttribute("输送线PLC通信参数"), DescriptionAttribute("输送线PLC刷新频率"), Chinese("刷新频率")]
        public int PLC6UpdateRate
        {
            get { return plc6UpdateRate; }
            set { plc6UpdateRate = value; }
        }
    }
}
