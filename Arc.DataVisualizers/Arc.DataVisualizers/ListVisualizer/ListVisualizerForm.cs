using Newtonsoft.Json.Linq;
using System;
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

                JObject obj = JObject.Parse(rootObj[0].ToString());

                foreach (var child in rootObj.Children())
                {
                    var type = child.Type;
                    foreach (var child1 in child.Children())
                    {
                        type = child1.Type;
                    }

                }


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

    }
}
