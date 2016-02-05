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
        object VisualizingSource = null;
        #endregion

        #region Constructor
        public ListVisualizerForm(object VisualizingSource)
        {
            try
            {
                InitializeComponent();
                this.VisualizingSource = VisualizingSource;

                gridData.AllowUserToAddRows = false;
                //gridData.CellFormatting += GridData_CellFormatting;
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
                            jsonResult = JObject.Parse(cellValue);
                        }
                        catch (JsonReaderException ex) { }

                        if (jsonResult != null)
                        {
                            var linkCell = new DataGridViewLinkCell();
                            linkCell.UseColumnTextForLinkValue = false;
                            linkCell.Tag = row.Cells[column.Index].Value;
                            linkCell.Value = "Click";
                            linkCell.LinkBehavior = LinkBehavior.AlwaysUnderline;
                            row.Cells[column.Index] = linkCell;
                            grid[column.Index, row.Index].Value = "Hide";
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
                var cellValue = cell?.Tag?.ToString();
                MessageBox.Show(cellValue);
            }

            //throw new NotImplementedException();
        }
        #endregion

        #region Form Events
        private void ListVisualizerForm_Load(object sender, EventArgs e)
        {
            try
            {
                JToken rootObj = (JToken)VisualizingSource;
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
