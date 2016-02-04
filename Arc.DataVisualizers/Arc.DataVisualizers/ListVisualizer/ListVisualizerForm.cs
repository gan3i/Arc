using Newtonsoft.Json;
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
                gridData.AllowUserToAddRows = false;

                gridData.RowPostPaint += GridData_RowPostPaint;
                gridData.CellFormatting += GridData_CellFormatting;
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
                JToken rootObj = (JObject)VisualizingSource;
                if (rootObj.Type == JTokenType.Object)
                {
                    rootObj = new JArray(rootObj);
                }

                gridData.DataSource = rootObj;
                gridData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        private void GridData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            var cellValue = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            JObject jsonResult = null;
            try
            {
                jsonResult = JObject.Parse(cellValue);
            }
            catch (JsonReaderException ex) { }

            if (jsonResult != null)
            {

            }
        }

        private void GridData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var value = (string)grid.Rows[e.RowIndex].DataBoundItem;
            DataGridViewCellStyle style = grid.Rows[e.RowIndex].DefaultCellStyle;
            // Do whatever you want with style and value

        }
    }
}
