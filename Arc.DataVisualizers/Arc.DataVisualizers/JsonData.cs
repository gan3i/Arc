using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Arc.DataVisualizers
{
    public class JsonData
    {
        public bool IsArray
        {
            get
            {
                if (JsonObject == null)
                    throw new System.Exception("JsonObject is null.");
                return JsonObject.Type == JTokenType.Array;
            }
        }

        public IEnumerable<JsonData> Objects
        {
            get
            {
                if (!IsArray) yield break;
                foreach (var token in JsonObject.Children().Where(c => c.Type == JTokenType.Object))
                {
                    yield return new JsonData(token.ToString());
                }
            }
        }

        public IEnumerable<object> Pairs
        {
            get
            {
                foreach (var item in this.JsonObject)
                {
                }
                    return null;
            }
        }

        readonly JToken JsonObject = null;
        public JsonData(string JsonString)
        {
            JsonObject = JToken.Parse(JsonString);
        }
        public IEnumerable<JsonData> GetNext(JTokenType type)
        {
            foreach (var token in JsonObject.Children().Where(t => t.Type == type))
            {
                yield return new JsonData(token.ToString());
            }
        }
    }
    public class JsonValue
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public JsonValue(string JsonString)
        {
            var data = JToken.Parse(JsonString);
            foreach (var child in data.Children())
            {
                if (child.Type == JTokenType.Object)
                {

                }
            }
        }
    }
}

//MessageBox.Show(json.Type.ToString());
//foreach (var item in json.Children())
//{
//    if (item.Type == JTokenType.Property)
//    {
//        MessageBox.Show(item + " is a property");
//    }
//    if (item.Type == JTokenType.Object)
//    {
//        MessageBox.Show(item + " is an object");
//    }
//}
