using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PipeClient
{
    public class GenericObjectHelper
    {
        public static object[] CreateObject(string pClassName, string pObjStr)
        {
            string[] objects = pObjStr.Split(';');
            string[] structures = pClassName.Split(';');
            object[] values = new object[objects.Count()];
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < structures.Count(); i++)
            {
                string t = structures[i];
                if (t.Equals("DateTime"))
                {
                    values[i] = DateTime.Parse(objects[i]);
                }
                else if (t.Equals("Boolean"))
                {
                    values[i] = Boolean.Parse(objects[i]);
                }
                else if (t.Equals("Int"))
                {
                    values[i] = Int32.Parse(objects[i]);
                }
                else if (t.Equals("Decimal"))
                {
                    values[i] = Decimal.Parse(objects[i]);
                }
                else if (t.Equals("Double"))
                {
                    values[i] = Double.Parse(objects[i]);
                }
                else if (t.Equals("String"))
                {
                    values[i] = objects[i];
                }
                else if (t.StartsWith("List"))
                {
                    string genericVBClass = t.Substring(5, t.Length - 6);
                    Type genericType = Type.GetType(ConvertToDOTNETClass(genericVBClass));
                    Type listType = typeof(List<>);
                    IList instance = (IList)Activator.CreateInstance(listType.MakeGenericType(genericType));
                    string[] genericValue = objects[i].Substring(1, objects[i].Length - 2).Split(',');
                    for (int j = 0; j < genericValue.Count(); j++)
                    {
                        instance.Add(CreateObject(genericVBClass, genericValue[j])[0]);
                    }
                    values[i] = instance;
                }
                else if (t.StartsWith("Array"))
                {
                    string genericVBClass = t.Substring(6, t.Length - 7);
                    string[] genericValue = objects[i].Substring(1, objects[i].Length - 2).Split(',');
                    if (genericVBClass.Equals("String")) values[i] = genericValue;
                    else if (genericVBClass.Equals("Double")) values[i] = genericValue.Select(s => { return Double.Parse(s); }).ToArray();
                    else if (genericVBClass.Equals("Int")) genericValue.Select(s => { return Int32.Parse(s); }).ToArray();
                    else if (genericVBClass.Equals("Decimal")) genericValue.Select(s => { return Decimal.Parse(s); }).ToArray();
                    else
                        throw new NotSupportedException("Can not support this type.");
                }
            }
            return values;
        }

        /// <summary>
        /// สำหรับแปลงค่าจาก VB ที่ส่งมาเป็น AssymblyName ของ DOTNET นำไปใช้ต่อใน WmslTaskRequest
        /// </summary>
        /// <param name="pClassName">ระบุ Type คั้นด้วย ; ตัวอย่างเช่น Date;Int</param>
        /// <returns></returns>
        public static string ConvertToDOTNETClass(string pClassName)
        {
            string[] structures = pClassName.Split(';');
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < structures.Length; i++)
            {
                string t = structures[i];
                if (t.StartsWith("Array"))
                {
                    string name = t.Substring(6, t.Length - 7);
                    string type = "";
                    if (name.Equals("String")) type = (new string[1]).GetType().AssemblyQualifiedName;
                    else if (name.Equals("Double")) type = (new double[1]).GetType().AssemblyQualifiedName;
                    else if (name.Equals("Int")) type = (new int[1]).GetType().AssemblyQualifiedName;
                    else if (name.Equals("Decimal")) type = (new decimal[1]).GetType().AssemblyQualifiedName;
                    else
                        throw new NotSupportedException("Can not support this type.");
                    strBuilder.Append(type + ";");
                }
                else if (t.StartsWith("List"))
                {
                    Type genericType = Type.GetType(ConvertToDOTNETClass(t.Substring(5, t.Length - 6)));
                    Type listType = typeof(List<>);
                    IList instance = (IList)Activator.CreateInstance(listType.MakeGenericType(genericType));
                    strBuilder.Append(instance.GetType().AssemblyQualifiedName + ";");
                }
                else if (t.Equals("DateTime"))
                {
                    strBuilder.Append(typeof(DateTime).AssemblyQualifiedName + ";");
                }
                else if (t.Equals("Boolean"))
                {
                    strBuilder.Append(typeof(bool).AssemblyQualifiedName + ";");
                }
                else if (t.Equals("Int"))
                {
                    strBuilder.Append(typeof(int).AssemblyQualifiedName + ";");
                }
                else if (t.Equals("Decimal"))
                {
                    strBuilder.Append(typeof(decimal).AssemblyQualifiedName + ";");
                }
                else if (t.Equals("Double"))
                {
                    strBuilder.Append(typeof(double).AssemblyQualifiedName + ";");
                }
                else if (t.Equals("String"))
                {
                    strBuilder.Append(typeof(string).AssemblyQualifiedName + ";");
                }
                else
                {
                    //object ที่เป็นพวก Wmsl DTO
                    strBuilder.Append(t + ";");
                }
            }
            return strBuilder.ToString().Substring(0, strBuilder.Length - 1);
        }
        /// <summary>
        /// สำหรับแปลงค่าคืนกลับให้ vb สามารถอ่านได้ ไม่ support Dictionary , ตัวอย่าง return Date;Int
        /// </summary>
        /// <param name="pClassName">ระบุ AssymblyName คั้นด้วย ; ตัวอย่างเช่น System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089;System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</param>
        /// <returns></returns>
        public static string ConvertToVBClass(string pClassName)
        {
            string[] structures = pClassName.Split(';');
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < structures.Length; i++)
            {
                Type t = Type.GetType(structures[i]);
                if (t.IsArray && t.IsAnsiClass) // Array DataType of System
                {
                    string name = t.Name.Substring(0, t.Name.Length - 2);

                    string genericType = "";

                    if (name.Equals(typeof(string).Name)) genericType = ConvertToVBClass(typeof(string).AssemblyQualifiedName);
                    else if (name.Equals(typeof(double).Name)) genericType = ConvertToVBClass(typeof(double).AssemblyQualifiedName);
                    else if (name.Equals(typeof(int).Name)) genericType = ConvertToVBClass(typeof(int).AssemblyQualifiedName);
                    else if (name.Equals(typeof(decimal).Name)) genericType = ConvertToVBClass(typeof(decimal).AssemblyQualifiedName);
                    else
                        throw new NotSupportedException("Can not support this type.");

                    strBuilder.Append(string.Format("Array[{0}];", genericType));
                }
                else if (t.IsGenericType && t.IsAnsiClass) // List DataType of System
                {
                    string genericType = ConvertToVBClass(t.GetGenericArguments()[0].AssemblyQualifiedName);

                    strBuilder.Append(string.Format("List[{0}];", genericType));
                }
                else
                {
                    if (t.Name.Equals(typeof(DateTime).Name))
                    {
                        strBuilder.Append("DateTime;");
                    }
                    else if (t.Name.Equals(typeof(bool).Name))
                    {
                        strBuilder.Append("Boolean;");
                    }
                    else if (t.Name.Equals(typeof(int).Name))
                    {
                        strBuilder.Append("Int;");
                    }
                    else if (t.Name.Equals(typeof(decimal).Name))
                    {
                        strBuilder.Append("Decimal;");
                    }
                    else if (t.Name.Equals(typeof(double).Name))
                    {
                        strBuilder.Append("Double;");
                    }
                    else if (t.Name.Equals(typeof(string).Name))
                    {
                        strBuilder.Append("String;");
                    }
                    else //Convert Object > Json Key:Value PropertyName:Type
                    {
                        IDictionary<string, string> type = new Dictionary<string, string>();
                        foreach (PropertyInfo prop in t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
                        {
                            type.Add(prop.Name, ConvertToVBClass(prop.PropertyType.AssemblyQualifiedName));
                        }
                        strBuilder.Append(JsonConvert.SerializeObject(type, new JsonCustomDateType()) + ";");
                    }
                }

            }
            return strBuilder.ToString().Substring(0, strBuilder.Length - 1);
        }
    }
}
