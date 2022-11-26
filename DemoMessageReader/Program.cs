using DemoInfo;

namespace DemoMessageReader
{
    public class DemoMessageReader
    {
        private static string? _file = string.Empty;
        private static readonly List<string> Messages = new();

        public static void Main(string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                Console.WriteLine("Drag a file here to use.");
                _file = Console.ReadLine();
            }
            else
                _file = args[0];

            try
            {
                Console.WriteLine("Opening demo...");
                using var fs = new FileStream(_file!, FileMode.Open);
                using var dp = new DemoParser(fs);
                dp.SayText2 += DpOnSayText2;
                dp.ParseHeader();
                dp.ParseToEnd();

                var fileInfo = new FileInfo(_file!);
                _file = $"{fileInfo.DirectoryName}\\{fileInfo.Name.Replace(fileInfo.Extension, "")}_messages.txt";

                Console.WriteLine("Writing messages to " + _file);
                File.WriteAllText(_file, string.Join("\n", Messages));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static void DpOnSayText2(object? sender, SayText2EventArgs e)
        {
            Messages.Add($"{e.Sender.Name} [{(e.IsChatAll ? "all" : "team")}]: {e.Text}");
        }
    }
}