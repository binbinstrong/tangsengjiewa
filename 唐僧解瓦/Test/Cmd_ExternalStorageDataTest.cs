using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 外部存储测试
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class OtherTest : IExternalCommand
    {
        Autodesk.Revit.DB.Document doc = null;
        UIApplication uiapp = null;
        UIDocument uidoc = null;
        string _volume = string.Empty;
        string _area = string.Empty;
        object obj = new object();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uiapp = commandData.Application;
            uidoc = commandData.Application.ActiveUIDocument;
            doc = commandData.Application.ActiveUIDocument.Document;
            object obj = new object();
            using (Transaction trans = new Transaction(doc, "xxx"))
            {
                trans.Start();

                //Remove();
                Data data = new Data();
                FaceRecorder.Instance(doc, data).Recorder();
                double d = FaceRecorder.Instance(doc, data).Extract("a1");

                trans.Commit();
            }


            return Result.Succeeded;
        }

        public void Remove()
        {
            List<DataStorage> dss = new FilteredElementCollector(doc).OfClass(typeof(DataStorage)).Cast<DataStorage>().ToList();
            foreach (DataStorage item in dss)
            {
                doc.Delete(item.Id);
            }
            Schema s = Schema.Lookup(new Guid("d07f0dc5-b028-45c0-b5e7-9583353315d6"));
            Schema.EraseSchemaAndAllEntities(s, true);
        }

    }

    public class FaceRecorder
    {
        private Document _doc = null;
        private Schema _schema = null;
        private IFaceRecordData _data = null;
        private static FaceRecorder _instance;
        private static readonly object syncRoot = new object();
        public static FaceRecorder Instance(Document doc, IFaceRecordData recordedData)
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new FaceRecorder(doc, recordedData);
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// 私有构造
        /// </summary>
        /// <param name="recordedData"></param>
        private FaceRecorder(Document doc, IFaceRecordData recordedData)
        {
            _doc = doc;
            _data = recordedData;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Recorder()
        {
            SchemaBuilder builder = new SchemaBuilder(_data.Guid);
            builder.SetWriteAccessLevel(AccessLevel.Public);
            builder.SetReadAccessLevel(AccessLevel.Public);
            builder.SetSchemaName(_data.SchemaName);
            foreach (RecordData item in _data.Fields)
            {
                if (item.Type == typeof(string) || item.Type == typeof(bool))
                    builder.AddSimpleField(item.Key, item.Type);
                else
                {
                    FieldBuilder fb = builder.AddSimpleField(item.Key, item.Type);
                    fb.SetUnitType(UnitType.UT_Length);
                }
            }
            _schema = builder.Finish();
            Entity ent = new Entity(_schema);
            foreach (RecordData item in _data.Fields)
            {
                ent.Set(item.Key, item.Value, DisplayUnitType.DUT_METERS);
            }
            // 仓库
            DataStorage st = DataStorage.Create(_doc);
            st.Name = "myStorage";
            st.SetEntity(ent);
        }

        public dynamic Extract(string fieldName)
        {
            DataStorage ds = new FilteredElementCollector(_doc).OfClass(typeof(DataStorage)).Cast<DataStorage>().FirstOrDefault(x => x.Name == "myStorage");
            Schema schema = Schema.Lookup(_data.Guid);
            Type t = _data.Fields.FirstOrDefault(x => x.Key == fieldName).Type;
            Entity e = ds.GetEntity(schema);
            var o = e.GetType().GetMethod("Get", new Type[] { typeof(string) }).MakeGenericMethod(t).Invoke(e, new object[] { fieldName });
            //object o = ds.GetEntity(schema).Get<t>(fieldName); // 这里有错误
            dynamic d = Convert.ChangeType(o, t);
            return d;
        }
    }

    public class Data : IFaceRecordData
    {
        public Guid Guid { get { return new Guid("d07f0dc5-b028-45c0-b5e7-9583353315d7"); } }

        public string SchemaName { get { return "mySchema"; } }

        public string StorageName { get { return "myStorage"; } }

        List<RecordData> IFaceRecordData.Fields
        {
            get
            {
                return new List<RecordData>() { new RecordData("a1", 2.2), new RecordData("a2", true), new RecordData("a3", "你好") };
            }
        }
    }

    public interface IFaceRecordData
    {
        /// <summary>
        /// 字段字典
        /// </summary>
        List<RecordData> Fields { get; }
        /// <summary>
        /// 架构名是必须的
        /// </summary>
        string SchemaName { get; }
        /// <summary>
        /// 设置一个仓库名是有必要的
        /// </summary>
        string StorageName { get; }
        /// <summary>
        /// 记好你的Guid
        /// </summary>
        Guid Guid { get; }
    }

    public class RecordData
    {
        public string Key { get; set; }
        public Type Type { get; set; }
        public dynamic Value { get; set; }
        public RecordData(string key, dynamic value)
        {
            this.Key = key;
            this.Value = value;
            this.Type = value.GetType();
        }
    }
}
