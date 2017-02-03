using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonObjectManipulator
{
    public class JsonCompletItem
    {
        private Object item;

        public Object Item
        {
            get { return item; }
            set { item = value; }
        }

        private List<JsonCompletItem> jsonSubItems;

        public List<JsonCompletItem> JsonSubItems
        {
            get { return jsonSubItems; }
            set { jsonSubItems = value; }
        }

        public JsonCompletItem()
        {
            JsonSubItems = new List<JsonCompletItem>();
        }

        public JsonCompletItem AddItem(JsonCompletItem item)
        {
            this.JsonSubItems.Add(item);
            return this;
        }

        public JsonCompletItem RemoveItem(JsonCompletItem item)
        {
            this.JsonSubItems.Remove(item);
            return this;
        }
    }
}
