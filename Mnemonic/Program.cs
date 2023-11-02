using Mnemonic;
using System.Text;

var wordsTree = new WordsTree();

await foreach (var russianWord in RussianWordsReader.GetRussianWords())
{
    wordsTree.AddWord(russianWord);
}

Console.OutputEncoding = Encoding.UTF8;

while(true)
{
    Console.Write("Введите число или нажмите ENTER чтобы выйти: ");
    var numberString = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(numberString))
        return;

    var words = wordsTree.GetWords(numberString);

    if (words.Count == 0)
    {
        Console.WriteLine("Слова не найдены");
        Console.WriteLine();
        continue;
    }

    foreach (var word in words)
    {
        Console.WriteLine(word);
    }

    Console.WriteLine();
}


