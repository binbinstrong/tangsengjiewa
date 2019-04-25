using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using BinLibrary.WinHelper;

namespace BinLibrary
{
    /// <summary>
    /// 外部事件类 需要在revit主线程中初始化
    /// </summary>
    public class ExternalEventHelper:IDisposable
    {
        #region fields

        private static ExternalEventHelper Instance;

        public static ExternalEventHelper GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new ExternalEventHelper();
                }
                return Instance;
            }
        }

        private ExternalEventHandlerCommon externalEventHandlerCommon;
        private Autodesk.Revit.UI.ExternalEvent externalEvent;
        #endregion

        #region events

        public event EventHandler<ExternalEventArg> Started;
        public event EventHandler<ExternalEventArg> End;

        #endregion

        #region ctors
        public ExternalEventHelper( )
        {
            this.externalEventHandlerCommon = new ExternalEventHandlerCommon();
            this.externalEvent = Autodesk.Revit.UI.ExternalEvent.Create(this.externalEventHandlerCommon);
            this.externalEventHandlerCommon.Started += externalEventCommon_Started;
            this.externalEventHandlerCommon.End += externalEventCommon_End;
        }

        public ExternalEventHelper(UIApplication uiApp)
        {
            this.externalEventHandlerCommon = new ExternalEventHandlerCommon();
            this.externalEvent = Autodesk.Revit.UI.ExternalEvent.Create(this.externalEventHandlerCommon);
            this.externalEventHandlerCommon.Started += externalEventCommon_Started;
            this.externalEventHandlerCommon.End += externalEventCommon_End;
        }

        public ExternalEventHelper(UIControlledApplication uiControlApp)
        {
            this.externalEventHandlerCommon = new ExternalEventHandlerCommon();
            this.externalEvent = Autodesk.Revit.UI.ExternalEvent.Create(this.externalEventHandlerCommon);
            this.externalEventHandlerCommon.Started += externalEventCommon_Started;
            this.externalEventHandlerCommon.End += externalEventCommon_End;
        }
        #endregion

        #region methos

        public void Invoke(Action<UIApplication> action, string name = "")
        {
            var nf = string.IsNullOrWhiteSpace(name) ? Guid.NewGuid().ToString() : name;
            this.externalEventHandlerCommon.Actions.Enqueue(new KeyValuePair<string, Action<UIApplication>>(nf, action));
            this.externalEvent.Raise();
        }


        public void externalEventCommon_Started(object sender, ExternalEventArg e)
        {
            if (this.Started != null)
            {
                this.Started(this, e);
            }
        }

        private void externalEventCommon_End(object sender, ExternalEventArg e)
        {
            if (this.End != null)
            {
                this.End(this, e);
            }
        }
        #endregion
        
        #region nestedClass

        class ExternalEventHandlerCommon : IExternalEventHandler
        {
            internal Queue<KeyValuePair<string, Action<UIApplication>>> Actions
            {
                get; set;
            }

            public event EventHandler<ExternalEventArg> Started;
            public event EventHandler<ExternalEventArg> End;

            internal ExternalEventHandlerCommon()
            {
                this.Actions = new Queue<KeyValuePair<string, Action<UIApplication>>>();
            }

            public void Execute(UIApplication app)
            {
                while (this.Actions.Count>0)
                {
                    var first = this.Actions.Dequeue();
                    if (string.IsNullOrWhiteSpace(first.Key)||first.Value==null)
                    {
                        continue;
                    }
                    try
                    {
                        if (this.Started!=null)
                        {
                            this.Started(this, new ExternalEventArg(app, first.Key));
                            first.Value(app);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.LogWrite(@"c:\ExternalEventhandlerLog.txt", e.ToString());

                    }
                    finally
                    {
                        this.End(this,new ExternalEventArg(app,first.Key));
                    } 
                }
            }

            public string GetName()
            {
                return "";
            }
        }
        #endregion

        public void Dispose()
        {
            externalEventHandlerCommon=null;
            externalEvent = null;
        }
    }
    /// <summary>
    /// 外部事件参数
    /// </summary>
    public class ExternalEventArg : EventArgs
    {
        public ExternalEventArg(UIApplication app, string name)
        {
            this.App = app;
            this.Name = name;
        }

        public UIApplication App
        {
            get; set;
        }
        public string Name { get; set; }
    }
}
