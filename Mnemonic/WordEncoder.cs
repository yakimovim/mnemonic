using System.Text;

namespace Mnemonic
{
    internal static class WordEncoder
    {
        private readonly static IReadOnlyDictionary<int, char[]> LetterCodes = new Dictionary<int, char[]>
        {
            { 0, new [] { 'н', 'м' } },
            { 1, new [] { 'г', 'ж' } },
            { 2, new [] { 'д', 'т' } },
            { 3, new [] { 'к', 'х' } },
            { 4, new [] { 'ч', 'щ' } },
            { 5, new [] { 'п', 'б' } },
            { 6, new [] { 'ш', 'л' } },
            { 7, new [] { 'с', 'з' } },
            { 8, new [] { 'в', 'ф' } },
            { 9, new [] { 'р', 'ц' } },
        };

        private readonly static IReadOnlyDictionary<char, int> CodesOfLetters;

        static WordEncoder()
        {
            var codesOfLetters = new Dictionary<char, int>();

            foreach (var item in LetterCodes)
            {
                foreach (var letter in item.Value)
                {
                    codesOfLetters.Add(letter, item.Key);
                }
            }

            CodesOfLetters = codesOfLetters;
        }

        public static IReadOnlyCollection<int> GetWordCode(string word)
        {
            var code = new LinkedList<int>();

            foreach (var letter in word)
            {
                if (CodesOfLetters.ContainsKey(letter))
                    code.AddLast(CodesOfLetters[letter]);
            }

            return code;
        }
    }
}
