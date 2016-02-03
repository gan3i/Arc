using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Arc.DataVisualizers
{
    public partial class ListVisualizerForm : Form
    {
        object VisualizingSource = null;

        public ListVisualizerForm(object VisualizingSource)
        {
            try
            {
                InitializeComponent();
                this.VisualizingSource = VisualizingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListVisualizerForm_Load(object sender, EventArgs e)
        {
            try
            {
                var rootObj = (JArray)VisualizingSource;

                //JObject obj = JObject.Parse(rootObj[0].ToString());

                //foreach (var child in rootObj.Children())
                //{
                //    var type = child.Type;
                //    foreach (var child1 in child.Children())
                //    {
                //        type = child1.Type;
                //    }

                //}


                //var test = new JsonData(rootObj.ToString());
                //if (test.IsArray)
                //{
                //    foreach (var item in test.Objects)
                //    {
                //        foreach (var pair in item.Pairs)
                //        {

                //        }
                //    }
                //}

                gridData.DataSource = rootObj;
                gridData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        public JsonValues ParseJsonArray(JToken RootJson)
        {
            var rootType = RootJson.Type;
            var jsonValue = new JsonValues();
            switch (rootType)
            {
                case JTokenType.Object:

                    break;
                case JTokenType.Property:
                    var jproperty = (JProperty)RootJson;
                    jsonValue.Key = jproperty.Name;
                    jsonValue.IsJsonValue = (jproperty.Value.Type == JTokenType.String);
                    jsonValue.Value = jproperty.Value.ToString();
                    break;
                default:
                    //return null;
                    break;
            }
            return null;
        }

    }

    public class JsonValues
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsJsonValue { get; set; }
    }

    class JsonValuesCollection
    {
        List<JToken> _list = new List<JToken>();
        public JToken this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }


}
