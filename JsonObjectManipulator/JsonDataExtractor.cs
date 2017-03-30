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
        static Dictionary<string, JsonDataExtractor> _instances = new Dictionary<string, JsonDataExtractor>();
        static object _lock = new object();

        private JsonDataExtractor() { }

        public static JsonDataExtractor Instance(string key)
        {
            lock (_lock)
            {
                if (!_instances.ContainsKey(key)) _instances.Add(key, new JsonDataExtractor());
            }
            return _instances[key];
        }

        public void DestroyInstance(String key)
        {
            _instances.Remove(key);
        }

        private Dictionary<String, Object> baseJson = new Dictionary<string, object>();

        public Dictionary<String, Object> BaseJson
        {
            get { return baseJson; }
            set { baseJson = value; }
        }

        private Dictionary<String, Object> currentJson = new Dictionary<string, object>();

        public Dictionary<String, Object> CurrentJson
        {
            get { return currentJson; }
            set { currentJson = value; }
        }

        private List<Dictionary<String, Object>> stepList = new List<Dictionary<String, Object>>();

        public List<Dictionary<String, Object>> StepList
        {
            get { return stepList; }
            set { stepList = value; }
        }

        public Object GetFieldsAndData(String json)
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

        public void GetSubFieldsAndDatas(Object item)
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
                        Dictionary<String, Object> step = new Dictionary<string, object>();
                        step.Add("currentJson", result);
                        step.Add("GetSubFieldsAndDatas", "Dictionary");
                        stepList.Add(step);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(subDictionnary.Value.ToString());
                            dicoSave[subDictionnary.Key] = result;
                            Dictionary<String, Object> step = new Dictionary<string, object>();
                            step.Add("currentListJson", result);
                            step.Add("GetSubFieldsAndDatas", "List");
                            stepList.Add(step);
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
                            Dictionary<String, Object> step = new Dictionary<string, object>();
                            step.Add("currentListJson", result);
                            step.Add("GetSubFieldsAndDatas", "Dictionary");
                            stepList.Add(step);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(subDictionnary.Value.ToString());
                                dicoListSave = result as List<Dictionary<String, Object>>;
                                Dictionary<String, Object> step = new Dictionary<string, object>();
                                step.Add("currentListJson", result);
                                step.Add("GetSubFieldsAndDatas", "List");
                                stepList.Add(step);
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

        public string SendBaseJsonToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this.stepList.ElementAt(1)["currentJson"]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string SendCurrentJsonToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this.currentJson);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonDataExtractor Init(string jsonData)
        {
            this.baseJson = GetFieldsAndData(jsonData) as Dictionary<String,Object>;
            this.currentJson = this.baseJson;
            return this;
        }

        public JsonDataExtractor Purje()
        {
            this.baseJson = new Dictionary<string, object>();
            this.currentJson = new Dictionary<string, object>();
            this.stepList = new List<Dictionary<string, object>>();
            return this;
        }

        public object GetSubListObjectById(List<Dictionary<string, object>> meta, string choosed)
        {
            this.currentJson = meta.ElementAt(Int32.Parse(choosed));
            Dictionary<String, Object> step = new Dictionary<string, object>();
            step.Add("currentJson", this.currentJson);
            step.Add("GetSubListObjectById", choosed);
            this.stepList.Add(step);
            return this.currentJson;
        }

        public Object GetSubObjectByKey(Dictionary<string, object> meta, String key)
        {
            Object result = new object();
            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(meta[key].ToString());
                this.currentJson = result as Dictionary<String, Object>;
                Dictionary<String, Object> step = new Dictionary<string, object>();
                step.Add("currentJson", this.currentJson);
                step.Add("GetSubObjectByKey", key);
                this.stepList.Add(step);
            }
            catch (Exception ex)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(meta[key].ToString());
                    Dictionary<String, Object> step = new Dictionary<string, object>();
                    step.Add("currentListJson", result);
                    step.Add("GetSubObjectByKey", key);
                    this.stepList.Add(step);
                }
                catch (Exception ex1)
                {
                }
            }
            return result;
        }

        public Dictionary<string, object> FieldRemover(string keyToRemove)
        {
            this.currentJson.Remove(keyToRemove);
            Dictionary<String, Object> step = new Dictionary<string, object>();
            step.Add("currentJson", this.currentJson);
            step.Add("FieldRemover", keyToRemove);
            stepList.Add(step);
            return this.currentJson;
        }

        public Dictionary<string, object> FieldAdder(string keyToAdd, object value)
        {
            this.currentJson.Add(keyToAdd, value);
            Dictionary<String, Object> step = new Dictionary<string, object>();
            Dictionary<String, Object> field = new Dictionary<string, object>();
            field.Add(keyToAdd, value);
            step.Add("currentJson", this.currentJson);
            step.Add("FieldAdder", field);
            stepList.Add(step);
            return this.currentJson;
        }

        public List<String> GetSubObjectsKeys(Dictionary<string, object> meta)
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

        public bool HaveSubObject(Dictionary<string, object> meta)
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

        public String SendToJson(Dictionary<String, Object> toJson)
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
        public  JsonCompletItem GetFieldsAndData(String json)
        {
            JsonCompletItem result = new JsonCompletItem();
            result.Item = JsonConvert.DeserializeObject<Dictionary<String, Object>>(json);
            GetSubFieldsAndDatas(result);
            return result;
        }

        public  void GetSubFieldsAndDatas(JsonCompletItem item)
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

        public  String SendToJson(JsonCompletItem toJson)
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
