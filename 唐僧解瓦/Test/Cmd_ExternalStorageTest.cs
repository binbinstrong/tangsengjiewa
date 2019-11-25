namespace TestForGe
{
    //[Transaction(TransactionMode.Manual)]
    //class OtherTest : IExternalCommand
    //{
    //    Autodesk.Revit.DB.Document doc = null;
    //    UIApplication uiapp = null;
    //    UIDocument uidoc = null;
    //    string _volume = string.Empty;
    //    string _area = string.Empty;
    //    object obj = new object();

    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        uiapp = commandData.Application;
    //        uidoc = commandData.Application.ActiveUIDocument;
    //        doc = commandData.Application.ActiveUIDocument.Document;
    //        object obj = new object();
    //        int a = 0;
    //        using (Transaction trans = new Transaction(doc, "xxx"))
    //        {
    //            trans.Start();
    //            VM vm = new VM("小王", 20 /*, Sex.男*/);
    //            Data data = new Data();
    //            FaceRecorder.Instance(doc, data, vm).Recorder();
    //            string name = FaceRecorder.Instance(doc, data, vm).Extract("Name");
    //            int age = FaceRecorder.Instance(doc, data, vm).Extract("Age");
    //            double D = FaceRecorder.Instance(doc, data, vm).Extract("D");
    //            float F = FaceRecorder.Instance(doc, data, vm).Extract("F");
    //            bool B = FaceRecorder.Instance(doc, data, vm).Extract("B");
    //            //string none = FaceRecorder.Instance(doc, data, vm).Extract("None");
    //            TaskDialog.Show("1", name.ToString());
    //            TaskDialog.Show("1", age.ToString());
    //            TaskDialog.Show("1", D.ToString());
    //            TaskDialog.Show("1", F.ToString());
    //            TaskDialog.Show("1", B.ToString());
    //            //TaskDialog.Show("1", none.ToString());

    //            //Remove();
    //            //Data data = new Data();
    //            //FaceRecorder.Instance(doc, data).Recorder();
    //            //double d = FaceRecorder.Instance(doc, data).Extract("a1");
    //            //bool d2 = FaceRecorder.Instance(doc, data).Extract("a2");
    //            //TaskDialog.Show("1", d2.ToString());

    //            trans.Commit();
    //        }


    //        return Result.Succeeded;
    //    }

    //    public void Remove()
    //    {
    //        List<DataStorage> dss = new FilteredElementCollector(doc).OfClass(typeof(DataStorage)).Cast<DataStorage>()
    //            .ToList();
    //        foreach (DataStorage item in dss)
    //        {
    //            doc.Delete(item.Id);
    //        }

    //        Schema s = Schema.Lookup(new Guid("d07f0dc5-b028-45c0-b5e7-9583353315d6"));
    //        Schema.EraseSchemaAndAllEntities(s, true);
    //    }

    //}

    //public class VM : BaseVM
    //{
    //    /// <summary>
    //    /// 姓名
    //    /// </summary>
    //    [Record(true)]
    //    public string Name { get; set; }

    //    /// <summary>
    //    /// 年龄
    //    /// </summary>
    //    [Record(true)]
    //    public int Age { get; set; }

    //    [Record(true)] public double D { get; set; }
    //    [Record(true)] public float F { get; set; }
    //    [Record(true)] public bool B { get; set; }


    //    //[Record(true)]
    //    //public Sex Sex { get; set; }

    //    public string None { get; set; }

    //    public VM(string name, int age /*, Sex sex*/)
    //    {
    //        this.Name = name;
    //        this.Age = age;
    //        //this.Sex = sex;
    //    }
    //}
}