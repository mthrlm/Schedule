using Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public class Form1_Presenter : Presenter
    {
        private new IView _view;
        public Form1_Presenter(IView view) : base()
        {
            _view = view;
            _view.OnClose += Exit;
            _view.OnAdd += Save;

            Model.OnUpdate += Update;
 
            Update();
        }

        public override void Delete(object sender, object args)
        {
            throw new NotImplementedException();
        }

        public override void Exit(object sender, object e)
        {
            _model.WriteData();
            OnExit?.Invoke(_view);
        }

        public override void Save(object sender, object args)
        {
            throw new NotImplementedException();
        }

        public override void ShowDeleteMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void ShowErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void ShowSaveMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            OnUpdate?.Invoke(_view, _model.Teachers, "Teacher");
            OnUpdate?.Invoke(_view, _model.Subjects, "Subject");
        }
    }
}
