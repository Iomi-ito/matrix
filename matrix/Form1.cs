using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.LinkLabel;


namespace matrix
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //checkedListBox1.ItemCheck += сheckedListBox_ItemCheck;
            //checkedListBox2.ItemCheck += сheckedListBox_ItemCheck;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;

        }
        int maxLengthCol = 0; // Переменная для хранения количества столбцов
        int maxLengthRow = 0; // Переменная для хранения количества строк
        int[] checkListBox1Value = new int[10];
        double[][] matrix;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                // Чтение содержимого файла
                string[] lines = File.ReadAllLines(filePath);
                bool isMatrix = true;
                matrix = new double[lines.Length][];
                int expectedLength = -1; // Ожидаемая длина строки
                maxLengthCol = lines.Length;

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] numbers = lines[i].Split(' ');
                    if (numbers.Length > maxLengthCol) // Проверяем, является ли текущая строка самой длинной
                    {
                        maxLengthRow = numbers.Length; // Обновляем значение максимальной длины
                    }

                    if (expectedLength == -1)
                    {
                        expectedLength = numbers.Length;
                    }
                    if (numbers.Length != expectedLength)
                    {
                        textBox1.Text = "Ошибка: строки матрицы имеют различные длины. Выберите другой файл или измените этот";
                      
                        return;
                    }
                    else
                    {
                        matrix[i] = new double[numbers.Length];
                        for (int j = 0; j < numbers.Length; j++)
                        {
                            if (!double.TryParse(numbers[j], out matrix[i][j]))
                            {
                                textBox1.Text = "Ошибка при чтении числа в строке " + (i + 1) + ", позиция " + (j + 1);
                                isMatrix = false;
                                
                                return;
                            }
                        }
                        if (isMatrix)
                        {
                            textBox1.Text = "Матрица корректна. Выберите желаемые варианты расчета";
                            checkedListBox1.Enabled = true;
                            checkedListBox2.Enabled = true;
                           
                        }
                    }

                }
            }
        }
       
    /*private void сheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                if ((checkedListBox1.CheckedItems.Count > 0) || (checkedListBox2.CheckedItems.Count > 0))
                {
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                }
            });
        }*/
     
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;
            int checkedIndex = e.Index;

            if (e.NewValue == CheckState.Checked) // Проверяем, что галочка устанавливается, а не снимается
            {

                // Создаем новое диалоговое окно для ввода числа
                Form inputForm = new Form();
                inputForm.Location = new Point(10, 10);
                inputForm.Size = new Size(120, 150);

                inputForm.Text = "Введите число";

                Label label = new Label();
                label.Text = "Введите число:";
                label.Location = new Point(10, 10);
                inputForm.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Location = new Point(10, 30);
                inputForm.Controls.Add(textBox);

                Button okButton = new Button();
                okButton.Text = "OK";
                okButton.Location = new Point(10, 60);
                okButton.Click += (s, ev) =>
                {
                    int number;
                    if (int.TryParse(textBox.Text, out number))
                    {
                        if ((selectedIndex + 1) % 2 == 0)
                        {
                            if (number <= maxLengthRow && number >= 1)
                            {
                                // Сохраняем число и закрываем окно
                                MessageBox.Show("Число " + number + " сохранено!");
                                checkListBox1Value[selectedIndex] = number;
                                inputForm.Close();
                            }
                            else
                            {
                                MessageBox.Show("Пожалуйста, введите номер столбца, по которому производить расчет (подсчет начинается с 1)!");
                            }
                        }
                        else
                        {
                            if (number <= maxLengthCol && number >= 1)
                            {
                                // Сохраняем число и закрываем окно
                                MessageBox.Show("Число " + number + " сохранено!");
                                checkListBox1Value[selectedIndex] = number;
                                inputForm.Close();
                            }
                            else
                            {
                                MessageBox.Show("Пожалуйста, введите номер строки, по которому производить расчет (подсчет начинается с 1)!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите корректное число!");
                    }
                };

                inputForm.Controls.Add(okButton);
                inputForm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String result = ""; // Результаты
            result = Calculate_checkedListBox1();
            result += Calculate_checkedListBox2();
            // Проверяем, какие элементы выбраны в CheckListBox1 и CheckListBox2


            textBox1.Text = result;
        }

        private String Calculate_checkedListBox1()
        {
            String result = "";
            double value=0;
            Console.WriteLine(maxLengthCol);
            Console.WriteLine(maxLengthRow);
            if (checkedListBox1.GetItemChecked(0))
            {
                // Нахождение среднего значения по строчке
                for (int i = 0;i < maxLengthRow;i++) 
                {
                    value += matrix[checkListBox1Value[0] - 1][i];
                }
                result += "Среднее значение по " + checkListBox1Value[0] + " строке - " + (value / maxLengthRow)+ Environment.NewLine;
            
            }
            if (checkedListBox1.GetItemChecked(1))
            {
                // Нахождение среднего значения по столбцу
                value = 0;
                for (int i = 0; i < maxLengthCol; i++)
                {
                    value += matrix[i][checkListBox1Value[1] - 1];
                }
                result += "Среднее значение по " + checkListBox1Value[1] + " столбцу - " + (value / maxLengthCol) + Environment.NewLine;
            }
            if (checkedListBox1.GetItemChecked(2))
            {
                // Нахождение суммы элементов в строчке
                value = 0;
                for (int i = 0; i < maxLengthRow; i++)
                {
                    value += matrix[checkListBox1Value[2] - 1][i];
                }
                result += "Сумма элементов по " + checkListBox1Value[2] + " строке - " + value + Environment.NewLine;

            }
            if (checkedListBox1.GetItemChecked(3))
            {
                // Нахождение суммы элементов в столбце
                value = 0;
                for (int i = 0; i < maxLengthCol; i++)
                {
                    value += matrix[i][checkListBox1Value[3] - 1];
                    Console.WriteLine(value);
                }
                result += "Сумма элементов по " + checkListBox1Value[3] + " столбцу - " + value + Environment.NewLine;
            }
            if (checkedListBox1.GetItemChecked(4))
            {
                // Нахождение произведения элементов в строчке
                value = 1;
                for (int i = 0; i < maxLengthRow; i++)
                {
                    value *= matrix[checkListBox1Value[4] - 1][i];
                }
                result += "Произведение элементов по " + checkListBox1Value[4] + " строке - " + value + Environment.NewLine;
            }
            if (checkedListBox1.GetItemChecked(5))
            {
                // Нахождение произведения элементов в столбце
                value = 1;
                for (int i = 0; i < maxLengthCol; i++)
                {
                    value *= matrix[i][checkListBox1Value[5] - 1];
                    Console.WriteLine(value);
                }
                result += "Произведение элементов по " + checkListBox1Value[5] + " столбцу - " + value + Environment.NewLine;
            }
            if (checkedListBox1.GetItemChecked(6))
            {
                // Нахождение минимального элемента в строчке
                double min = matrix[checkListBox1Value[6] - 1][0];
                for (int i = 0; i < maxLengthRow; i++)
                {
                    if (matrix[checkListBox1Value[6] - 1][i]<min)
                    {
                        min = matrix[checkListBox1Value[6] - 1][i];
                    }
                   
                }
                result += "Минимальный элемент в " + checkListBox1Value[6] + " строке - " + min + Environment.NewLine;

            }
            if (checkedListBox1.GetItemChecked(7))
            {
                // Нахождение минимального элемента в столбце
                double min = matrix[0][checkListBox1Value[7] - 1];
                for (int i = 0; i < maxLengthCol; i++)
                {
                    if (matrix[i][checkListBox1Value[7] - 1] < min)
                    {
                        min = matrix[i][checkListBox1Value[7] - 1];
                    }

                }
                result += "Минимальный элемент в " + checkListBox1Value[7] + " столбце - " + min + Environment.NewLine;
            }
            if (checkedListBox1.GetItemChecked(8))
            {
                // Нахождение максимального элемента в строчке
                double max = matrix[checkListBox1Value[8] - 1][0];
                for (int i = 0; i < maxLengthRow; i++)
                {
                    if (matrix[checkListBox1Value[8] - 1][i] > max)
                    {
                        max = matrix[checkListBox1Value[8] - 1][i];
                        
                    }
                }
                result += "Максимальный элемент в " + checkListBox1Value[8] + " строке - " + max + Environment.NewLine;
            }
            if (checkedListBox1.GetItemChecked(9))
            {
                // Нахождение максимального элемента в столбце
                double max = matrix[0][checkListBox1Value[9] - 1];
                for (int i = 0; i < maxLengthCol; i++)
                {
                    if (matrix[i][checkListBox1Value[9] - 1] > max)
                    {
                        max = matrix[i][checkListBox1Value[9] - 1];
                    }

                }
                result += "Максимальный элемент в " + checkListBox1Value[9] + " столбце - " + max + Environment.NewLine;
            }
            
            return result;
        }



        private String Calculate_checkedListBox2()
        {
            String result="";
            double value = 0;
            if (checkedListBox2.GetItemChecked(0))
            {
                // Нахождение среднего значения в матрице
                result += "Среднее значение элементов в матрице - " + (sumMatrix()/(maxLengthCol*maxLengthRow)) + Environment.NewLine;

            }
            if (checkedListBox2.GetItemChecked(1))
            {
                // Сумма элементов в матрице
                result += "Сумма элементов в матрице - " + sumMatrix() + Environment.NewLine;
            }
            if (checkedListBox2.GetItemChecked(2))
            {
                //  Произведение элементов в матрице
                result += "Произведение элементов в матрице - " + multiplicationMatrix() + Environment.NewLine;
            }
            if (checkedListBox2.GetItemChecked(3))
            {
                //Нахождение миним значения в матрице
                result += "Минимальное значение в матрице - " + minimMatrix() + Environment.NewLine;
            }
            if (checkedListBox2.GetItemChecked(4))
            {
                // Нахождение макс значения в матрице
                result += "Максимальное значение в матрице - " + maximMatrix() + Environment.NewLine;
            }

            return result;
        }
        private double sumMatrix()
        {
            double result = 0;
            for (int i = 0; i<maxLengthCol; i++)
            {
                for (int j = 0; j < maxLengthRow; j++)
                {
                    result += matrix[i][j];
                }
            }
            return result;
        }
        private double multiplicationMatrix()
        {
            double result = 1;
            if (minimMatrix()==0) { return 0; }
            for (int i = 0; i < maxLengthCol; i++)
            {
                for (int j = 0; j < maxLengthRow; j++)
                {
                    result *= matrix[i][j];
                }
            }
            return result;
        }
        private double minimMatrix()
        {
            double result = matrix[0][0];
            for (int i = 0; i < maxLengthCol; i++)
            {
                for (int j = 0; j < maxLengthRow; j++)
                {
                    if (matrix[i][j] < result)
                    {
                        result = matrix[i][j];
                    }
                }
            }
            return result;
        }
        private double maximMatrix()
        {
            double result = matrix[0][0];
            for (int i = 0; i < maxLengthCol; i++)
            {
                for (int j = 0; j < maxLengthRow; j++)
                {
                    if (matrix[i][j] > result)
                    {
                        result = matrix[i][j];
                    }
                }
            }
            return result;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            // Создаем новую форму для отображения справки
            HelpForm helpForm = new HelpForm();
            // Показываем новую форму
            helpForm.Show();

        }
    }
    
}
