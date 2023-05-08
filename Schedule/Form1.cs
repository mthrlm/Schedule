using Microsoft.Office.Interop.Excel;
using Schedule.Entities;
using Schedule.Interfaces;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;
using DataTable = System.Data.DataTable;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Numerics;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;

namespace Schedule
{
    public partial class Form1 : Form, IView
    {
        public event EventHandler<object> OnAdd;
        public event EventHandler<object> OnDelete;
        public event EventHandler<object> OnClose;

        private List<DataGridView> data = new List<DataGridView>();
        List<DataTable> table = new List<DataTable>();
        DataTable resultTable = new DataTable();
        DataTable scheduleTable = new DataTable();
        List<Teacher> teachers = new List<Teacher>();
        List<Subject> subjects = new List<Subject>();
        List<SubjectToTeach> subjectsToTeach = new List<SubjectToTeach>();

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

            for (int i = 0; i < data.Count; i++)
            {
                table.Add(new DataTable());
                table[i].Columns.Add("№", typeof(string));
                table[i].Columns.Add("Буква класса", typeof(string));
                table[i].Columns.Add("Предмет", typeof(string));
                table[i].Columns.Add("Учитель", typeof(string));
                table[i].Columns.Add("Часы в неделю", typeof(string));
                data[i].DataSource = table[i];

                DataColumn[] primaryKeyTable = new DataColumn[1];
                primaryKeyTable[0] = table[i].Columns["№"];
                table[i].PrimaryKey = primaryKeyTable;
            }
            dataGridView1.DataSource = resultTable;
            dataGridView2.DataSource = scheduleTable;

            resultTable.Columns.Add("№", typeof(string));
            resultTable.Columns.Add("Преподаватель", typeof(string));
            resultTable.Columns.Add("Общ Нгр", typeof(string));
            resultTable.Columns.Add("Предм", typeof(string));
            resultTable.Columns.Add("Нгр", typeof(string));

            DataColumn[] primaryKeyResult = new DataColumn[1];
            primaryKeyResult[0] = resultTable.Columns["№"];
            resultTable.PrimaryKey = primaryKeyResult;
            resultTable.DefaultView.Sort = "Преподаватель";

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
            if (sender.Equals(this))
                this.Show();
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
                teachers = args as List<Teacher>;
                comboBox2.Items.Clear();
                foreach (var value in args as List<Teacher>)
                    comboBox2.Items.Add(value.ToString(true));
            }

            if (type == "Subject")
            {
                subjects = args as List<Subject>;
                comboBox1.Items.Clear();
                foreach (var value in args as List<Subject>)
                    comboBox1.Items.Add(value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new EditSubjects_Presenter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddTeachers_Presenter();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!ErrorCheck()) return;

            int index = tabControl2.SelectedIndex;
            textBox2.Text = textBox2.Text.ToUpper();
            string className = (index + 5).ToString() + textBox2.Text;

            AddColumn(className, index);
            AddRow(className);
            Clear();
        }

        private void AddColumn(string className, int index)
        {
            table[index].Rows.Add(table[index].Rows.Count + 1, className, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), textBox1.Text);
            if (!resultTable.Columns.Contains(className))
            {
                DataColumn column = new DataColumn(className, typeof(string));
                resultTable.Columns.Add(column);
                List<string> columnNames = new List<string>();
                for (int i = 4; i < resultTable.Columns.Count; i++)
                {
                    columnNames.Add(resultTable.Columns[i].ColumnName);
                }
                resultTable.SetColumnsOrder(columnNames.ToArray());
            }
            resultTable.AcceptChanges();
        }

        private void AddRow(string className)
        {
            DataRow newRow = resultTable.NewRow();

            foreach (DataRow row in resultTable.Rows)
            {
                if (row["Преподаватель"] == comboBox2.SelectedItem)
                {
                    if (row["Предм"] == comboBox1.SelectedItem.ToString())
                    {
                        row["Общ Нгр"] = int.Parse(row["Общ Нгр"].ToString()) + int.Parse(textBox1.Text);
                        row[className] = textBox1.Text;
                        row["Нгр"] = int.Parse(row["Нгр"].ToString()) + int.Parse(textBox1.Text);
                        break;
                    }
                    else if (resultTable.Rows.Count - 1 == resultTable.Rows.IndexOf(row))
                    {
                        row["Общ Нгр"] = int.Parse(row["Общ Нгр"].ToString()) + int.Parse(textBox1.Text);
                        newRow["№"] = resultTable.Rows.Count + 1;
                        newRow["Преподаватель"] = row["Преподаватель"];
                        newRow["Общ Нгр"] = row["Общ Нгр"];
                        newRow["Нгр"] = textBox1.Text;
                        newRow["Предм"] = comboBox1.SelectedItem;
                        newRow[className] = textBox1.Text;
                        resultTable.Rows.Add(newRow);
                        break;
                    }
                }
                else if (resultTable.Rows.Count - 1 == resultTable.Rows.IndexOf(row))
                {
                    newRow["№"] = resultTable.Rows.Count + 1;
                    newRow["Преподаватель"] = comboBox2.SelectedItem;
                    newRow["Общ Нгр"] = textBox1.Text;
                    newRow["Нгр"] = textBox1.Text;
                    newRow["Предм"] = comboBox1.SelectedItem;
                    newRow[className] = textBox1.Text;
                    resultTable.Rows.Add(newRow);
                    break;
                }
            }

            if (resultTable.Rows.Count == 0)
            {
                newRow["№"] = 1;
                newRow["Преподаватель"] = comboBox2.SelectedItem;
                newRow["Общ Нгр"] = textBox1.Text;
                newRow["Нгр"] = textBox1.Text;
                newRow["Предм"] = comboBox1.SelectedItem;
                newRow[className] = textBox1.Text;
                resultTable.Rows.Add(newRow);
            }
            resultTable.AcceptChanges();
        }

        private void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private bool ErrorCheck()
        {
            int minLength = 1;
            int maxLength = 8;

            if (textBox2.Text.Length > 1 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Проверьте введенные данные");
                return false;
            }

            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Проверьте введенные данные");
                return false;
            }

            foreach (var c in textBox1.Text)
            {
                if (!Char.IsDigit(c))
                {
                    MessageBox.Show("Проверьте введенные данные");
                    return false;
                }
            }

            if (int.Parse(textBox1.Text) < minLength || int.Parse(textBox1.Text) > maxLength)
            {
                MessageBox.Show("Проверьте введенные данные");
                return false;
            }

            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Проверьте введенные данные");
                return false;
            }

            int index = tabControl2.SelectedIndex;
            textBox2.Text = textBox2.Text.ToUpper();
            string className = (index + 5).ToString() + textBox2.Text;

            foreach (DataRow row in table[index].Rows)
            {
                if (row["Буква класса"].Equals(className) && row["Учитель"].Equals(comboBox2.SelectedItem.ToString()) && row["Предмет"].Equals(comboBox1.SelectedItem.ToString()))
                {
                    MessageBox.Show("Попытка добавить существующую запись, проверьте введенные данные.");
                    return false;
                }
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFile(resultTable, dataGridView1);
        }

        void SaveFile(DataTable dt, DataGridView dgv)
        {
            string fileName;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.Title = "To Excel";
            saveFileDialog1.FileName = this.Text + " (" + DateTime.Now.ToString("yyyy-MM-dd") + ")";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(this.Text);
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dgv.Columns[i].Name;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = dt.Rows[i][j].ToString();

                        if (worksheet.Cell(i + 2, j + 1).Value.ToString().Length > 0)
                        {
                            XLAlignmentHorizontalValues align;

                            switch (dgv.Rows[i].Cells[j].Style.Alignment)
                            {
                                case DataGridViewContentAlignment.BottomRight:
                                    align = XLAlignmentHorizontalValues.Right;
                                    break;
                                case DataGridViewContentAlignment.MiddleRight:
                                    align = XLAlignmentHorizontalValues.Right;
                                    break;
                                case DataGridViewContentAlignment.TopRight:
                                    align = XLAlignmentHorizontalValues.Right;
                                    break;

                                case DataGridViewContentAlignment.BottomCenter:
                                    align = XLAlignmentHorizontalValues.Center;
                                    break;
                                case DataGridViewContentAlignment.MiddleCenter:
                                    align = XLAlignmentHorizontalValues.Center;
                                    break;
                                case DataGridViewContentAlignment.TopCenter:
                                    align = XLAlignmentHorizontalValues.Center;
                                    break;

                                default:
                                    align = XLAlignmentHorizontalValues.Left;
                                    break;
                            }

                            worksheet.Cell(i + 2, j + 1).Style.Alignment.Horizontal = align;

                            XLColor xlColor = XLColor.FromColor(dgv.Rows[i].Cells[j].Style.SelectionBackColor);
                            worksheet.Cell(i + 2, j + 1).AddConditionalFormat().WhenLessThan(1).Fill.SetBackgroundColor(xlColor);

                            worksheet.Cell(i + 2, j + 1).Style.Font.FontName = dgv.Font.Name;
                            worksheet.Cell(i + 2, j + 1).Style.Font.FontSize = dgv.Font.Size;

                        }
                    }
                }
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(fileName);
                MessageBox.Show("Done");
            }
        }

        bool IsTheSameCellValue(int column, int row)
        {
            if (column > 2 || column < 1) return false;
            DataGridViewCell cell1 = dataGridView1[column, row];
            DataGridViewCell cell2 = dataGridView1[column, row - 1];

            DataGridViewCell teacherCell1 = dataGridView1[1, row];
            DataGridViewCell teacherCell2 = dataGridView1[1, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }
            return cell1.Value.ToString() == cell2.Value.ToString() && teacherCell1.Value.ToString() == teacherCell2.Value.ToString();
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;
            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = dataGridView1.AdvancedCellBorderStyle.Top;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)
                return;
            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                e.Value = "";
                e.FormattingApplied = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Algorithm.Plan res = GetPlan();

            scheduleTable.Columns.Add(new DataColumn("Урок", typeof(string)));
            for (int i = 4; i < resultTable.Columns.Count - 1; i++)
            {
                DataColumn column = new DataColumn(dataGridView1.Columns[i].Name, typeof(string));
                scheduleTable.Columns.Add(column);
            }
            scheduleTable.AcceptChanges();

            for (int i = 0; i < Algorithm.Plan.DaysPerWeek; i++)
            {
                DataRow row = scheduleTable.NewRow();
                for (int j = 0; j < Algorithm.Plan.HoursPerDay; j++)
                {
                    List<Algorithm.Lessоn> byHour = res.GetLessonsOfDay((byte)i).Where(x => x.Hour == j).ToList();
                    foreach (Algorithm.Lessоn lesson in byHour)
                    {
                        SubjectToTeach subject = subjectsToTeach.Find(x => x.Id == lesson.Teacher);
                        row["Урок"] = (lesson.Hour + 1).ToString();
                        row[dataGridView1.Columns[lesson.Group].Name] = subject.Name + " " + subject.SubjectTeacher.ToString();

                    }
                    scheduleTable.Rows.Add(row);
                    row = scheduleTable.NewRow();
                    scheduleTable.AcceptChanges();
                }
            }
            SaveFile(scheduleTable, dataGridView2);
        }

        Algorithm.Plan GetPlan()
        {
            List<int> groups;
            List<int> teach;

            GetVars(out groups, out teach);

            var list = new List<Algorithm.Lessоn>();
            for (int i = 0; i < groups.Count; i++)
                list.Add(new Algorithm.Lessоn(groups[i], teach[i]));

            var solver = new Algorithm.Solver();//создаем решатель

            Algorithm.Plan.DaysPerWeek = 6;//устанавливаем только два учебных дна - это нужно лишь для данной тестовой задачи, в реальности - выставьте нужное число учебных дней!
            Algorithm.Plan.HoursPerDay = 8;

            solver.FitnessFunctions.Add(Algorithm.FitnessFunctions.Windows);//будем штрафовать за окна
            solver.FitnessFunctions.Add(Algorithm.FitnessFunctions.LateLesson);//будем штрафовать за поздние пары

            return solver.Solve(list);//находим лучший план
        }

        void GetVars(out List<int> groups, out List<int> teach)
        {
            groups = new List<int>();
            teach = new List<int>();

            for (int row = 0; row < resultTable.Rows.Count; row++)
            {
                for (int col = 4; col < resultTable.Columns.Count - 1; col++)
                {
                    try
                    {
                        var index = int.Parse(resultTable.Rows[row][col].ToString());
                    }
                    catch (ArgumentNullException ex)
                    {
                        continue;
                    }
                    catch (FormatException ex)
                    {
                        continue;
                    }

                    for (int hourCount = 0; hourCount < int.Parse(resultTable.Rows[row][col].ToString()); hourCount++)
                    {
                        groups.Add(col);
                        string name = subjects.Find(x => x.ToString().Equals(resultTable.Rows[row]["Предм"].ToString())).ToString();
                        Teacher teacher = teachers.Find(x => x.ToString() == resultTable.Rows[row]["Преподаватель"].ToString());

                        if (subjectsToTeach.Count > 0 && subjectsToTeach.Exists(x => x.Name.Equals(name) && x.SubjectTeacher.Id == teacher.Id))
                        {
                            int id = subjectsToTeach.Where(x => x.Name == name && x.SubjectTeacher.Id == teacher.Id).ToList()[0].Id;
                            teach.Add(id);
                        }
                        else
                        {
                            SubjectToTeach subject = new SubjectToTeach(name, teacher);
                            subjectsToTeach.Add(subject);
                            teach.Add(subject.Id);
                        }
                    }
                }
            }
        }
    }
}