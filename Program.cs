namespace ParserEngine
{
    internal class Program
    {
        public static string _path;
        static void Main()
        {
            // Запуск браузера
            Engine.Start();

            // Проверка настроек папки с командами
            if (!File.Exists(Environment.CurrentDirectory + @"path.tmp"))
            {
                Console.WriteLine("Введи путь к папке с командами"); _path = Console.ReadLine();
                File.WriteAllText(Environment.CurrentDirectory + @"path.tmp", _path);
            }
            else
            {
                _path = File.ReadAllText(Environment.CurrentDirectory + @"path.tmp");
            }

            Console.WriteLine(_path);

            // Бесконечный цикл чтения команд с паузой на сутки
            while (true)
            {
                var files = Directory.GetFiles(_path);
                foreach (var file in files)
                {
                    Manager.ReadCommand(file);
                }

                Thread.Sleep((24*360000));
            }
            


            Console.ReadKey();
        }
    }
}