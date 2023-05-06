﻿using Schedule.Entities;
using Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public class AddTeachers_Presenter : Presenter
    {
        public AddTeachers_Presenter() : base()
        {
            _view = new AddTeachers();
            _view.OnDelete += Delete;

            Model.OnUpdate += Update;

            OnShow.Invoke(_view);
            Update();
        }

        public override void Delete(object sender, object args)
        {
            Teacher subject = args as Teacher;
            OnDelete?.Invoke(subject);
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
            throw new NotImplementedException();
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
            OnUpdate?.Invoke(_view, _model.Teachers, "Teacher");
        }
    }
}
