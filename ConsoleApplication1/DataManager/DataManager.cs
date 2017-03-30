using ClassLibrary1.Entities;
using ConsoleUtil;
using DatabaseClassLibrary.Database;
using DatabaseManagerUtil.Database;
using JsonObjectManipulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class DataManager
    {
        private static DataManager instance;

        private UserMySqlManager userManager;
        private MySQLManager<Data> dataManager;

        private DataManager()
        {
            userManager = new UserMySqlManager();
            dataManager = new MySQLManager<Data>(DataConnectionResource.LOCALMYSQL);
        }

        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }

        public async void PrintUsers()
        {
            foreach (var item in (await userManager.Get()).ToList())
            {
                Console.WriteLine("Id: " + item.UserId + "  Firstname: " + item.Firstname + "  Lastname: " + item.Lastname);
            }
        }

        public void PrintUserDatas(User user)
        {
            ICollection<Data> datas = (userManager.GetDatas(user)).Datas;
            foreach (var item in datas)
            {
                PrintData(item, () => { });
            }
            SelectDataById(datas);
        }

        public void PrintData(Data data, Action action)
        {
            if (data != null)
            {
                Console.WriteLine("Id: " + data.Id + "  Data: " + data.JsonData);
                action.Invoke();
            }
            else
            {
                ConsoleManager.Instance.Issue("No data exist for this id");
            }
        }

        private void PrintModification(Data data)
        {
            Object meta = JsonDataExtractor.Instance("json1").Init(data.JsonData).BaseJson;
            ConsoleManager.Instance.LoopSinceExit("exit", () =>
            {
                PrintModificationMenu(data, meta as Dictionary<String, Object>);
            });
        }

        private void PrintModificationMenu(Data data, List<Dictionary<String, Object>> meta)
        {
            Console.WriteLine("Select item by is position :");
            int i = 0;
            List<String> keys = new List<string>();
            foreach (var item in meta)
            {
                keys.Add(i.ToString());
                Console.WriteLine(i);
                i++;
            }
            Int32 selectedItem;
            Int32.TryParse(Console.ReadLine(), out selectedItem);
            String choosed = ConsoleManager.Instance.LoopSinceChoseOrExit(keys, "exit");
            Object dicoItem = JsonDataExtractor.Instance("json1").GetSubListObjectById(meta, choosed);

            if (dicoItem is Dictionary<String, Object>)
            {
                PrintModificationMenu(data, dicoItem as Dictionary<String, Object>);
            }
            else
            {
                PrintModificationMenu(data, dicoItem as List<Dictionary<String, Object>>);
            }
        }

        private async void PrintModificationMenu(Data data, Dictionary<String, Object> meta)
        {
            Console.WriteLine("Object structure :");
            foreach (var item in meta as Dictionary<String, Object>)
            {
                Console.WriteLine(item.Key + " : " + item.Value);
            }
            Console.WriteLine();
            Console.WriteLine("1 : remove field");
            Console.WriteLine("2 : add field");
            Console.WriteLine("3 : update field");
            Console.WriteLine("4 : remove current object");
            if (JsonDataExtractor.Instance("json1").HaveSubObject(meta))
            {
                Console.WriteLine("5 : navigate to object");
            }
            else
            {
                Console.WriteLine("9 : validate");
            }
            
            Console.WriteLine("exit to quit");

            Int32 selectedItem;
            Int32.TryParse(Console.ReadLine(), out selectedItem);
            switch (selectedItem)
            {
                case 1:
                    meta = FieldRemover(meta);
                    String metaJson = JsonDataExtractor.Instance("json1").SendToJson(meta);
                    break;
                case 2:
                    meta = FieldAdder(meta);
                    break;
                case 3:
                    break;
                case 5:
                    List<String> keys = JsonDataExtractor.Instance("json1").GetSubObjectsKeys(meta);
                    Console.WriteLine("Avaibles keys :");
                    foreach (var item in keys)
                    {
                        Console.WriteLine("- " + item);
                    }
                    String choosed = ConsoleManager.Instance.LoopSinceChoseOrExit(keys, "exit");
                    Object dicoItem = JsonDataExtractor.Instance("json1").GetSubObjectByKey(meta, choosed);
                    if (dicoItem is Dictionary<String, Object>)
                    {
                        PrintModificationMenu(data, dicoItem as Dictionary<String, Object>);
                    }
                    else
                    {
                        PrintModificationMenu(data, dicoItem as List<Dictionary<String, Object>>);
                    }
                    
                    break;
                case 9:
                    String result = JsonDataExtractor.Instance("json1").SendBaseJsonToString();
                    if (result != null)
                    {
                        data.JsonData = result;
                        await dataManager.Update(data);
                    }
                    else
                    {
                        Console.WriteLine("Json is corrupted!");
                    }
                    break;
                default:
                    break;
            }
        }

        private Dictionary<String, Object> FieldAdder(Dictionary<String, Object> meta)
        {
            ConsoleManager.Instance.LoopSinceExit("Continue ? Y/N", "N",
                () =>
                {
                    Console.WriteLine("Existing keys :");
                    foreach (var item in meta)
                    {
                        Console.Write(item.Key + ", ");
                    }

                    Console.WriteLine();

                    Console.WriteLine("New key to create :");
                    String key = ConsoleManager.Instance.LinePromptSaver();

                    Console.WriteLine("New value for key " + key + " :");
                    String value = ConsoleManager.Instance.LinePromptSaver();

                    meta = JsonDataExtractor.Instance("json1").FieldAdder(key, value);
                });
            return meta;
        }

        private Dictionary<String, Object> FieldRemover(Dictionary<String, Object> meta)
        {
            Console.WriteLine("Select key to remove");
            foreach (var item in meta)
            {
                Console.WriteLine(item.Key);
            }
            String keyToRemove = ConsoleManager.Instance.LoopSinceChoseOrExit(meta.Keys, "exit");

            return JsonDataExtractor.Instance("json1").FieldRemover(keyToRemove);
        }

        public void SelectDataById(ICollection<Data> datas)
        {
            Console.WriteLine("Select a data to pick up using his Id");
            String value = ConsoleManager.Instance.GetCommandLineValue();
            Int32 dataId;
            Int32.TryParse(value, out dataId);
            if (dataId != 0)
            {
                Data data = datas.FirstOrDefault(x => x.Id == dataId);
                PrintData(data, () => { PrintModification(data); });
            }
            else
            {
                ConsoleManager.Instance.Issue("Bad input");
            }

        }

        public async void PrintUserChoice()
        {
            Console.WriteLine("Select a user to pick up using his Id");
            PrintUsers();
            String value = ConsoleManager.Instance.GetCommandLineValue();
            Int32 userId;
            Int32.TryParse(value, out userId);
            if (userId == 0)
            {
                ConsoleManager.Instance.Issue("Bad input");
                PrintUserChoice();
            }
            User user = await userManager.Get(userId);
            PrintUserDatas(user);
        }
    }
}
