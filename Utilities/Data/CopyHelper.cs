using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.FileHelper;

namespace Utilities.Data
{
    public class CopyHelper
    {
        public static object DowngradeCopy(Type originType, Type newType, object originData)
        {
            try
            {
                object itemInstance = newType.InvokeMember("", BindingFlags.CreateInstance, null, null, null);
                PropertyInfo[] properties = newType.GetProperties();
                PropertyInfo[] originProperties = originType.GetProperties();
                if (properties.Length > 0)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        int j;
                        bool isExist = false;
                        for (j = 0; j < originProperties.Length; j++)
                        {
                            if (originProperties[j].Name.Equals(properties[i].Name))
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist)
                        {
                            try
                            {
                                object originValue = originProperties[j].GetValue(originData, null);
                                properties[i].SetValue(itemInstance, originValue, null);
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                }
                return itemInstance;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }
}
