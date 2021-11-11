using System.Collections;
using CabinetMgr.Dal;

namespace CabinetMgr.Bll
{
    public class BllProcessMagta
    {
        public static int GenerateAllProcessMagta()
        {
            return DalProcessMagta.GenerateAllProcessMagta();
        }

        public static IList GetProcessMagta(string processDefinitionId)
        {
            return DalProcessMagta.GetProcessMagta(processDefinitionId);
        }
    }
}
