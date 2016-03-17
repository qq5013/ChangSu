using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace App
{
    public class Crane
    {
        public int CraneNo { get; set; }
        public int Column { get; set; }
    }

    public delegate void CraneEventHandler(CraneEventArgs args);
    public class CraneEventArgs
    {
        private Crane _crane;
        public Crane crane
        {
            get
            {
                return _crane;
            }
        }
        public CraneEventArgs(Crane crane)
        {
            this._crane = crane;
        }
    }
    public class Cranes
    {
        public static event CraneEventHandler OnCrane = null;

        public static void CraneInfo(Crane crane)
        {
            if (OnCrane != null)
            {
                OnCrane(new CraneEventArgs(crane));
            }
        }
    }

    public class Car
    {
        public int CarNo { get; set; }
        public string TaskNo { get; set; }
        public string PalletCode { get; set; }
        public int ErrCode { get; set; }
    }

    public delegate void CarEventHandler(CarEventArgs args);
    public class CarEventArgs
    {
        private Car _car;
        public Car car
        {
            get
            {
                return _car;
            }
        }
        public CarEventArgs(Car car)
        {
            this._car = car;
        }
    }
    public class Cars
    {
        public static event CarEventHandler OnCar = null;

        public static void CarInfo(Car car)
        {
            if (OnCar != null)
            {
                OnCar(new CarEventArgs(car));
            }
        }
    }


    public class PLC
    {
        public string TextName { get; set; }
        public string Text { get; set; }
        public string ShowPic { get; set; }
        public string PicName { get; set; }
        public string IsErr { get; set; }
        public string ErrCode { get; set; }
        public string TextErrName { get; set; }
        public string ID { get; set; }
        public string TaskType { get; set; }
        public string TextTypeName { get; set; }
    }

    public delegate void PLCEventHandler(PLCEventArgs args);
    public class PLCEventArgs
    {
        private PLC _plc;
        public PLC plc
        {
            get
            {
                return _plc;
            }
        }
        public PLCEventArgs(PLC plc)
        {
            this._plc = plc;
        }
    }
    public class PLCS
    {
        public static event PLCEventHandler OnPLC = null;

        public static void PLCInfo(PLC plc)
        {
            if (OnPLC != null)
            {
                OnPLC(new PLCEventArgs(plc));
            }
        }
    }
}
