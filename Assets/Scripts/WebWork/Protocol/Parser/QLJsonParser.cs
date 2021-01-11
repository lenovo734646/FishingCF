using QL.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace QL.Parser
{
    /// <summary>
    /// QL JSON响应通用解释器。
    /// </summary>
    public class QLJsonParser : IQLParser
    {
        private static readonly Dictionary<string, Dictionary<string, QLAttribute>> attrs_ = new Dictionary<string, Dictionary<string, QLAttribute>>();

        #region IQLParser Members

        private Dictionary<string, object> convertJObject(JObject t)
        {
            var r = new Dictionary<string, object>();
            var data = t.ToObject<Dictionary<string, object>>();
            if (data != null)
            {
                foreach (var pair in data)
                {
                    var v = pair.Value;
                    if (v is JObject)
                        v = convertJObject(v as JObject);
                    else if (v is JArray)
                        v = convertJArray(v as JArray);
                    r[pair.Key] = v;
                }
            }
            return r;
        }
        private List<object> convertJArray(JArray t)
        {
            var r = new List<object>();
            var data = t.ToObject<List<object>>();
            if (data != null)
            {
                foreach (var obj in data)
                {
                    var v = obj;
                    if (v is JObject)
                        v = convertJObject(v as JObject);
                    else if (v is JArray)
                        v = convertJArray(v as JArray);
                    r.Add(v);
                }
            }
            return r;
        }

        public virtual T Parse<T>(string body) where T : QLResponse
        {
            T rsp = null;

            var json = convertJObject(JObject.Parse(body));
            if (json != null)
            {
                IDictionary data = null;

                // 忽略根节点的名称
                foreach (string key in json.Keys)
                {
                    data = json[key] as IDictionary;
                    break;
                }

                if (data != null)
                {
                    IQLReader reader = new QLJsonReader(data);
                    rsp = (T)FromJson(reader, typeof(T));
                }
            }

            if (rsp == null)
            {
                rsp = Activator.CreateInstance<T>();
            }

            if (rsp != null)
            {
                rsp.Body = body;
            }

            return rsp;
        }

        #endregion

        private Dictionary<string, QLAttribute> GetQLAttributes(Type type)
        {
            Dictionary<string, QLAttribute> tas = null;

            lock (attrs_)
            {
                attrs_.TryGetValue(type.FullName, out tas);
            }

            if (tas != null) // 从缓存中获取类属性元数据
                return tas;

            tas = new Dictionary<string, QLAttribute>();

            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                QLAttribute qa = new QLAttribute();
                qa.Method = pi.GetSetMethod();

                // 获取对象属性名称
                XmlElementAttribute[] xeas = pi.GetCustomAttributes(typeof(XmlElementAttribute), true) as XmlElementAttribute[];
                if (xeas != null && xeas.Length > 0)
                {
                    qa.ItemName = xeas[0].ElementName;
                }

                // 获取列表属性名称
                if (qa.ItemName == null)
                {
                    XmlArrayItemAttribute[] xaias = pi.GetCustomAttributes(typeof(XmlArrayItemAttribute), true) as XmlArrayItemAttribute[];
                    if (xaias != null && xaias.Length > 0)
                    {
                        qa.ItemName = xaias[0].ElementName;
                    }
                    XmlArrayAttribute[] xaas = pi.GetCustomAttributes(typeof(XmlArrayAttribute), true) as XmlArrayAttribute[];
                    if (xaas != null && xaas.Length > 0)
                    {
                        qa.ListName = xaas[0].ElementName;
                    }
                    if (qa.ListName == null)
                    {
                        continue;
                    }
                }

                // 获取属性类型
                if (pi.PropertyType.IsGenericType)
                {
                    Type[] types = pi.PropertyType.GetGenericArguments();
                    qa.ListType = types[0];
                }
                else
                {
                    qa.ItemType = pi.PropertyType;
                }

                tas.Add(pi.Name, qa);
            }

            lock (attrs_)
            {
                attrs_[type.FullName] = tas;
            }

            return tas;
        }

        public object FromJson(IQLReader reader, Type type)
        {
            object rsp = null;
            Dictionary<string, QLAttribute> pas = GetQLAttributes(type);

            Dictionary<string, QLAttribute>.Enumerator em = pas.GetEnumerator();
            while (em.MoveNext())
            {
                KeyValuePair<string, QLAttribute> kvp = em.Current;
                QLAttribute qa = kvp.Value;
                string itemName = qa.ItemName;
                string listName = qa.ListName;

                if (!reader.HasReturnField(itemName) && (string.IsNullOrEmpty(listName) || !reader.HasReturnField(listName)))
                {
                    continue;
                }

                object value = null;
                if (qa.ListType != null)
                {
                    value = reader.GetListObjects(listName, itemName, qa.ListType, FromJson);
                }
                else
                {
                    if (typeof(string) == qa.ItemType)
                    {
                        object tmp = reader.GetPrimitiveObject(itemName);
                        if (typeof(string) == tmp.GetType())
                        {
                            value = tmp;
                        }
                        else
                        {
                            if (tmp != null)
                            {
                                value = tmp.ToString();
                            }
                        }
                    }
                    else if (typeof(long) == qa.ItemType)
                    {
                        object tmp = reader.GetPrimitiveObject(itemName);
                        if (typeof(long) == tmp.GetType())
                        {
                            value = tmp;
                        }
                        else
                        {
                            if (tmp != null)
                            {
                                value = long.Parse(tmp.ToString());
                            }
                        }
                    }
                    else if (typeof(bool) == qa.ItemType)
                    {
                        object tmp = reader.GetPrimitiveObject(itemName);
                        if (typeof(bool) == tmp.GetType())
                        {
                            value = tmp;
                        }
                        else
                        {
                            if (tmp != null)
                            {
                                value = bool.Parse(tmp.ToString());
                            }
                        }
                    }
                    else
                    {
                        value = reader.GetReferenceObject(itemName, qa.ItemType, FromJson);
                    }
                }

                if (value != null)
                {
                    if (rsp == null)
                    {
                        rsp = Activator.CreateInstance(type);
                    }
                    qa.Method.Invoke(rsp, new object[] { value });
                }
            }

            return rsp;
        }
    }
}
