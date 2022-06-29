using Mnemonic;

var wordsTree = new WordsTree();

await foreach (var russianWord in RussianWordsReader.GetRussianWords())
{
    wordsTree.AddWord(russianWord);
}

Console.Write("Enter a number: ");
var numberString = Console.ReadLine();

if (string.IsNullOrEmpty(numberString))
    return;

var words = wordsTree.GetWords(numberString);

foreach (var word in words.OrderBy(w => w.Length))
{
    Console.WriteLine(word);
}

