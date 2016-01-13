using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void ListVisualizerForm_Load(object sender, EventArgs e)
        {
            try
            {
                var rootObj = (JArray)VisualizingSource;
                foreach (var listItem in rootObj.Children())
                {
                    //var type = listItem.Value<JToken>().Type;
                    foreach (var prop in listItem.Children())
                    {
                        if (prop.Type == JTokenType.Object)
                        {
                            MessageBox.Show(prop.ToString() + " is object");
                        }
                    }
                }
                gridData.DataSource = rootObj;
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
    }

    public class JsonData
    {
        string BaseJsonString = string.Empty;
        public JsonData(string JsonString)
        {
            this.BaseJsonString = JsonString;
        }
    }
}
