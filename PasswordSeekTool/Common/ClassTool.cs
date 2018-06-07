using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    public class ClassTool
    {
        public static List<Type> GetType(Type interfaceType)
        {
            List<Type> result = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == interfaceType)
                        {
                            result.Add(type);
                        }
                    }
                }
            }

            return result;
        }
    }
}
