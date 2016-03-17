using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MCP;

namespace Dispatching.LED2008
{
    public class LEDUtil
    {
        private static LedCollection leds = new LedCollection();
        public static Dictionary<int, string> isActiveLeds = new Dictionary<int, string>();

        public static void Release()
        {
            //释放LED屏资源
            leds.Release();
            leds = null;
        }

        public static void Initialize( string LedCollection)
        {
            leds.DelAllProgram();
            isActiveLeds.Clear();
            string[] ledConfig = LedCollection.Split(';');
            Microsoft.VisualBasic.Devices.Network network = new Microsoft.VisualBasic.Devices.Network();
            foreach (string led in ledConfig)
            {
                if (network.Ping(led.Split(',')[1]))
                {
                    isActiveLeds.Add(Convert.ToInt32(led.Split(',')[0]), led.Split(',')[1]);
                    leds.Add(Convert.ToInt32(led.Split(',')[0]));
                }
                else
                {
                    Logger.Error(Convert.ToInt32(led.Split(',')[0]) + "号LED屏故障，请检查！IP:[" + led.Split(',')[1] + "]");
                }
            }
        }

        public static void RefreshLED(string ledNo, string strContent)
        {
            int cardNum = Convert.ToInt32(ledNo);

            if (!IsOnLineLed(cardNum))
            {
                return;
            }

            leds.DelProgram(cardNum);
            leds.AddTextToProgram(cardNum, 0, 0, 32, 192, strContent, LED2008.YELLOW, true);
            leds.SendToScreen(cardNum);
        }


        public static void RefreshSingleLED(string ledNo, string strContent,bool blnUp)
        {
            int cardNum = Convert.ToInt32(ledNo);

            if (!IsOnLineLed(cardNum))
            {
                return;
            }
            int Y = 0;
            if (!blnUp)
            {
                Y = 16;
            }
            leds.AddSingleTextToProgram(cardNum, 0, Y, 16, 192, strContent, LED2008.YELLOW, true);
            leds.SendToScreen(cardNum);
        }


        private static bool IsOnLineLed(int ledNo)
        {
            if (isActiveLeds.ContainsKey(ledNo))
            {
                Microsoft.VisualBasic.Devices.Network network = new Microsoft.VisualBasic.Devices.Network();
                if (!network.Ping(isActiveLeds[ledNo]))
                {
                    //Logger.Error(ledNo + "号LED屏故障，请检查！IP:[" + isActiveLeds[ledNo] + "]");
                    return false;
                }
                else
                    return true;
            }
            else 
                return false;
        }

      
    }
}
