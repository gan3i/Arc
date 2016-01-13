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
                foreach (var child in rootObj.Children())
                {
                    var childValue = (JArray)child;
                    foreach (var child1 in child.Children())
                    {
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
}
