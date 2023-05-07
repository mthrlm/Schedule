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

namespace Schedule
{
    public partial class EditSubject : Form, IView
    {
        public event EventHandler<object> OnAdd; 
        public event EventHandler<object> OnDelete; 
        public event EventHandler<object> OnClose; 
        public EditSubject()
        {
            InitializeComponent();
            Presenter.OnShow += ShowForm;
            Presenter.OnExit += HideForm;
        }

        public void ShowDeleteMessage(string message)
        {
            throw new NotImplementedException();
        }

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

        public void ShowSaveMessage(string message)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnAdd?.Invoke(this, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HideForm(this);
        }


        public void Update(object view, object args, string type)
        {
            throw new NotImplementedException();
        }
    }
}
