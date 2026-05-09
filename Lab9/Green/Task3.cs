using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab9.Green;

public class Task3 : Green
{
    private string _pattern;
    private string[] _output;

    public string[] Output => _output.ToArray();

    public Task3(string text, string pattern) : base(text)
    {
        _pattern = pattern ?? string.Empty;
        _output = [];
    }

    public override void Review()
    {
        _output = [];

        if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(_pattern))
        {
            return;
        }

        string loweredPattern = _pattern.ToLower();
        string[] allWords = ExtractWords(Input);
        string[] result = [];

        foreach (string word in allWords)
        {
            if (word.ToLower().Contains(loweredPattern))
            {
                if (!result.Contains(word, StringComparer.OrdinalIgnoreCase))
                    result = result.Append(word).ToArray();
            }
        }

        _output = result;
    }

    public override string ToString()
    {
        if (_output == null || _output.Length == 0) return string.Empty;
        return string.Join(Environment.NewLine, _output);
    }

    private static string[] ExtractWords(string text)
    {
        string[] words = [];
        int index = 0;

        while (index < text.Length)
        {
            if (!IsWordSymbol(text[index]))
            {
                index++;
                continue;
            }

            int start = index;
            string word = "";

            while (index < text.Length && IsWordSymbol(text[index]))
            {
                word += text[index];
                index++;
            }

            int end = index - 1;
            bool isDashed = text[start] == '-' || text[end] == '-';
            
            if (!isDashed && word.Any(char.IsLetter))
            {
                words = words.Append(word).ToArray();
            }
        }

        return words;
    }

    private static bool IsWordSymbol(char symbol)
    {
        return char.IsLetter(symbol) || symbol == '-' || symbol == '\'';
    }
}