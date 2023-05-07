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
    public partial class EditSubjects : Form, IView
    {
        public EditSubjects()
        {
            InitializeComponent();
            Presenter.OnShow += ShowForm;
            Presenter.OnExit += HideForm;
            Presenter.OnUpdate += Update;
        }

        public event EventHandler<object>? OnClose;
        public event EventHandler<object>? OnAdd;
        public event EventHandler<object>? OnDelete;

        public void ShowForm(object sender)
        {
            if (sender.Equals(this))
                this.Show();
        }

        public void HideForm(object sender)
        {
            if (sender.Equals(this))
                this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new EditSubject_Presenter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            OnDelete?.Invoke(this, listBox1.SelectedItem);
        }

        public void Update(object view, object args, string type)
        {
            if (!view.Equals(this)) return;
            if (args == null) return;

            listBox1.Items.Clear();

            foreach (var value in args as List<Subject>)
            {
                listBox1.Items.Add(value.Name);
            }
        }
    }
}
