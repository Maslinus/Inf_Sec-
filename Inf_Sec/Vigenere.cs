using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Inf_Sec
{
    public class Vigenere
    {
        public const string EngAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public const string RuAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ1234567890";
        private string alphabet;

        public Vigenere(string language)
        {
            if (language.ToLower() == "ru")
                alphabet = RuAlphabet;
            else if (language.ToLower() == "en")
                alphabet = EngAlphabet;
        }
        public string Encrypt(string plainText, string key, int r)
        {
            string keyUP = key.ToUpper();

            string result = "";
            int keyword_index = 0;

            foreach (char symbol in plainText)
            {
                char letter = symbol;
                bool isUpper = char.IsUpper(letter);
                letter = char.ToUpper(letter);
                if (alphabet.Contains(letter))
                {
                    int symbolIndex = alphabet.IndexOf(letter);
                    int keyIndex = alphabet.IndexOf(keyUP[keyword_index]);

                    int c = (symbolIndex + keyIndex * r + alphabet.Length) % alphabet.Length;
                    if (!isUpper)
                    {
                        result += char.ToLower(alphabet[c]);
                    }
                    else
                    {
                        result += alphabet[c];
                    }

                    keyword_index++;

                    if (keyword_index >= keyUP.Length)
                        keyword_index = 0;
                }
            }

            return result;
        }

        public string Decrypt(string plainText, string key)
        {
            string result = Encrypt(plainText, key, -1);
            return result;
        }

        public (string decryptedText, string key) Hack(string cipherText)
        {
            int keyLength = FindKeyLengthByIOC(cipherText);  // Находим длину ключа

            string key = FindKey(cipherText, keyLength);  // Находим ключ

            string decryptedText = Decrypt(cipherText, key);  // Расшифровка текста

            return (decryptedText, key);
        }

        public int FindKeyLengthByIOC(string cipherText)
        {
            double bestIOC = 0.000001;
            int bestKeyLength = 0;
            for (int keyLength = 1; keyLength <= 20; keyLength++)
            {
                List<string> groups = new List<string>();
                for (int i = 0; i < keyLength; i++)
                {
                    string group = "";
                    for (int j = i; j < cipherText.Length; j += keyLength)
                    {
                        if (char.IsLetter(cipherText[j]))
                        {
                            group += cipherText[j];
                        }
                    }
                    groups.Add(group);
                }

                double totalIOC = 0;
                foreach (var group in groups)
                {
                    totalIOC += CalculateIOC(group);
                }
                Console.WriteLine(totalIOC);
                double averageIOC = Math.Floor((totalIOC / keyLength) * 1000) / 1000;

                if (averageIOC > bestIOC && (averageIOC / bestIOC > 1.2))
                {
                    bestIOC = averageIOC;
                    bestKeyLength = keyLength;
                }
            }
            return bestKeyLength;
        }

        // Расчет индекса совпадений
        public double CalculateIOC(string text)
        {
            int[] letterCounts = new int[alphabet.Length];
            int totalLetters = 0;

            foreach (char c in text.ToUpper())
            {
                if (char.IsLetter(c))
                {
                    letterCounts[c % alphabet.Length]++;
                    totalLetters++;
                }
            }
            double numerator = 0;
            foreach (var count in letterCounts)
            {
                numerator += count * (count - 1);
            }

            return numerator / (totalLetters * (totalLetters - 1));
        }

        public string FindKey(string cipherText, int keyLength)
        {
            string key = "";
            for (int i = 0; i < keyLength; i++)
            {
                string substring = "";
                for (int j = i; j < cipherText.Length; j += keyLength)
                {
                    substring += cipherText[j];
                }
                char probableKeyChar = FindMostFrequentLetter(substring);
                key += probableKeyChar;
            }
            return key;
        }

        private char FindMostFrequentLetter(string text)
        {
            string textUp = text.ToUpper();
            Dictionary<char, int> frequency = new Dictionary<char, int>();
            foreach (char letter in textUp)
            {
                if (alphabet.Contains(letter))
                {
                    if (!frequency.ContainsKey(letter))
                    {
                        frequency[letter] = 0;
                    }
                    frequency[letter]++;
                }
            }
            char maxLetter = '\0';
            int maxFrequency = 0;

            foreach (var symbolFrequency in frequency)
            {
                if (symbolFrequency.Value > maxFrequency)
                {
                    maxFrequency = symbolFrequency.Value;
                    maxLetter = symbolFrequency.Key;
                }
            }
            char exeptLet;
            if (alphabet.Contains('E'))
            {
                exeptLet = 'E';
            }
            else
            {
                exeptLet = 'О';
            }
            int maxFrequencyIndex = alphabet.IndexOf(maxLetter);
            int exeptLetIndex = alphabet.IndexOf(exeptLet);
            int shift = maxFrequencyIndex - exeptLetIndex;
            if (shift < 0)
            {
                shift += alphabet.Length;
            }
            return alphabet[shift];
        }
    }
}
