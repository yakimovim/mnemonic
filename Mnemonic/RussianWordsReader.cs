using System.Reflection;
using System.Text;

namespace Mnemonic
{
    internal static class RussianWordsReader
    {
        static RussianWordsReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static async IAsyncEnumerable<string> GetRussianWords()
        {
            using var russianWordsStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(
                    "Mnemonic.Russian words.txt"
                );

            if (russianWordsStream == null)
            {
                Console.WriteLine("There are no Russian words");
                yield break;
            }

            using var russianWordsReader = new StreamReader(
                    russianWordsStream,
                    Encoding.GetEncoding("windows-1251")
                );

            while (true)
            {
                var russianWord = await russianWordsReader.ReadLineAsync();

                if (russianWord == null) break;

                russianWord = russianWord.ToLower();

                yield return russianWord;
            }
        }
    }
}
