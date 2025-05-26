using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace Inf_Sec
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Width = 700;
            this.Height = 700;
            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        // страница 1
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            openFileDialog.Title = "Открыть текстовый файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    textBox1.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inText = textBox1.Text;
            string keyText = textBox4.Text;
            try
            {
                string language = comboBox1.SelectedItem.ToString();
                if (language == "Русский")
                {
                    language = "ru";
                }
                else
                {
                    language = "en";
                }
                Binary binary = new Binary(language);

                if (!GetKeyVigrnere(keyText, language)) return;
                string key = keyText;

                if (!GetTextFromInput(inText, language)) return;
                var result = binary.Encrypt(inText, key);
                textBox3.Text = result.messageBinary;
                textBox5.Text = result.encryptedMessageBinary;
                textBox6.Text = result.encryptText;
                textBox2.Text = result.key;
                button2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при шифровании: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string outText = textBox6.Text;
            string keyText = textBox4.Text;
            try
            {
                string language = comboBox1.SelectedItem.ToString();
                if (language == "Русский")
                {
                    language = "ru";
                }
                else if (language == "Английский")
                {
                    language = "en";
                }
                Binary binary = new Binary(language);

                if (!GetKeyVigrnere(keyText, language)) return;
                string key = keyText;

                if (!GetTextFromOutput(outText, language)) return;
                string decryptedText = binary.Decrypt(outText, key);
                textBox6.Text = decryptedText;
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
        }

        private bool IsValidText(string text, string language)
        {
            string alphabet;
            if (language == "ru")
            {
                alphabet = Caesar.RuAlphabet;
            }
            else if (language == "en")
            {
                alphabet = Caesar.EngAlphabet;
            }
            else
            {
                return false;
            }

            foreach (char c in text.ToUpper())
            {
                if (!alphabet.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetTextFromInput(string inText, string language)
        {
            string text = inText;
            if (!IsValidText(text, language))
            {
                MessageBox.Show("Некорректный текст.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Введите текст для шифрования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool GetTextFromOutput(string outText, string language)
        {
            string text = outText;
            if (!IsValidText(text, language))
            {
                MessageBox.Show("Некорректный текст.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Введите текст для расшифрования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool GetKeyVigrnere(string inText, string language)
        {
            string text = inText;
            if (!IsValidText(text, language))
            {
                MessageBox.Show("Ключ не должен содержать пробелы.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Ключ не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // страница 2
        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            openFileDialog.Title = "Открыть текстовый файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    textBox12.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string inText = textBox12.Text;
            try
            {
                string language = comboBox1.SelectedItem.ToString();
                if (language == "Русский")
                {
                    language = "ru";
                }
                else if (language == "Английский")
                {
                    language = "en";
                }
                Binary binary = new Binary(language);

                if (!GetTextFromInput(inText, language)) return;
                var result = binary.EncryptR(inText);
                textBox10.Text = result.messageBinary;
                textBox8.Text = result.encryptedMessageBinary;
                textBox7.Text = result.encryptText;
                Console.WriteLine(result.key);
                textBox11.Text = result.key;
                button3.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при шифровании: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string outText = textBox7.Text;
            string keyText = textBox11.Text;
            try
            {
                string language = comboBox1.SelectedItem.ToString();
                if (language == "Русский")
                {
                    language = "ru";
                }
                else if (language == "Английский")
                {
                    language = "en";
                }
                Binary binary = new Binary(language);

                if (!GetKeyVigrnere(keyText, language)) return;
                string key = keyText;

                if (!GetTextFromOutput(outText, language)) return;
                string decryptedText = binary.DecryptR(outText, key);
                textBox7.Text = decryptedText;
                button3.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
        }
        // Страница 3
        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            openFileDialog.Title = "Открыть текстовый файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    textBox16.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string inKey = textBox17.Text;
            string inText = textBox16.Text;
            try
            {
                string language = comboBox1.SelectedItem.ToString();
                if (language == "Русский")
                {
                    language = "ru";
                }
                else if (language == "Английский")
                {
                    language = "en";
                }
                Binary binary = new Binary(language);

                if (!GetTextFromInput(inText, language)) return;
                var result = binary.EncryptCBC(inText, inKey);
                textBox14.Text = result.messageBinary;
                textBox13.Text = result.encryptedMessageBinary;
                textBox9.Text = result.encryptText;
                textBox15.Text = result.key;
                textBox18.Text = result.blockToEncrypt1;
                textBox19.Text = result.binaryKey;
                button6.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при шифровании: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string outText = textBox9.Text;
            string keyText = textBox17.Text;
            string gamma = textBox15.Text;
            try
            {
                string language = comboBox1.SelectedItem.ToString();
                if (language == "Русский")
                {
                    language = "ru";
                }
                else if (language == "Английский")
                {
                    language = "en";
                }
                Binary binary = new Binary(language);

                if (!GetKeyVigrnere(keyText, language)) return;
                string key = keyText;

                if (!GetTextFromOutput(outText, language)) return;
                string decryptedText = binary.DecryptCBC(outText, key, gamma);
                textBox9.Text = decryptedText;
                button6.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }
}
