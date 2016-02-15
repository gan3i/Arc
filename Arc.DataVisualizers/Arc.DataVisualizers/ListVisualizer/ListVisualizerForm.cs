using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Arc.DataVisualizers
{
    public partial class ListVisualizerForm : Form
    {
        #region Variables & Properties
        public JToken VisualizingSource { get; private set; }
        private const string IDCOLUMNNAME = "$id";
        private const string REFCOLUMNNAME = "$ref";

        Dictionary<int, JToken> JReferences { get; set; } = new Dictionary<int, JToken>();
        #endregion

        #region Constructor
        public ListVisualizerForm(object VisualizingSource)
        {
            try
            {
                InitializeComponent();
                this.WindowState = FormWindowState.Maximized;
                this.ShowInTaskbar = false;

                this.VisualizingSource = (JToken)VisualizingSource;

                gridData.AllowUserToAddRows = false;
                gridData.EditMode = DataGridViewEditMode.EditProgrammatically;
                gridData.CellClick += GridData_CellClick;
                gridData.DataBindingComplete += GridData_DataBindingComplete;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        private ListVisualizerForm(object VisualizingSource, Dictionary<int, JToken> ReferencesList) : this(VisualizingSource)
        {
            JReferences = ReferencesList;
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

                var children = rootObj.Children().ToList();
                foreach (var item in children)
                {
                    var firstProperty = item.First;
                    if (firstProperty != null && firstProperty is JProperty)
                    {
                        var firstJProperty = (JProperty)firstProperty;
                        if (firstJProperty.Name == IDCOLUMNNAME)
                        {
                            int objId = Convert.ToInt32(firstJProperty.Value.ToString());
                            if (JReferences.ContainsKey(objId))
                                JReferences[objId] = item;
                            else
                                JReferences.Add(objId, item);
                        }
                        else if (firstJProperty.Name == REFCOLUMNNAME)
                        {
                            int refId = Convert.ToInt32(firstJProperty.Value.ToString());
                            if (JReferences.ContainsKey(refId))
                            {
                                var refJson = JReferences[refId];
                                item.AddBeforeSelf(refJson);
                                item.Remove();
                            }
                        }
                    }
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

                        if (column.Name == IDCOLUMNNAME)
                        {

                        }

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
                    ListVisualizerForm frm = new ListVisualizerForm(cellValue, JReferences);
                    frm.ShowDialog(this);
                }
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