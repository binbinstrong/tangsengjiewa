using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitHelper
{
    /// <summary>
    /// 动态模型更新
    /// </summary>
    public class DMUHelper
    {
        private IUpdater updater;
        private Document doc;
        private Parameter parameter; // elementid id;
        private List<ElementId> elements;
        private AddInId addinid;
        private Guid guid;

        private bool registered=false;
        private bool triggerAdded=false;

        public DMUHelper()
        {
            //Updater = new ParameterUpdater(addinid, Guid.NewGuid());
        }
        public IUpdater Updater
        {
            get { return updater; }
            set { updater = value; }
        }
        public Document Doc
        {
            get { return doc; }
            set { doc = value; }
        }
        public List<ElementId> Elements
        {
            get { return elements; }
            set { elements = value; }
        }

        public Guid Guid1
        {
            get { return guid; }
            set
            {
                guid = value;
                updater = new ParameterUpdater(addinid,guid); //guid 变化是 生成新的 updater
            }
        }

        public Parameter Parameter1
        {
            get { return parameter; }
            set { parameter = value; }
        }
         
        public void Register()
        {
            if(updater!=null&&!UpdaterRegistry.IsUpdaterRegistered(Updater.GetUpdaterId(),doc))
            UpdaterRegistry.RegisterUpdater(Updater);
            registered = true;
        }
        public void AddTrigger()
        {
            UpdaterRegistry.AddTrigger(Updater.GetUpdaterId(), doc, elements,Element.GetChangeTypeParameter(Parameter1));

            triggerAdded = true;
        }
         
        public void StartMonitor()
        {
            if (!registered) Register();
            if(!triggerAdded) AddTrigger();
        }
        

    }

    public class ParameterUpdater : IUpdater
    {
        private UpdaterId _uid;
        
        public ParameterUpdater(AddInId adinid,Guid guid/*updater 的 id*/)
        {
            _uid = new UpdaterId(adinid, guid);
        }
        public void Execute(UpdaterData data)
        {
            Func<ICollection<ElementId>, string> toString = ids => ids.Aggregate("", (ss, id) => ss + "," + id).TrimStart(',');
            var sb = new StringBuilder();
            sb.AppendLine("added" + toString(data.GetAddedElementIds()));
            sb.AppendLine("modified:" + toString(data.GetModifiedElementIds()));
            sb.AppendLine("deleted" + toString(data.GetDeletedElementIds()));

            TaskDialog.Show("changes", sb.ToString());
        }

        public string GetAdditionalInformation()
        {
            return "监控某个参数修改";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FreeStandingComponents;
        }

        public UpdaterId GetUpdaterId()
        {
            return _uid;
        }

        public string GetUpdaterName()
        {
            return "ParameterUpdater";
        }
    }
    public class customUpdater : IUpdater
    {
        private UpdaterId _uid;

        Action<UpdaterData> action ;
        
        public customUpdater(AddInId adinid, Guid guid/*updater 的 id*/)
        {
            _uid = new UpdaterId(adinid, guid);
        }
        public void Execute(UpdaterData data)
        {
            action(data);
        }

        public string GetAdditionalInformation()
        {
            return "监控某个参数修改";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FreeStandingComponents;
        }

        public UpdaterId GetUpdaterId()
        {
            return _uid;
        }

        public string GetUpdaterName()
        {
            return "ParameterUpdater";
        }
    }
}
