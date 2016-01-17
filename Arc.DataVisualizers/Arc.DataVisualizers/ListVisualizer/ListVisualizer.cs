﻿using Microsoft.VisualStudio.DebuggerVisualizers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Arc.DataVisualizers.ListVisualizer),
typeof(Arc.DataVisualizers.ControlVisualizerObjectSource),
Target = typeof(System.Collections.Generic.List<object>),
Description = "List<T> Visualizer")]
namespace Arc.DataVisualizers
{
    public class ControlVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            ////using (var writer = new StreamWriter(outgoingData))
            //{
            //    var bin = new BinaryFormatter();
            //    bin.Serialize(outgoingData, target);
            //}

            StreamWriter writer = new StreamWriter(outgoingData);
            JsonTextWriter jsonWriter = new JsonTextWriter(writer);
            JsonSerializer ser = new JsonSerializer();
            ser.Serialize(jsonWriter, target);
            jsonWriter.Flush();

            //var serializer = new XmlSerializer(target.GetType());
            //serializer.Serialize(outgoingData, target);

            //var writer = new StreamWriter(outgoingData);
            //writer.WriteLine(target);
            //writer.Flush();
        }
    }
    public class ListVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            try
            {
                StreamReader reader = new StreamReader(objectProvider.GetData());
                JsonTextReader jsonReader = new JsonTextReader(reader);
                JsonSerializer ser = new JsonSerializer();
                var sourceObject = ser.Deserialize<dynamic>(jsonReader);

                //var sourceObject = objectProvider.GetObject();
                //MessageBox.Show(sourceObject.ToString());
                using (Form frm = new ListVisualizerForm(sourceObject))
                {
                    windowService.ShowDialog(frm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message + Environment.NewLine + ex.GetBaseException().StackTrace);
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(
            objectToVisualize,
            typeof(ListVisualizer),
            typeof(ControlVisualizerObjectSource));

            visualizerHost.ShowVisualizer();
        }
    }
}
