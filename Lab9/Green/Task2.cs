using System.Collections.Generic;
using System.Linq;

namespace Lab9.Green;

public class Task2 : Green
{
    private char[] _output;

    public char[] Output => _output.ToArray();

    public Task2(string text) : base(text)
    {
        _output = [];
    }

    public override void Review()
    {
        _output = [];

        if (Input == null || Input.Length == 0)
        {
            return;
        }

        Dictionary<char, int> counts = [];
        foreach (string word in MadeListOfWords(Input))
        {
            if (word.Length > 0)
            {
                char first = char.ToLower(word[0]);  // FIX 1: приводим к нижнему регистру
                if (!char.IsLetter(first))           // FIX 2: пропускаем, если не буква
                    continue;
                    
                if (counts.ContainsKey(first))
                    counts[first]++;
                else
                    counts[first] = 1;
            }
        }
        
        char[] letters = counts.Keys.ToArray();
        for (int i = 0; i < letters.Length - 1; i++)
        {
            for (int j = 0; j < letters.Length - i - 1; j++)
            {
                bool shouldSwap = counts[letters[j]] < counts[letters[j + 1]]
                                  || counts[letters[j]] == counts[letters[j + 1]] && letters[j] > letters[j + 1];

                if (shouldSwap)
                {
                    (letters[j], letters[j + 1]) = (letters[j + 1], letters[j]);
                }
            }
        }

        _output = letters;
    }
    
    public override string ToString()
    {
        // FIX 3: возвращаем не пустую строку, а буквы через запятую
        if (_output == null || _output.Length == 0)
        {
            return string.Empty;
        }

        return string.Join(", ", _output);
    }

    // MadeListOfWords и IsWordSymbol — без изменений
    private List<string> MadeListOfWords(string text)
    {
        if (text == null || text.Length == 0)
            return [];
        
        List<string> words = [];

        int index = 0;
        while (index < text.Length)
        {
            if (!IsWordSymbol(text[index]))
            {
                index++;
                continue;
            }
            
            int start = index;
            bool hasLetter = false;
            while (index < text.Length && IsWordSymbol(text[index]))
            {
                if (char.IsLetter(text[index])) hasLetter = true;
                index++;
            }

            int length = index - start;
            bool touchesDigit = start > 0 && char.IsDigit(text[start - 1]) || index < text.Length && char.IsDigit(text[index]);
            
            if (!touchesDigit && hasLetter)
                words.Add(text.Substring(start, length));
        }
        
        return words;
    }
    
    private bool IsWordSymbol(char c)
    {
        return char.IsLetter(c) || c == '-' || c == '\'';
    }
}