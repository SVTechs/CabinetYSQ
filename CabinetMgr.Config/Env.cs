using System;

namespace CabinetMgr.Config
{
    public class Env
    {
        public static string ConfigPath = "";

        public static string EncSeed = "CabinetMgr";

        public static string LocalDb = "vHQUCoDfttB+VYYXBlOVZtbhoiVGZ86xX1YRJ2bED4hh3KDy/+KN5oWNSaKvAs1Kv1fcBvqnqyF6nK3Amz2EvCh3UHV50dRQ+z13FgLnd4o=";
        public static string QcshkfDb = "9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDhBJNe6Kbw413/m53BCyhPyxb87KZExmqvWBlZrGdpI1lcTcFF18w5QdaK5TO3lo0+B1Xemd9fUJZAGwKJ2t71K";
            //"9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDjRVp8844HUa2277JXaJQ+oYgvWRooEA25BUiMPYa3RX4vi8X1vogV/WbjdMeEBfZY=";
        public static string QcshkfDbLocal = "vHQUCoDfttB+VYYXBlOVZguNoSIa8UTnQcmeSikKR+xBU6I+yY3XCyF3z2SS5n9t3L3mlvKQXziQse7TOyDnkQ==";
        public static string ShangHaiDevice1Db = "9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDhBJNe6Kbw413/m53BCyhPyAEgeKJ+gOdQusmeXyL9wKc2xJ1NleeH9UeRKntzLQQZCU/HOX44tJqOO5WbFpcXZ";
        public static string ShangHaiDevice1DbLocal = "vHQUCoDfttB+VYYXBlOVZqGvAD4JALPL2DD0oJZodWC2QrMRdCM5p9myZ8L77fuiLjVjlqqEZXkPzM2x27zOhEN2IsDlm+FvfCczOxHpzQ4=";
        public static string ShangHaiMeasureDb = "9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDhBJNe6Kbw413/m53BCyhPySn4GL1GGWp5V+yU8R6f4Qo8XF84W4hrjPG5aPv28Kbe2bypTWIV10AfJxkEYUyqg";
        public static string ShangHaiMeasureDbLocal = "vHQUCoDfttB+VYYXBlOVZqGvAD4JALPL2DD0oJZodWCPASNnEU1cVKzEjH9z6bi35Gornc8joRsKuJsC/UKF+8W508+rWuB0EYvFODHEvNw=";

        public static string RedisUserDb = "10.128.180.89";
        public static int RedisUserDbPort = 6379;

        public static int UhfDelay = 3;

        public static string DataType = "";

        public static DateTime MinTime = DateTime.Parse("1980-01-01 00:00:00"), MaxTime = DateTime.Parse("2099-12-31 23:59:59");
    }
}
