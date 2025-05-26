using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Inf_Sec
{
    public class Caesar
    {
        public const string EngAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public const string RuAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ1234567890";
        private string alphabet;

        public Caesar(string language)
        {
            if (language.ToLower() == "ru")
                alphabet = RuAlphabet;
            else if (language.ToLower() == "en")
                alphabet = EngAlphabet;
        }

        public string Encrypt(string input, BigInteger key)
        {
            BigInteger key1 = key % alphabet.Length;
            char[] text = input.ToCharArray();
            for (int i = 0; i < text.Length; i++)
            {
                char letter = text[i];
                bool isUpper = char.IsUpper(letter);
                letter = char.ToUpper(letter);
                if (alphabet.Contains(letter))
                {
                    int letPos = alphabet.IndexOf(letter);
                    int newLetPos = (letPos + (int)key1 + alphabet.Length) % alphabet.Length;
                    char encrLet = alphabet[newLetPos];
                    text[i] = isUpper ? encrLet : char.ToLower(encrLet);
                }
            }
            return new string(text);
        }

        public string Decrypt(string input, BigInteger key)
        {
            return Encrypt(input, -key);
        }

        public (string decryptedText, int key) Hack(string text)
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
            string decryptedText = Decrypt(text, shift);
            return (decryptedText, shift);
        }
    }
}
