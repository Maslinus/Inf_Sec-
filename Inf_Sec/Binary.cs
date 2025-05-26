using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Inf_Sec
{
    internal class Binary
    {
        public const string EngAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public const string RuAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ1234567890";
        private string alphabet;

        public Binary(string language)
        {
            if (language.ToLower() == "ru")
                alphabet = RuAlphabet;
            else if (language.ToLower() == "en")
                alphabet = EngAlphabet;
        }

        public (string encryptText, string messageBinary, string encryptedMessageBinary, string key) Encrypt(string inputMessage, string key)
        {
            string inputMessageUP = inputMessage.ToUpper();
            string messageBinary = ToBinaryString(inputMessage);
            string keyBinary = ToBinaryString(key);

            string repeatedKey = GenerateRepeatedKey(keyBinary, messageBinary.Length);

            string encryptedMessageBinary = XORStrings(messageBinary, repeatedKey);

            return (encryptedMessageBinary, messageBinary, encryptedMessageBinary, keyBinary);
        }
        public (string encryptText, string messageBinary, string encryptedMessageBinary, string key) EncryptR(string inputMessage)
        {
            string messageBinary = ToBinaryString(inputMessage);
            string keyBinary = GeneratePerfectGamma(messageBinary.Length);

            string encryptedMessageBinary = XORStrings(messageBinary, keyBinary);

            return (encryptedMessageBinary, messageBinary, encryptedMessageBinary, keyBinary);
        }
        public string ToBinaryString(string text)
        {
            byte[] byteRepresentation = ToByteRepresentation(text, alphabet);
            var binaryString2 = string.Concat(byteRepresentation.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            return binaryString2.ToString();
        }

        static byte[] ToByteRepresentation(string text, string alphabet)
        {
            // Преобразуем каждый символ текста в его индекс в алфавите
            string textUP= text.ToUpper();
            return textUP.Select(c =>
            {
                int index = alphabet.IndexOf(c); // Находим индекс символа в алфавите
                if (index == -1)
                    throw new ArgumentException($"Символ '{c}' не найден в алфавите.");
                return (byte)index; // Возвращаем индекс как байт
            }).ToArray();
        }

        static string GenerateRepeatedKey(string keyBinary, int length)
        {
            StringBuilder repeatedKey = new StringBuilder();
            int keyIndex = 0;

            while (repeatedKey.Length < length)
            {
                repeatedKey.Append(keyBinary[keyIndex]);
                keyIndex = (keyIndex + 1) % keyBinary.Length;
            }

            return repeatedKey.ToString();
        }

        public string ToTextFromBinaryString(string binaryString)
        {
            var stringArray = Enumerable.Range(0, binaryString.Length / 8).Select(i => Convert.ToByte(binaryString.Substring(i * 8, 8), 2)).ToArray();
            string str = ToStringFromByteArray(stringArray, alphabet);
            str = str.ToLower();

            return str;
        }

        static string ToStringFromByteArray(byte[] byteArray, string alphabet)
        {
            char[] charArray = new char[byteArray.Length];

            for (int i = 0; i < byteArray.Length; i++)
            {
                charArray[i] = alphabet[byteArray[i]];
            }
            return new string(charArray);
        }

        // Гаммирование
        static string XORStrings(string str1, string str2)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < str1.Length; i++)
            {
                result.Append(str1[i] == str2[i] ? '0' : '1');
            }
            return result.ToString();
        }

        public string Decrypt(string encryptedMessageBinary, string inputKey)
        {
            string keyBinary = ToBinaryString(inputKey);

            string repeatedKey = GenerateRepeatedKey(keyBinary, encryptedMessageBinary.Length);

            string decryptedMessageBinary = XORStrings(encryptedMessageBinary, repeatedKey);

            string decryptedMessage = ToTextFromBinaryString(decryptedMessageBinary);

            return decryptedMessage;
        }

        public string DecryptR(string encryptedMessageBinary, string RandomBinaryKey)
        {
            string decryptedMessageBinary = XORStrings(encryptedMessageBinary, RandomBinaryKey);

            string decryptedMessage = ToTextFromBinaryString(decryptedMessageBinary);

            return decryptedMessage;
        }

        public static string GeneratePerfectGamma(int length)
        {
            int halfLength = length / 2;
            char[] keyArray = new char[length];
            for (int i = 0; i < halfLength; i++)
            {
                keyArray[i] = '1';
                keyArray[halfLength + i] = '0';
            }
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(length);
                char temp = keyArray[i];
                keyArray[i] = keyArray[randomIndex];
                keyArray[randomIndex] = temp;
            }

            return new string(keyArray);
        }

        public (string encryptText, string messageBinary, string encryptedMessageBinary, string key, string blockToEncrypt1, string binaryKey) EncryptCBC(string plaintext, string key)
        {
            string k = key[0].ToString();
            string binaryKey = ToBinaryString(k);
            string binaryMessage = ToBinaryString(plaintext);

            string iv = GeneratePerfectGamma(8);
            string prevCipherBlock = iv;
            string block1 = binaryMessage.Substring(0, Math.Min(8, binaryMessage.Length - 0));
            string blockToEncrypt1 = XORStrings(block1, prevCipherBlock);

            StringBuilder ciphertext = new StringBuilder();

            for (int i = 0; i < binaryMessage.Length; i += 8)
            {
                string block = binaryMessage.Substring(i, Math.Min(8, binaryMessage.Length - i));

                string blockToEncrypt = XORStrings(block, prevCipherBlock);

                string encryptedBlock = XORStrings(blockToEncrypt, binaryKey);

                ciphertext.Append(encryptedBlock);

                prevCipherBlock = encryptedBlock;
            }

            return (ciphertext.ToString(), binaryMessage, ciphertext.ToString(), iv, blockToEncrypt1, binaryKey);
        }

        public string DecryptCBC(string ciphertext, string key, string gamma)
        {
            string k = key[0].ToString();
            string binaryKey = ToBinaryString(k);

            string iv = gamma;
            string prevCipherBlock = iv;

            StringBuilder plaintext = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i += 8)
            {
                string block = ciphertext.Substring(i, Math.Min(8, ciphertext.Length - i));

                string decryptedBlock = XORStrings(block, binaryKey);

                string originalBlock = XORStrings(decryptedBlock, prevCipherBlock);

                plaintext.Append(originalBlock);

                prevCipherBlock = block;
            }

            return ToTextFromBinaryString(plaintext.ToString());
        }
    }
}
