using System;

namespace CabinetService.Config
{
    public class Env
    {
        public static string CabinetServerDb = "qv1ZZvasfJbwsWCAPKlhVOTBMAxRljvnRtkV5ohRZwLiPKw/UQwWSZdk7X3zIVBcJeD4YYW8s7HaZNKcpOpsmYBujcbNcWUVdiSMYQymM+mr51kZAoMXeXMIctHgsS/f";
        //"vHQUCoDfttB+VYYXBlOVZtbhoiVGZ86xX1YRJ2bED4i8Xu4pcJRp6vFVfq1tcoxwohSz7vtH9nzVJpJikZQEBLluj9QtXMyvScInhCi6GNU=";
        //qv1ZZvasfJbwsWCAPKlhVOTBMAxRljvnRtkV5ohRZwLiPKw/UQwWSZdk7X3zIVBcJeD4YYW8s7HaZNKcpOpsmYBujcbNcWUVdiSMYQymM+mr51kZAoMXeXMIctHgsS/f
        //"9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDjIizth/my42EHeQtLnD8t5eYGSbBUzfbga+RVNdIVCXSs8kPJDJ9KPFUROS8YiiNmGoM1kWuQibckWYstmsFRK";

        public static string CabinetServerDbLocal = "vHQUCoDfttB+VYYXBlOVZtbhoiVGZ86xX1YRJ2bED4i8Xu4pcJRp6vFVfq1tcoxwohSz7vtH9nzVJpJikZQEBHad7srgPVqyixbP4/FJsb8=";

        public static string QcshkfDb = "9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDhBJNe6Kbw413/m53BCyhPyxb87KZExmqvWBlZrGdpI1lcTcFF18w5QdaK5TO3lo0+B1Xemd9fUJZAGwKJ2t71K";
        //"9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDjRVp8844HUa2277JXaJQ+oYgvWRooEA25BUiMPYa3RX4vi8X1vogV/WbjdMeEBfZY=";
        public static string QcshkfDbLocal = "vHQUCoDfttB+VYYXBlOVZguNoSIa8UTnQcmeSikKR+xBU6I+yY3XCyF3z2SS5n9t3L3mlvKQXziQse7TOyDnkQ==";

        public static string QcdeviceDb = "9fkEbUiRn5i8KszmXIgaiczdU860g09UdKubX+ZisDgwRIl50KQ2R2pJ1hQmrsjNYg9wpIJO7VedOFFlxZOBoaIyUGqAS15JKaH/Xn0Gyj3dUvDvntmMcASkLK4i8jFt";
        public static string QcdeviceDbLocal = "vHQUCoDfttB+VYYXBlOVZguNoSIa8UTnQcmeSikKR+zahVDPbrINyGwZn0dTjDkkxeDSdHY4l9DnvlIPUoPuQA==";

        public static string EncSeed = "CabinetMgr";
        public static string AppRoot = "";

        public static DateTime MinTime = DateTime.Parse("1980-01-01 00:00:00"), MaxTime = DateTime.Parse("2099-12-31 23:59:59");
    }
}
