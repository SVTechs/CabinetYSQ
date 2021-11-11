using System;

namespace WebConsole.Config
{
    public class Env
    {
        public static string UnitName = "迎水桥机务段";
        public static string AppVer = "1.0.0.0";
        public static string AppName = "智能工具柜管理系统";

        public static int LocalMode = 0;

        public static string EncSeed = "CabinetMgr";
        public static string AppRoot = "/";

        public static string DefaultPwd = "123456";

        public static DateTime MinTime = DateTime.Parse("1980-01-01 00:00:00"), MaxTime = DateTime.Parse("2099-12-31 23:59:59");

        public static string AsposeLicense = @"<License>
<Data>
<LicensedTo>Shanghai Hudun Information Technology Co., Ltd</LicensedTo>
<EmailTo>317701809@qq.com</EmailTo>
<LicenseType>Developer OEM</LicenseType>
<LicenseNote>Limited to 1 developer, unlimited physical locations</LicenseNote>
<OrderID>180514201116</OrderID>
<UserID>266166</UserID>
<OEM>This is a redistributable license</OEM>
<Products>
<Product>Aspose.Total for .NET</Product>
</Products>
<EditionType>Enterprise</EditionType>
<SerialNumber>210ec8e7-81e1-4537-b446-692de4981217</SerialNumber>
<SubscriptionExpiry>20190517</SubscriptionExpiry>
<LicenseVersion>3.0</LicenseVersion>
<LicenseInstructions>http://www.aspose.com/corporate/purchase/license-instructions.aspx</LicenseInstructions>
</Data>
<Signature>ctJ3yLxSAPsBQd0Jcqf7CA53FzN1YrvaA5dSrTpdFW/Afh0hyKKwry+C1tjWIOEFyzKYWH+Ngn/HeXUzMQJA0Roowcq112nV/QnrSSqDm6FJVNssH4p/YmXRjl7LBixwV8AbyWX8lhVoyok7lI5k5K8bbaK+T8Ur+jIwSZAcmVA=</Signature>
</License>";
    }
}
