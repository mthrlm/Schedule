using Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public class EditSubject_Presenter : Presenter
    {
        public EditSubject_Presenter() : base() 
        {
            _view = new EditSubject();
            _view.OnAdd += Save;
            OnShow.Invoke(_view);
        }

        public override void Delete(object sender, object args)
        {
            throw new NotImplementedException();
        }

        public override void Exit(object sender, object args)
        {
            OnExit?.Invoke(sender);
        }

        public override void Save(object sender, object args)
        {
            if (!sender.Equals(_view)) return;

            try
            {
                if (args.ToString().Equals(string.Empty)) throw new ArgumentNullException("Поле не может быть пустым");

                string value = Utils.ToUpperFirstLetter(args.ToString());
                Subject subject = new Subject(value);

                OnSave?.Invoke(subject);
                Exit(_view, null);
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
