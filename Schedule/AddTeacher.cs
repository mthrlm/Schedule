using Schedule.Entities;
using Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Schedule
{
    public partial class AddTeacher : Form, IView
    {

        public AddTeacher()
        {
            InitializeComponent();
            Presenter.OnShow += ShowForm;
            Presenter.OnExit += HideForm;
        }

        public event EventHandler<object>? OnClose;
        public event EventHandler<object>? OnAdd;
        public event EventHandler<object>? OnDelete;

        public void ShowForm(object sender)
        {
            if (sender.Equals(this))
                this.ShowDialog();
        }

        public void HideForm(object sender)
        {
            if (sender.Equals(this))
                this.Close();
        }

        public void Update(object view, object args, string type)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>() { nameText.Text, surnameText.Text, middleNameText.Text };
            OnAdd?.Invoke(this, args);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HideForm(this);
        }
    }
}
