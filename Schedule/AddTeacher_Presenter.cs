using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    internal class AddTeacher_Presenter : Presenter
    {
        public AddTeacher_Presenter() : base()
        {
            _view = new AddTeacher();
            _view.OnAdd += Save;
            OnShow.Invoke(_view);
        }

        public override void Delete(object sender, object args)
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            OnExit?.Invoke(_view);
        }

        public override void Hide()
        {
            throw new NotImplementedException();
        }

        public override void Save(object sender, object args)
        {
            if (!sender.Equals(_view)) return;
            try
            {
                List<string> values = args as List<string>;

                if (values.FindAll(x => x.Length == 0).Count > 0) throw new ArgumentNullException("Поля не могут быть пустыми");
                Teacher teacher = new Teacher(values[0], values[1], values[2]);
                OnSave?.Invoke(teacher);
                Exit();
            }
            catch (ArgumentNullException e)
            {
                ShowErrorMessage("Ошибка при добавлении. " + e.Message);
            }
        }

        public override void ShowDeleteMessage(string message)
        {
            MessageBox.Show(message);
        }

        public override void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        public override void ShowSaveMessage(string message)
        {
            MessageBox.Show(message);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
