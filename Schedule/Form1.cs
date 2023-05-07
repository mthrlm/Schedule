using Microsoft.Office.Interop.Excel;
using Schedule.Entities;
using Schedule.Interfaces;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using Excel = Microsoft.Office.Interop.Excel;
using ClosedXML.Excel;
using DataTable = System.Data.DataTable;
using DocumentFormat.OpenXml.Wordprocessing;

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
                table[i].Columns.Add("�", typeof(string));
                table[i].Columns.Add("����� ������", typeof(string));
                table[i].Columns.Add("�������", typeof(string));
                table[i].Columns.Add("�������", typeof(string));
                table[i].Columns.Add("���� � ������", typeof(string));
                data[i].DataSource = table[i];

                DataColumn[] primaryKeyTable = new DataColumn[1];
                primaryKeyTable[0] = table[i].Columns["�"];
                table[i].PrimaryKey = primaryKeyTable;
            }
            dataGridView1.DataSource = resultTable;

            resultTable.Columns.Add("�", typeof(string));
            resultTable.Columns.Add("�������������", typeof(string));
            resultTable.Columns.Add("��� ���", typeof(string));
            resultTable.Columns.Add("�����", typeof(string));
            resultTable.Columns.Add("���", typeof(string));

            DataColumn[] primaryKeyResult = new DataColumn[1];
            primaryKeyResult[0] = resultTable.Columns["�"];
            resultTable.PrimaryKey = primaryKeyResult;
            resultTable.DefaultView.Sort = "�������������";

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
                comboBox2.Items.Clear();
                foreach (var value in args as List<Teacher>)
                    comboBox2.Items.Add(value.ToString(true));
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
                if (row["�������������"] == comboBox2.SelectedItem)
                {
                    if (row["�����"] == comboBox1.SelectedItem.ToString())
                    {
                        row["��� ���"] = int.Parse(row["��� ���"].ToString()) + int.Parse(textBox1.Text);
                        row[className] = textBox1.Text;
                        row["���"] = int.Parse(row["���"].ToString()) + int.Parse(textBox1.Text);
                        break;
                    }
                    else if (resultTable.Rows.Count - 1 == resultTable.Rows.IndexOf(row))
                    {
                        row["��� ���"] = int.Parse(row["��� ���"].ToString()) + int.Parse(textBox1.Text);
                        newRow["�"] = resultTable.Rows.Count + 1;
                        newRow["�������������"] = row["�������������"];
                        newRow["��� ���"] = row["��� ���"];
                        newRow["���"] = textBox1.Text;
                        newRow["�����"] = comboBox1.SelectedItem;
                        newRow[className] = textBox1.Text;
                        resultTable.Rows.Add(newRow);
                        break;
                    }
                }
                else if (resultTable.Rows.Count - 1 == resultTable.Rows.IndexOf(row))
                {
                    newRow["�"] = resultTable.Rows.Count + 1;
                    newRow["�������������"] = comboBox2.SelectedItem;
                    newRow["��� ���"] = textBox1.Text;
                    newRow["���"] = textBox1.Text;
                    newRow["�����"] = comboBox1.SelectedItem;
                    newRow[className] = textBox1.Text;
                    resultTable.Rows.Add(newRow);
                    break;
                }
            }

            if (resultTable.Rows.Count == 0)
            {
                newRow["�"] = 1;
                newRow["�������������"] = comboBox2.SelectedItem;
                newRow["��� ���"] = textBox1.Text;
                newRow["���"] = textBox1.Text;
                newRow["�����"] = comboBox1.SelectedItem;
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
                MessageBox.Show("��������� ��������� ������");
                return false;
            }

            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("��������� ��������� ������");
                return false;
            }

            foreach (var c in textBox1.Text)
            {
                if (!Char.IsDigit(c))
                {
                    MessageBox.Show("��������� ��������� ������");
                    return false;
                }
            }

            if (int.Parse(textBox1.Text) < minLength || int.Parse(textBox1.Text) > maxLength)
            {
                MessageBox.Show("��������� ��������� ������");
                return false;
            }

            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("��������� ��������� ������");
                return false;
            }

            int index = tabControl2.SelectedIndex;
            textBox2.Text = textBox2.Text.ToUpper();
            string className = (index + 5).ToString() + textBox2.Text;

            foreach (DataRow row in table[index].Rows)
            {
                if (row["����� ������"].Equals(className) && row["�������"].Equals(comboBox2.SelectedItem.ToString()) && row["�������"].Equals(comboBox1.SelectedItem.ToString()))
                {
                    MessageBox.Show("������� �������� ������������ ������, ��������� ��������� ������.");
                    return false;
                }
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
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
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dataGridView1.Columns[i].Name;
                }

                for (int i = 0; i < resultTable.Rows.Count; i++)
                {
                    for (int j = 0; j < resultTable.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = resultTable.Rows[i][j].ToString();

                        if (worksheet.Cell(i + 2, j + 1).Value.ToString().Length > 0)
                        {
                            XLAlignmentHorizontalValues align;

                            switch (dataGridView1.Rows[i].Cells[j].Style.Alignment)
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

                            XLColor xlColor = XLColor.FromColor(dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor);
                            worksheet.Cell(i + 2, j + 1).AddConditionalFormat().WhenLessThan(1).Fill.SetBackgroundColor(xlColor);

                            worksheet.Cell(i + 2, j + 1).Style.Font.FontName = dataGridView1.Font.Name;
                            worksheet.Cell(i + 2, j + 1).Style.Font.FontSize = dataGridView1.Font.Size;

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
    }
}