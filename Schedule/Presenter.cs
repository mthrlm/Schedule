using Schedule.Entities;
using Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public abstract class Presenter
    {
        public static Action<object> OnSave { get; set; }
        public static Action<object> OnDelete { get; set; }
        public static Action<object, object, string> OnUpdate { get; set; }
        public static Action<object> OnExit { get; set; }
        public static Action<object> OnShow { get; set; }

        protected static IModel _model = new Model();
        protected IView _view;

        public abstract void Delete(object sender, object args);
        public abstract void Update();
        public abstract void Hide();
        public abstract void Save(object sender, object args);
        public abstract void Exit();
        public abstract void ShowSaveMessage(string message);
        public abstract void ShowErrorMessage(string message);
        public abstract void ShowDeleteMessage(string message);
    }
}
