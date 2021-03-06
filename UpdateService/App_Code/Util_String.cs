using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Util_String 的摘要说明
/// </summary>
public class Util_String
{
    public Util_String()
    {

    }

    #region 判断是否为字符
    public static bool isChar(string instr)
    {
        for (int i = 0; i < instr.Length; i++)
        {
            if (!isChar(instr[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool isChar(char instr)
    {
        char tmp = instr;
        if ((tmp >= 'a' && tmp <= 'z') || (tmp >= 'A' && tmp <= 'Z'))
        {
            return true;
        }
        return false;
    }

    public static bool isChar(int instr)
    {
        int tmp = instr;
        if ((tmp >= 'a' && tmp <= 'z') || (tmp >= 'A' && tmp <= 'Z'))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region 获取中文字符串拼音码
    /// <summary>  
    /// 获取中文串对应的首字母字符串  
    /// </summary>  
    /// <param name="Cnstr">中文串</param>  
    /// <returns>中文串对应的首字母英文串</returns>  
    public static string GetSpellCode(string Cnstr)
    {
        string temp = "";
        char cur;
        int testnum;
        for (int i = 0; i < Cnstr.Length; i++)
        {
            cur = Cnstr[i];
            if (isChar(cur) || (int.TryParse(Cnstr[i].ToString(), out testnum) == true))
            {
                temp += cur;
            }
            else
            {
                temp += GetSpellCodeAt(Cnstr.Substring(i, 1));
            }
        }
        return temp;
    }

    private static string GetSpellCodeAt(string s)
    {
        long iCnChar;

        byte[] ZW = System.Text.Encoding.Default.GetBytes(s);
        if (ZW.Length == 1)
        {
            return s.ToUpper();
        }

        int i1 = (short)ZW[0];
        int i2 = (short)ZW[1];
        iCnChar = i1 * 256 + i2;

        if ((iCnChar >= 45217) && (iCnChar <= 45252))
        {
            return "A";
        }
        else if ((iCnChar >= 45253) && (iCnChar <= 45760))
        {
            return "B";
        }
        else if ((iCnChar >= 45761) && (iCnChar <= 46317))
        {
            return "C";
        }
        else if ((iCnChar >= 46318) && (iCnChar <= 46825))
        {
            return "D";
        }
        else if ((iCnChar >= 46826) && (iCnChar <= 47009))
        {
            return "E";
        }
        else if ((iCnChar >= 47010) && (iCnChar <= 47296))
        {
            return "F";
        }
        else if ((iCnChar >= 47297) && (iCnChar <= 47613))
        {
            return "G";
        }
        else if ((iCnChar >= 47614) && (iCnChar <= 48118))
        {
            return "H";
        }
        else if ((iCnChar >= 48119) && (iCnChar <= 49061))
        {
            return "J";
        }
        else if ((iCnChar >= 49062) && (iCnChar <= 49323))
        {
            return "K";
        }
        else if ((iCnChar >= 49324) && (iCnChar <= 49895))
        {
            return "L";
        }
        else if ((iCnChar >= 49896) && (iCnChar <= 50370))
        {
            return "M";
        }

        else if ((iCnChar >= 50371) && (iCnChar <= 50613))
        {
            return "N";
        }
        else if ((iCnChar >= 50614) && (iCnChar <= 50621))
        {
            return "O";
        }
        else if ((iCnChar >= 50622) && (iCnChar <= 50905))
        {
            return "P";
        }
        else if ((iCnChar >= 50906) && (iCnChar <= 51386))
        {
            return "Q";
        }
        else if ((iCnChar >= 51387) && (iCnChar <= 51445))
        {
            return "R";
        }
        else if ((iCnChar >= 51446) && (iCnChar <= 52217))
        {
            return "S";
        }
        else if ((iCnChar >= 52218) && (iCnChar <= 52697))
        {
            return "T";
        }
        else if ((iCnChar >= 52698) && (iCnChar <= 52979))
        {
            return "W";
        }
        else if ((iCnChar >= 52980) && (iCnChar <= 53688))
        {
            return "X";
        }
        else if ((iCnChar >= 53689) && (iCnChar <= 54480))
        {
            return "Y";
        }
        else if ((iCnChar >= 54481) && (iCnChar <= 55289))
        {
            return "Z";
        }
        else return ("?");
    }
    #endregion

    public static string SQLFilter(string str)
    {
        if (str == String.Empty)
            return String.Empty;
        str = str.Replace("'", "‘");
        str = str.Replace(";", "；");
        str = str.Replace(",", "，");
        str = str.Replace("?", "？");
        str = str.Replace("<", "＜");
        str = str.Replace(">", "＞");
        str = str.Replace("(", "（");
        str = str.Replace(")", "）");
        str = str.Replace("@", "＠");
        str = str.Replace("=", "＝");
        str = str.Replace("+", "＋");
        str = str.Replace("*", "＊");
        str = str.Replace("&", "＆");
        str = str.Replace("#", "＃");
        str = str.Replace("%", "％");
        str = str.Replace("$", "￥");

        return str;
    }

}