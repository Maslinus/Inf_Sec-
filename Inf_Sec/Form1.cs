using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using System.IO;

namespace Inf_Sec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 550;
            this.Height = 520;
            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.SelectedIndex = 0;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Первая страница
        private void button1_Click(object sender, EventArgs e)
        {
            string inText = textBox1.Text;
            string keyText = textBox3.Text;
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
                if (comboBox4.SelectedIndex == 0)
                {
                    Caesar caesar = new Caesar(language);

                    BigInteger key = GetKeyFromInput(keyText);
                    if (key == BigInteger.Zero) return;

                    if (!GetTextFromInput(inText, language)) return;
                    string encryptedText = caesar.Encrypt(inText, key);
                    textBox2.Text = encryptedText;
                    button2.Enabled = true;
                    button1.Enabled = false;
                }else if(comboBox4.SelectedIndex == 1)
                {
                    Vigenere vigenere = new Vigenere(language);

                    if (!GetKeyVigrnere(keyText, language)) return;
                    string key = keyText;

                    if (!GetTextFromInput(inText, language)) return;
                    string encryptedText = vigenere.Encrypt(inText, key, 1);
                    textBox2.Text = encryptedText;
                    button2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при шифровании: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string outText = textBox2.Text;
            string keyText = textBox3.Text;
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
                if (comboBox4.SelectedIndex == 0)
                {
                    Caesar caesar = new Caesar(language);

                    BigInteger key = GetKeyFromInput(keyText);
                    if (key == BigInteger.Zero) return;

                    if (!GetTextFromOutput(outText, language)) return;
                    string decryptedText = caesar.Decrypt(outText, key);
                    textBox2.Text = decryptedText;
                    button1.Enabled = true;
                    button2.Enabled = false;

                }else if(comboBox4.SelectedIndex == 1)
                {
                    Vigenere vigenere = new Vigenere(language);

                    if (!GetKeyVigrnere(keyText, language)) return;
                    string key = keyText;

                    if (!GetTextFromOutput(outText, language)) return;
                    string decryptedText = vigenere.Decrypt(outText, key);
                    textBox2.Text = decryptedText;
                    button2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
        }

        //Вторая страница
        private void button3_Click(object sender, EventArgs e)
        {
            string outText = textBox5.Text;
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
                if (comboBox4.SelectedIndex == 0)
                {
                    Caesar caesar = new Caesar(language);

                    BigInteger key = GetKeyFromInput(keyText);
                    if (key == BigInteger.Zero) return;

                    if (!GetTextFromOutput(outText, language)) return;
                    string decryptedText = caesar.Decrypt(outText, key);
                    textBox5.Text = decryptedText;
                    button4.Enabled = true;
                }else if (comboBox4.SelectedIndex == 1)
                {
                    Vigenere vigenere = new Vigenere(language);

                    if (!GetKeyVigrnere(keyText, language)) return;
                    string key = keyText;

                    if (!GetTextFromOutput(outText, language)) return;
                    string decryptedText = vigenere.Decrypt(outText, key);
                    textBox5.Text = decryptedText;
                    button4.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string inText = textBox6.Text;
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

                if (comboBox4.SelectedIndex == 0)
                {
                    Caesar caesar = new Caesar(language);

                    BigInteger key = GetKeyFromInput(keyText);
                    if (key == BigInteger.Zero) return;

                    if (!GetTextFromInput(inText, language)) return;
                    string encryptedText = caesar.Encrypt(inText, key);
                    textBox5.Text = encryptedText;
                    button4.Enabled = false;
                }else if (comboBox4.SelectedIndex == 1)
                {
                    Vigenere vigenere = new Vigenere(language);

                    if (!GetKeyVigrnere(keyText, language)) return;
                    string key = keyText;

                    if (!GetTextFromInput(inText, language)) return;
                    string encryptedText = vigenere.Encrypt(inText, key, 1);
                    textBox5.Text = encryptedText;
                    button4.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при шифровании: " + ex.Message);
            }
        }

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
                    textBox6.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            saveFileDialog.Title = "Сохранить текстовый файл";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, textBox5.Text);
                    MessageBox.Show("Файл успешно сохранен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Третья страница
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
                    textBox8.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
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

                if (comboBox4.SelectedIndex == 0)
                {
                    Caesar caesar = new Caesar(language);
                    string text = textBox8.Text;
                    if (!IsValidText(text, language))
                    {
                        MessageBox.Show("Некорректный текст.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        MessageBox.Show("Введите текст для взлома.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var result = caesar.Hack(text);
                    textBox9.Text = result.key.ToString();
                    string hackText = result.decryptedText;
                    textBox7.Text = hackText;



                } else if (comboBox4.SelectedIndex == 1)
                {
                    Vigenere vigenere = new Vigenere(language);
                    string text = textBox8.Text;
                    if (!IsValidText(text, language))
                    {
                        MessageBox.Show("Некорректный текст.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        MessageBox.Show("Введите текст для взлома.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var result = vigenere.Hack(text);
                    textBox9.Text = result.key.ToString();
                    string hackText = result.decryptedText;
                    textBox7.Text = hackText;

                }
                else if (comboBox4.SelectedIndex == 2)
                {
                    MessageBox.Show("Метода расшифровки на данный момент нет.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            saveFileDialog.Title = "Сохранить текстовый файл";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, textBox7.Text);
                    MessageBox.Show("Файл успешно сохранен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private BigInteger GetKeyFromInput(string keyText)
        {
            if (string.IsNullOrWhiteSpace(keyText))
            {
                MessageBox.Show("Пожалуйста, введите ключ.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return BigInteger.Zero;
            }

            if (keyText.Contains(" "))
            {
                MessageBox.Show("Ключ не должен содержать пробелов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return BigInteger.Zero;
            }

            if (!BigInteger.TryParse(keyText, out BigInteger key))
            {
                MessageBox.Show("Ключ должен быть целым числом.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return BigInteger.Zero;
            }

            return key;
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
                MessageBox.Show("Ключ некорректный.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Ключ не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}