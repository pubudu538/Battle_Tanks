using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Game_V1._2.Util
{
    class Constants
    {
        public static String ServerIP = ConfigurationManager.AppSettings.Get("ServerIP");
        //public static int ScreenWidth = int.Parse(ConfigurationManager.AppSettings.Get("ScreenWidth"));
        //public static int ScreenHeight = int.Parse(ConfigurationManager.AppSettings.Get("ScreenHeight"));
        public static int ServerPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));
        public static int ClientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));
        public static int MapSize = int.Parse(ConfigurationManager.AppSettings.Get("MapSize"));
        public static int Offset = int.Parse(ConfigurationManager.AppSettings.Get("Offset"));
        public static int ImageSize = int.Parse(ConfigurationManager.AppSettings.Get("ImageSize"));
        public static float Scale = float.Parse(ConfigurationManager.AppSettings.Get("Scale"));
        public static String JoinRequest = ConfigurationManager.AppSettings.Get("JoinRequest");
        public static String MoveUp = ConfigurationManager.AppSettings.Get("MoveUp");
        public static String MoveDown = ConfigurationManager.AppSettings.Get("MoveDown");
        public static String MoveRight = ConfigurationManager.AppSettings.Get("MoveRight");
        public static String MoveLeft = ConfigurationManager.AppSettings.Get("MoveLeft");
        public static String Shoot = ConfigurationManager.AppSettings.Get("Shoot");
        public static int Timeout = int.Parse(ConfigurationManager.AppSettings.Get("Timeout"));
        //public static float Scale = float.Parse(ConfigurationManager.AppSettings.Get("Scale"));

        public static int ScreenHeight = (int)(ImageSize * Scale * MapSize) + (Offset * 1);
        public static int ScreenWidth = (int)(ImageSize * Scale * MapSize) + (Offset * 3) + (95 * 4);
        //public static float Scale = MapSize/50;
    }
}
