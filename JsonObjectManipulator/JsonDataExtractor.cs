using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonObjectManipulator
{
    public class JsonDataExtractor
    {
        public static Object GetFieldsAndData(String json)
        {
            Object result = new Object();
            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(json);
            }
            catch (Exception)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(json);
                }
                catch (Exception)
                {
                }
            }
            
            GetSubFieldsAndDatas(result);
            return result;
        }

        public static void GetSubFieldsAndDatas(Object item)
        {
            Object result = new object();
            if (item is Dictionary<String, Object>)
            {
                Dictionary<String, Object> dico = item as Dictionary<String, Object>;
                Dictionary<String, Object> dicoSave = new Dictionary<string, object>();
                foreach (var subDictionnary in dico)
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(subDictionnary.Value.ToString());
                        dicoSave[subDictionnary.Key] = result;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(subDictionnary.Value.ToString());
                            dicoSave[subDictionnary.Key] = result;
                        }
                        catch (Exception ex1)
                        {
                        }
                    }
                }
                item = dicoSave;
            }
            else if (item is List<Dictionary<String, Object>>)
            {
                List<Dictionary<String, Object>> dicoList = item as List<Dictionary<String, Object>>;
                List<Dictionary<String, Object>> dicoListSave = new List<Dictionary<string, object>>();
                foreach (var subList in dicoList)
                {
                    foreach (var subDictionnary in subList)
                    {
                        try
                        {
                            result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(subDictionnary.Value.ToString());
                            Dictionary<String, Object> temp = new Dictionary<string, object>();
                            temp.Add(subDictionnary.Key, result);
                            dicoListSave.Add(temp);

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(subDictionnary.Value.ToString());
                                /*List<Dictionary<String, Object>> temp = new List<Dictionary<string, object>>();
                                temp.Add(subDictionnary.Key, result);*/
                                dicoListSave = result as List<Dictionary<String, Object>>;
                            }
                            catch (Exception ex1)
                            {
                            }
                        }
                    }
                }
                item = dicoListSave;
            }
            else
            {
                return;
            }

            GetSubFieldsAndDatas(result);
        }

        public static object GetSubListObjectById(List<Dictionary<string, object>> meta, string choosed)
        {
            return meta.ElementAt(Int32.Parse(choosed));
        }

        public static Object GetSubObjectByKey(Dictionary<string, object> meta, String key)
        {
            Object result = new object();
            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(meta[key].ToString());
            }
            catch (Exception ex)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(meta[key].ToString());
                }
                catch (Exception ex1)
                {
                }
            }
            return result;
        }

        public static List<String> GetSubObjectsKeys(Dictionary<string, object> meta)
        {
            List<String> keys = new List<string>();
            foreach (var item in meta)
            {
                Object result = new object();
                try
                {
                    result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(item.Value.ToString());
                    keys.Add(item.Key);
                }
                catch (Exception ex)
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(item.Value.ToString());
                        keys.Add(item.Key);
                    }
                    catch (Exception ex1)
                    {
                    }
                }
            }
            return keys;
        }

        public static bool HaveSubObject(Dictionary<string, object> meta)
        {
            foreach (var item in meta)
            {
                Object result = new object();
                try
                {
                    result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(item.Value.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(item.Value.ToString());
                        return true;
                    }
                    catch (Exception ex1)
                    {
                    }
                }
            }

            return false;
        }

        public static String SendToJson(Dictionary<String, Object> toJson)
        {
            try
            {
                return JsonConvert.SerializeObject(toJson);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}



/*
 * using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonObjectManipulator
{
    public class JsonDataExtractor
    {
        public static JsonCompletItem GetFieldsAndData(String json)
        {
            JsonCompletItem result = new JsonCompletItem();
            result.Item = JsonConvert.DeserializeObject<Dictionary<String, Object>>(json);
            GetSubFieldsAndDatas(result);
            return result;
        }

        public static void GetSubFieldsAndDatas(JsonCompletItem item)
        {
            JsonCompletItem result = new JsonCompletItem();
            if (item.Item != null)
            {
            foreach (var subDictionnary in item.Item)
            {
                try
                {
                    result.Item = JsonConvert.DeserializeObject<Dictionary<String, Object>>(subDictionnary.Value.ToString());
                    item.AddItem(result);
                }
                catch (Exception ex)
                {
                    try
                    {
                        result.ItemList = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(subDictionnary.Value.ToString());
                        item.AddItem(result);
                    }
                    catch (Exception ex1)
                    {
                    }
                }
            }
            }
            else
            {
                foreach (var subDictionnaryList in item.ItemList)
                {
                    foreach (var subDictionnary in subDictionnaryList)
                    {
                    try
                    {
                        result.Item = JsonConvert.DeserializeObject<Dictionary<String, Object>>(subDictionnary.Value.ToString());
                        item.AddItem(result);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            result.ItemList = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(subDictionnary.Value.ToString());
                            item.AddItem(result);
                        }
                        catch (Exception ex1)
                        {
                        }
                        }

                    }
                }
            }
            GetSubFieldsAndDatas(result);
        }

        public static String SendToJson(JsonCompletItem toJson)
        {
            try
            {
                return JsonConvert.SerializeObject(toJson);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
*/
