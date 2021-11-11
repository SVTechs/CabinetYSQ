using System.Collections;
using Ext.Net;

/// <summary>
/// PermissionControl 的摘要说明
/// </summary>
public class PermissionControl
{
    public static bool CheckPermission(object sessionObj, string funcCode)
    {
        if (sessionObj == null)
        {
            X.Msg.Alert("提示", "用户信息异常，请重新登录").Show();
            return false;
        }
        Hashtable permHt = (Hashtable)sessionObj;
        if (permHt[funcCode] == null)
        {
            X.Msg.Alert("提示", "用户权限不足").Show();
            return false;
        }
        return true;
    }
}