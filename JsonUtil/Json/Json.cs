using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonUtil.Json
{
    public class Json
    {
        public String Result { get; set; }
        public String Result1 { get; set; }
        public String Result2 { get; set; }
        public String Result3 { get; set; }
        public MyClass3 MyClass3 { get; set; }
        public MyClass MyClass { get; set; }

        public Json()
        {
            this.Run();
        }

        public async void Run()
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    this.Result2 = JsonConvert.SerializeObject(new MyClass3());
                    this.Result2 += "";
                });

                await Task.Factory.StartNew(() =>
                {
                    this.Result3 = JsonConvert.SerializeObject(new MyClass4());
                    this.Result3 += "";
                });
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                this.Result = JsonConvert.SerializeObject(new MyClass3());
                this.Result += "";

                this.Result1 = JsonConvert.SerializeObject(new MyClass4());
                this.Result1 += "";
            }
            catch (Exception e)
            {
                throw e;
            }

            this.MyClass3 = JsonConvert.DeserializeObject<MyClass3>(this.Result2);
            this.MyClass = JsonConvert.DeserializeObject<MyClass>(this.Result2);

            TestMyClassConsistancy();

            String jsonMyClass = JsonConvert.SerializeObject(this.MyClass3.ObservableCollection[0].MyClass1.MyClasss[0]);
            this.MyClass = JsonConvert.DeserializeObject<MyClass>(jsonMyClass);

            TestMyClassConsistancy();
        }

        private Boolean TestMyClassConsistancy()
        {
            Boolean isFinded = false;
            foreach (var item in this.MyClass3.ObservableCollection)
            {
                var querry = (from g in item.MyClass1.MyClasss where g.Id == this.MyClass.Id select g);
                if (querry.ToList().Count != 0)
                {
                    isFinded = true;
                }
            }

            var seti = this.MyClass3.ObservableCollection.Where(x => x.Id != 10 || x.Id == 9);
                //.Select(x => x.Id == 1);

            var querry1 = (from g in this.MyClass3.ObservableCollection where g.MyClass1.MyClass.Id == this.MyClass.Id select g);

            if (querry1.ToList().Count != 0)
            {
                isFinded = true;
            }

            return isFinded;
        }
    }
}
