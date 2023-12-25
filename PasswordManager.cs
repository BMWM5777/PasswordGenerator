using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Password
{
    public class PasswordManager
    {
        private List<PasswordTemplate> templates = new List<PasswordTemplate>();
        private List<string> generatedPasswords = new List<string>();
        private bool excludeSimilarCharacters = true; // Новая переменная для исключения похожих символов
        private int memorablePasswordLength = 10; // Новая переменная для длины легко произносимого пароля
        private void DisplaySecurityTips()
        {
            Console.WriteLine("Советы по безопасности:");
            Console.WriteLine("- Не используйте один и тот же пароль на разных сайтах;");
            Console.WriteLine("- Избегайте использования личных информационных фрагментов в пароле;");
            Console.WriteLine("- Периодически изменяйте свои пароли;");
            Console.WriteLine("- Храните пароли в надежном месте, избегайте записей на бумаге.");
        }

        public void AddPasswordTemplate()
        {
            Console.WriteLine("Введите шаблон пароля (используйте символ '*' для обозначения пропусков):");
            string template = Console.ReadLine();

            Console.WriteLine("Введите категорию шаблона:");
            string category = Console.ReadLine();

            // Включить прописные буквы (A-Z)?
            bool includeUppercase = GetYesNoAnswer("Включить прописные буквы (A-Z)?");

            // Включить строчные буквы (a-z)?
            bool includeLowercase = GetYesNoAnswer("Включить строчные буквы (a-z)?");

            // Включить цифры (0-9)?
            bool includeDigits = GetYesNoAnswer("Включить цифры (0-9)?");

            // Включить специальные символы (!@#$%^&*()-_=+[]{}|;:'",.<>/?)?
            bool includeSpecialCharacters = GetYesNoAnswer("Включить специальные символы (!@#$%^&*()-_=+[]{}|;:'\",.<>/?)?");

            // Исключить похожие символы (I, l, 1, O, 0)?
            excludeSimilarCharacters = GetYesNoAnswer("Исключить похожие символы (I, l, 1, O, 0)?");

            templates.Add(new PasswordTemplate
            {
                Template = template,
                Category = category,
                IncludeUppercase = includeUppercase,
                IncludeLowercase = includeLowercase,
                IncludeDigits = includeDigits,
                IncludeSpecialCharacters = includeSpecialCharacters
            });

            Console.WriteLine("Шаблон успешно добавлен!");
        }

        public void GeneratePassword()
        {
            Console.WriteLine("Выберите категорию:");
            string category = Console.ReadLine();

            List<PasswordTemplate> matchingTemplates = templates.FindAll(t => t.Category == category);
            if (matchingTemplates.Count == 0)
            {
                Console.WriteLine("Нет доступных шаблонов для данной категории.");
                return;
            }

            Console.WriteLine("Сколько паролей вы хотите сгенерировать?");
            int count;
            if (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное число.");
                return;
            }

            foreach (PasswordTemplate template in matchingTemplates)
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Заполните пропуски для шаблона '{template.Template}':");
                    string password = GeneratePasswordFromTemplate(template.Template, template.IncludeUppercase, template.IncludeLowercase, template.IncludeDigits, template.IncludeSpecialCharacters);
                    generatedPasswords.Add(password);
                    Console.WriteLine($"Сгенерированный пароль: {password}");
                    // Вывод советов по безопасности
                    DisplaySecurityTips();
                }
            }
        }

        public void GenerateRandomPassword()
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            Random random = new Random();
            int minLength = 8;
            int maxLength = 32;

            Console.WriteLine("Введите длину случайного пароля:");
            int passwordLength;
            if (!int.TryParse(Console.ReadLine(), out passwordLength) || passwordLength < minLength || passwordLength > maxLength)
            {
                Console.WriteLine($"Некорректная длина пароля. Введите число от {minLength} до {maxLength}.");
                return;
            }

            char[] password = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = characters[random.Next(characters.Length)];
            }

            if (excludeSimilarCharacters)
            {
                password = ExcludeSimilar(password);
            }

            string randomPassword = new string(password);
            generatedPasswords.Add(randomPassword);

            Console.WriteLine($"Сгенерированный случайный пароль: {randomPassword}");
            // Вывод советов по безопасности
            DisplaySecurityTips();
        }

        public void GenerateMemorablePassword()
        {
            Console.WriteLine("Введите длину легко произносимого пароля:");
            int length;
            if (!int.TryParse(Console.ReadLine(), out length) || length <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное число.");
                return;
            }

            string memorablePassword = GenerateMemorablePassword(length);
            generatedPasswords.Add(memorablePassword);

            Console.WriteLine($"Сгенерированный легко произносимый пароль: {memorablePassword}");
            // Вывод советов по безопасности
            DisplaySecurityTips();
        }

        public void SetMemorablePasswordLength()
        {
            Console.WriteLine("Введите длину легко произносимого пароля:");
            int length;
            if (!int.TryParse(Console.ReadLine(), out length) || length <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное число.");
                return;
            }

            memorablePasswordLength = length;
            Console.WriteLine($"Длина легко произносимого пароля установлена: {memorablePasswordLength} символов.");
        }

        private string GeneratePasswordFromTemplate(string template, bool includeUppercase, bool includeLowercase, bool includeDigits, bool includeSpecialCharacters)
        {
            Random random = new Random();
            char[] password = new char[template.Length];

            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] == '*')
                {
                    string characters = "";

                    if (includeUppercase)
                        characters += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                    if (includeLowercase)
                        characters += "abcdefghijklmnopqrstuvwxyz";

                    if (includeDigits)
                        characters += "0123456789";

                    if (includeSpecialCharacters)
                        characters += "!@#$%^&*()";

                    password[i] = characters[random.Next(characters.Length)];
                }
                else
                {
                    password[i] = template[i];
                }
            }

            return new string(password);
        }

        private char[] ExcludeSimilar(char[] password)
        {
            for (int i = 0; i < password.Length; i++)
            {
                switch (password[i])
                {
                    case 'I':
                    case 'l':
                    case '1':
                    case 'O':
                    case '0':
                        password[i] = GetRandomNonSimilarCharacter();
                        break;
                }
            }
            return password;
        }

        private char GetRandomNonSimilarCharacter()
        {
            const string characters = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@#$%^&*()";
            Random random = new Random();
            return characters[random.Next(characters.Length)];
        }

        private string GenerateMemorablePassword(int length)
        {
            string vowels = "aeiou";
            string consonants = "bcdfghjklmnpqrstvwxyz";
            StringBuilder passwordBuilder = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    passwordBuilder.Append(consonants[random.Next(consonants.Length)]);
                }
                else
                {
                    passwordBuilder.Append(vowels[random.Next(vowels.Length)]);
                }
            }

            return passwordBuilder.ToString();
        }

        public void SaveGeneratedPasswordsToFile()
        {
            Console.WriteLine("Введите полный путь и имя файла для сохранения паролей:");
            string fileName = Console.ReadLine();

            try
            {
                System.IO.File.WriteAllLines(fileName, generatedPasswords);
                Console.WriteLine("Пароли успешно сохранены в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        public void LoadPasswordsFromFile()
        {
            Console.WriteLine("Введите имя файла для загрузки паролей:");
            string fileName = Console.ReadLine();

            if (System.IO.File.Exists(fileName))
            {
                string[] passwords = System.IO.File.ReadAllLines(fileName);

                Console.WriteLine("Загруженные пароли:");

                foreach (var password in passwords)
                {
                    Console.WriteLine(password);
                }

                Console.WriteLine("Пароли успешно загружены из файла.");
            }
            else
            {
                Console.WriteLine("Файл не найден.");
            }
        }

        public void SaveTemplatesToFile()
        {
            Console.WriteLine("Введите полный путь и имя файла для сохранения шаблонов:");
            string fileName = Console.ReadLine();

            try
            {
                // Преобразование списка шаблонов в строки
                List<string> templateLines = templates.Select(t => $"{t.Template},{t.Category},{t.IncludeUppercase},{t.IncludeLowercase},{t.IncludeDigits},{t.IncludeSpecialCharacters}").ToList();
                System.IO.File.WriteAllLines(fileName, templateLines);
                Console.WriteLine("Шаблоны успешно сохранены в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        public void LoadTemplatesFromFile()
        {
            Console.WriteLine("Введите имя файла для загрузки шаблонов:");
            string fileName = Console.ReadLine();

            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    // Чтение строк из файла
                    string[] templateLines = System.IO.File.ReadAllLines(fileName);

                    // Преобразование строк в шаблоны и добавление их в список
                    foreach (var line in templateLines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 6)
                        {
                            templates.Add(new PasswordTemplate
                            {
                                Template = parts[0],
                                Category = parts[1],
                                IncludeUppercase = bool.Parse(parts[2]),
                                IncludeLowercase = bool.Parse(parts[3]),
                                IncludeDigits = bool.Parse(parts[4]),
                                IncludeSpecialCharacters = bool.Parse(parts[5])
                            });
                        }
                    }

                    Console.WriteLine("Шаблоны успешно загружены из файла.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Файл не найден.");
            }
        }



        private bool GetYesNoAnswer(string question)
        {
            Console.Write($"{question} (y/n): ");
            char answer = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine(); // Добавим пустую строку для лучшего визуального разделения

            return answer == 'y';
        }
    }
}