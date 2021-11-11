using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Utilities.DbHelper;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllPageFunctions
    {
        public static IList<PageFunctions> SearchPageFunctions()
        {
            return DalPageFunctions.SearchPageFunctions();
        }

        public static PageFunctions GetPageFunctions(string id)
        {
            return DalPageFunctions.GetPageFunctions(id);
        }

        public static Hashtable GetFunctionTable()
        {
            Hashtable funcTable = new Hashtable();
            IList<PageFunctions> funcList = SearchPageFunctions();
            if (SqlDataHelper.IsDataValid(funcList))
            {
                for (int i = 0; i < funcList.Count; i++)
                {
                    if (funcTable[funcList[i].FunctionMenu] == null)
                    {
                        funcTable[funcList[i].FunctionMenu] = new ArrayList();
                    }
                    ((ArrayList)funcTable[funcList[i].FunctionMenu]).Add(funcList[i]);
                }
            }
            return funcTable;
        }

        public static int AddPageFunction(string funcName, int funcOrder, string funcMenu, string funcDesp)
        {
            if (string.IsNullOrEmpty(funcName) || string.IsNullOrEmpty(funcMenu))
            {
                return -100;
            }
            return DalPageFunctions.AddPageFunction(funcName, funcOrder, funcMenu, funcDesp);
        }

        public static int ModifyPageFunction(string funcId, string funcName, int funcOrder, string funcDesp)
        {
            if (string.IsNullOrEmpty(funcId) || string.IsNullOrEmpty(funcName))
            {
                return -100;
            }
            return DalPageFunctions.ModifyPageFunction(funcId, funcName, funcOrder, funcDesp);
        }

        public static int DeletePageFunctions(string id)
        {
            return DalPageFunctions.DeletePageFunctions(id);
        }
    }
}
