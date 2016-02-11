using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Arc.DataVisualizers
{
    public partial class ListVisualizerForm : Form
    {
        #region Variables & Properties



        public JToken VisualizingSource { get; private set; }
        #endregion

        #region Constructor
        public ListVisualizerForm(object VisualizingSource)
        {
            try
            {
                InitializeComponent();
                this.VisualizingSource = (JToken)VisualizingSource;

                gridData.AllowUserToAddRows = false;
                gridData.CellClick += GridData_CellClick;
                gridData.DataBindingComplete += GridData_DataBindingComplete;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        #endregion

        #region Grid Events
        private void GridData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                var grid = sender as DataGridView;

                foreach (DataGridViewRow row in grid.Rows)
                {
                    foreach (DataGridViewColumn column in grid.Columns)
                    {
                        var cellValue = row.Cells[column.Index]?.Value?.ToString();
                        if (cellValue == null)
                            return;
                        JToken jsonResult = null;
                        try
                        {
                            jsonResult = JToken.Parse(cellValue);
                        }
                        catch (JsonReaderException ex) { }

                        if (jsonResult != null && (jsonResult is JObject || jsonResult is JArray))
                        {
                            var linkCell = new DataGridViewLinkCell();
                            linkCell.Tag = jsonResult;
                            row.Cells[column.Index] = linkCell;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell is DataGridViewLinkCell)
            {
                var cellValue = cell?.Tag as JToken;
                if (cellValue != null)
                {
                    //MessageBox.Show(cellValue.ToString());
                    ListVisualizerForm frm = new ListVisualizerForm(cellValue);
                    frm.ShowDialog(this);
                }
            }
        }
        #endregion

        #region Form Events
        private void ListVisualizerForm_Load(object sender, EventArgs e)
        {
            try
            {
                JToken rootObj = VisualizingSource;
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
        #endregion

        #region Button Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion  
    }
}
