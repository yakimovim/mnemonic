namespace Mnemonic
{
    internal abstract class WordsTreeNode<U>
        where U : WordsTreeNode<U>
    {
        protected readonly Lazy<Dictionary<int, U>> _subNodes =
            new Lazy<Dictionary<int, U>>(
                () => new Dictionary<int, U>()
            );

        private readonly Func<int, U> _nodeCreator;

        public WordsTreeNode(Func<int, U> nodeCreator)
        {
            _nodeCreator = nodeCreator ?? throw new ArgumentNullException(nameof(nodeCreator));
        }

        public IReadOnlyDictionary<int, U> SubNodes => _subNodes.Value;

        public U GetOrAddSubNode(int digit)
        {
            var subNodes = _subNodes.Value;
            if (subNodes.ContainsKey(digit))
                return subNodes[digit];

            var subNode = _nodeCreator(digit);
            subNodes.Add(digit, subNode);
            return subNode;
        }

        public abstract void AddWord(string word);
    }

    internal sealed class WordsTree : WordsTreeNode<WordsTreeNode>
    {
        private readonly static IReadOnlySet<string> NoWords = new HashSet<string>();

        public WordsTree()
            : base(digit => new WordsTreeNode(digit))
        {}

        public IReadOnlySet<string> GetWords(string numberString)
        {
            WordsTreeNode? node = null;
            IReadOnlyDictionary<int, WordsTreeNode> subNodes = SubNodes;

            foreach (var digitChar in numberString)
            {
                if (!int.TryParse(digitChar.ToString(), out var digit))
                {
                    Console.WriteLine("Number must contain only digits");
                    return NoWords;
                }

                if (!subNodes.ContainsKey(digit))
                    return NoWords;

                node = subNodes[digit];
                subNodes = node.SubNodes;
            }

            return node == null ? NoWords : node.GetNestedWords();
        }

        public override void AddWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return;

            var code = WordEncoder.GetWordCode(word);

            if (code.Count == 0)
                return;

            WordsTreeNode<WordsTreeNode> node = this;

            foreach (var digit in code)
            {
                node = node.GetOrAddSubNode(digit);
            }

            node.AddWord(word);
        }
    }

    internal sealed class WordsTreeNode : WordsTreeNode<WordsTreeNode>
    {
        private readonly Lazy<HashSet<string>> _words =
            new Lazy<HashSet<string>>(
                    () => new HashSet<string>()
                );

        public int Digit { get; init; }

        public IReadOnlySet<string> Words => _words.Value;

        public WordsTreeNode(int digit)
            : base(digit => new WordsTreeNode(digit))
        {
            if (digit < 0 || digit > 9) 
                throw new ArgumentOutOfRangeException(nameof(digit), "Digit must be between 0 and 9"); ;
            Digit = digit;
        }

        public override void AddWord(string word)
        {
            _words.Value.Add(word);
        }

        public IReadOnlySet<string> GetNestedWords()
        {
            var words = new HashSet<string>();

            if(_words.IsValueCreated)
            {
                words.UnionWith(_words.Value);
            }

            if(_subNodes.IsValueCreated)
            {
                foreach (var subNode in _subNodes.Value.Values)
                {
                    words.UnionWith(subNode.GetNestedWords());
                }
            }

            return words;
        }
    }
}
