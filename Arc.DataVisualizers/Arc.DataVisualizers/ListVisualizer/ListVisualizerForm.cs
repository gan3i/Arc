using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
        TwoKeyDictionary<int, int, JToken> ObjectsList = new TwoKeyDictionary<int, int, JToken>();
        #endregion

        #region Constructor
        public ListVisualizerForm(object VisualizingSource)
        {
            try
            {
                InitializeComponent();
                this.WindowState = FormWindowState.Normal;
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

                gridData.DataSource = JToken.Parse(rootObj.ToString());
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
                        if (string.IsNullOrEmpty(cellValue))
                            return;

                        if (column.Name == IDCOLUMNNAME)
                        {
                            column.Visible = false;
                        }

                        JToken jsonResult = null;
                        try
                        {
                            jsonResult = JToken.Parse(cellValue);
                        }
                        catch (JsonReaderException ex) { }

                        if (jsonResult != null && (jsonResult is JObject || jsonResult is JArray))
                        {
                            ObjectsList[row.Index, column.Index] = jsonResult;

                            row.Cells[column.Index].Value = "<Object>";
                            //row.Cells[column.Index].Style.BackColor = System.Drawing.Color.Blue;
                            //row.Cells[column.Index].Tag = jsonResult;

                            //var linkCell = new DataGridViewLinkCell
                            //{
                            //    Tag = jsonResult
                            //};
                            //row.Cells[column.Index] = linkCell;
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = sender as DataGridView;
            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];

            var cellJson = ObjectsList[e.RowIndex, e.ColumnIndex];
            if (cellJson is JToken jData)
            {
                ListVisualizerForm frm = new ListVisualizerForm(jData, JReferences);
                frm.ShowDialog(this);
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

    #region TwoKeyDictionary Class
    public class TwoKeyDictionary<K1, K2, V> : ICollection
    {
        #region TwoKeysValue Class
        public class TwoKeysValue<TK1, TK2, TV>
        {
            public TK1 Key1 { get; set; }
            public TK2 Key2 { get; set; }
            public TV Value { get; set; }
        }
        #endregion

        #region Variables & Properties
        private List<TwoKeysValue<K1, K2, V>> m_list = new List<TwoKeysValue<K1, K2, V>>();
        #endregion

        #region Public Methods
        public V this[K1 key1, K2 key2]
        {
            get
            { /* return the specified index here */
                lock (m_list)
                {
                    var containsObject = m_list.Any(tkv => tkv.Key1.Equals(key1) && tkv.Key2.Equals(key2));
                    if (containsObject)
                        return m_list.First(tkv => tkv.Key1.Equals(key1) && tkv.Key2.Equals(key2)).Value;
                    return default(V);
                }
            }
            set
            { /* set the specified index to value here */
                //if (m_list.Any(tkv => tkv.Key1.Equals(key1) && tkv.Key2.Equals(key2)))
                //    throw new Exception("Same key combination already exists.");
                lock (m_list)
                {
                    if (m_list.Any(tkv => tkv.Key1.Equals(key1) && tkv.Key2.Equals(key2)))
                        m_list.First(tkv => tkv.Key1.Equals(key1) && tkv.Key2.Equals(key2)).Value = value;
                    else
                        m_list.Add(new TwoKeysValue<K1, K2, V>() { Key1 = key1, Key2 = key2, Value = value });
                }
            }
        }

        public IEnumerable<V> this[K1 key1]
        {
            get
            { /* return the specified index here */
                lock (m_list)
                    return m_list.Where(tkv => tkv.Key1.Equals(key1)).Select(tkv => tkv.Value);
            }
        }

        public IEnumerable<V> this[K2 key2]
        {
            get
            { /* return the specified index here */
                lock (m_list)
                    return m_list.Where(tkv => tkv.Key2.Equals(key2)).Select(tkv => tkv.Value);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)m_list).GetEnumerator();
        }
        #endregion

        #region ICollection Methods
        public int Count => ((ICollection)m_list).Count;
        public object SyncRoot => ((ICollection)m_list).SyncRoot;
        public bool IsSynchronized => ((ICollection)m_list).IsSynchronized;
        public void CopyTo(Array array, int index)
        {
            ((ICollection)m_list).CopyTo(array, index);
        }
        #endregion
    }
    #endregion
}