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

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ����������"), Chinese("����������")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        //private string dbName;

        //[CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ�����"), Chinese("���ݿ���")]
        //public string DBName
        //{
        //    get { return dbName; }
        //    set { dbName = value; }
        //}

        private string dbUser;

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ������û���"), Chinese("�û���")]
        public string DBUser
        {
            get { return dbUser; }
            set { dbUser = value; }
        }
        private string password;

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ���������"), Chinese("����")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //private string ip;

        //[CategoryAttribute("�Ѷ��ͨ�Ų���"), DescriptionAttribute("��ַIP"), Chinese("IP")]
        //public string IP
        //{
        //    get { return ip; }
        //    set { ip = value; }
        //}

        //private int port;

        //[CategoryAttribute("�Ѷ��ͨ�Ų���"), DescriptionAttribute("ͨ�Ŷ˿�"), Chinese("�˿�")]
        //public int Port
        //{
        //    get { return port; }
        //    set { port = value; }
        //}


        private string plc1ServerName;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC1ServerName
        {
            get { return plc1ServerName; }
            set { plc1ServerName = value; }
        }

        private string plc1ServerIp;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC1ServerIP
        {
            get { return plc1ServerIp; }
            set { plc1ServerIp = value; }
        }


        private string plc1GroupString;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("1�ŶѶ��PLCͨѶ��������"), Chinese("��������")]
        public string PLC1GroupString
        {
            get { return plc1GroupString; }
            set { plc1GroupString = value; }
        }

        private int plc1UpdateRate;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("1�ŶѶ��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC1UpdateRate
        {
            get { return plc1UpdateRate; }
            set { plc1UpdateRate = value; }
        }


        private string plc2ServerName;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC2ServerName
        {
            get { return plc2ServerName; }
            set { plc2ServerName = value; }
        }

        private string plc2ServerIp;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC2ServerIP
        {
            get { return plc2ServerIp; }
            set { plc2ServerIp = value; }
        }


        private string plc2GroupString;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("2�ŶѶ��ͨѶ��������"), Chinese("��������")]
        public string PLC2GroupString
        {
            get { return plc2GroupString; }
            set { plc2GroupString = value; }
        }

        private int plc2UpdateRate;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("2�ŶѶ��ˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC2UpdateRate
        {
            get { return plc2UpdateRate; }
            set { plc2UpdateRate = value; }
        }


        private string plc3ServerName;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC3ServerName
        {
            get { return plc3ServerName; }
            set { plc3ServerName = value; }
        }

        private string plc3ServerIp;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC3ServerIP
        {
            get { return plc3ServerIp; }
            set { plc3ServerIp = value; }
        }


        private string plc3GroupString;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("3�ŶѶ��ͨѶ��������"), Chinese("��������")]
        public string PLC3GroupString
        {
            get { return plc3GroupString; }
            set { plc3GroupString = value; }
        }

        private int plc3UpdateRate;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("3�ŶѶ��ˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC3UpdateRate
        {
            get { return plc3UpdateRate; }
            set { plc3UpdateRate = value; }
        }


        private string plc4ServerName;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC4ServerName
        {
            get { return plc4ServerName; }
            set { plc4ServerName = value; }
        }

        private string plc4ServerIp;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC4ServerIP
        {
            get { return plc4ServerIp; }
            set { plc4ServerIp = value; }
        }


        private string plc4GroupString;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("4�ŶѶ��ͨѶ��������"), Chinese("��������")]
        public string PLC4GroupString
        {
            get { return plc4GroupString; }
            set { plc4GroupString = value; }
        }

        private int plc4UpdateRate;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("4�ŶѶ��ˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC4UpdateRate
        {
            get { return plc4UpdateRate; }
            set { plc4UpdateRate = value; }
        }


        private string plc5ServerName;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC5ServerName
        {
            get { return plc5ServerName; }
            set { plc5ServerName = value; }
        }

        private string plc5ServerIp;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC5ServerIP
        {
            get { return plc5ServerIp; }
            set { plc5ServerIp = value; }
        }


        private string plc5GroupString;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("5�ŶѶ��ͨѶ��������"), Chinese("��������")]
        public string PLC5GroupString
        {
            get { return plc5GroupString; }
            set { plc5GroupString = value; }
        }

        private int plc5UpdateRate;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("5�ŶѶ��ˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC5UpdateRate
        {
            get { return plc5UpdateRate; }
            set { plc5UpdateRate = value; }
        }



















        private string plc6ServerName;
        [CategoryAttribute("������PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC6ServerName
        {
            get { return plc6ServerName; }
            set { plc6ServerName = value; }
        }

        private string plc6ServerIp;
        [CategoryAttribute("������PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC6ServerIP
        {
            get { return plc6ServerIp; }
            set { plc6ServerIp = value; }
        }


        private string plc6GroupString;
        [CategoryAttribute("������PLCͨ�Ų���"), DescriptionAttribute("������PLCͨѶ��������"), Chinese("��������")]
        public string PLC6GroupString
        {
            get { return plc6GroupString; }
            set { plc6GroupString = value; }
        }

        private int plc6UpdateRate;
        [CategoryAttribute("������PLCͨ�Ų���"), DescriptionAttribute("������PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC6UpdateRate
        {
            get { return plc6UpdateRate; }
            set { plc6UpdateRate = value; }
        }
    }
}
