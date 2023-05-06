using Schedule.Entities;
using Schedule.Interfaces;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Forms;

namespace Schedule
{
    public partial class Form1 : Form, IView
    {
        public event EventHandler<object> OnAdd;
        public event EventHandler<object> OnDelete;
        public event EventHandler<object> OnClose;

        private List<DataGridView> data = new List<DataGridView>();
        List<DataTable> table = new List<DataTable>();

        public Form1()
        {
            InitializeComponent();
            data.Add(dataGridView5);
            data.Add(dataGridView6);
            data.Add(dataGridView7);
            data.Add(dataGridView8);
            data.Add(dataGridView9);
            data.Add(dataGridView10);
            data.Add(dataGridView11);

            for(int i = 0; i < data.Count; i++)
            {
                table.Add(new DataTable());
                table[i].Columns.Add("Буква класса", typeof(string));
                table[i].Columns.Add("Предмет", typeof(string));
                table[i].Columns.Add("Учитель", typeof(string));
                table[i].Columns.Add("Часы в неделю", typeof(string));
                data[i].DataSource = table[i];
            }

            Presenter.OnShow += ShowForm;
            Presenter.OnUpdate += Update;
            Form1_Presenter presenter = new Form1_Presenter(this);
        }

        public void ShowDeleteMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowForm(object sender)
        {
            this.Show();
        }

        public void HideForm(object sender)
        {
            throw new NotImplementedException();
        }

        public void ShowSaveMessage(string message)
        {
            throw new NotImplementedException();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClose?.Invoke(this, e);
        }

        public void Update(object view, object args, string type)
        {
            if (!view.Equals(this))
            if (args == null) return;

            if (type == "Teacher")
            {
                comboBox2.Items.Clear();
                foreach (var value in args as List<Teacher>)
                    comboBox2.Items.Add(value);
            }

            if (type == "Subject")
            {
                comboBox1.Items.Clear();
                foreach (var value in args as List<Subject>)
                    comboBox1.Items.Add(value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditSubjects_Presenter presenter = new EditSubjects_Presenter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTeachers_Presenter presenter = new AddTeachers_Presenter();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = tabControl2.SelectedIndex;
            //if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || comboBox1.SelectedText.Length == 0 || comboBox2.SelectedText.Length == 0)
            //    return;
            table[index].Rows.Add(textBox2.Text, comboBox1.SelectedItem, comboBox2.SelectedItem, textBox1.Text);
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
    }
}