using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab9.Green;

public class Task1 : Green
{
    private static readonly char[] RussianAlphabet =
    [
        'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о',
        'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'
    ];

    private (char, double)[] _output;

    public (char, double)[] Output => _output.ToArray();

    public Task1(string text) : base(text)
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

        int[] counts = new int[RussianAlphabet.Length];
        int totalLetters = 0;

        for (int i = 0; i < Input.Length; i++)
        {
            char lowered = char.ToLower(Input[i]);
            if (char.IsLetter(lowered))
            {
                totalLetters++;

                int index = GetRussianLetterIndex(lowered);
                if (index >= 0)
                {
                    counts[index]++;
                }
            }
        }

        if (totalLetters == 0)
        {
            return;
        }

        List<(char, double)> result = [];
        for (int i = 0; i < RussianAlphabet.Length; i++)
        {
            if (counts[i] > 0)
            {
                result.Add((RussianAlphabet[i], Math.Round((double)counts[i] / totalLetters, 4)));
            }
        }

        _output = result.ToArray();
    }

    public override string ToString()
    {
        if (_output == null || _output.Length == 0)
        {
            return string.Empty;
        }

        return string.Join("\n", _output.Select(item => $"{item.Item1}:{item.Item2:F4}"));
    }

    private static int GetRussianLetterIndex(char letter)
    {
        for (int i = 0; i < RussianAlphabet.Length; i++)
        {
            if (RussianAlphabet[i] == letter)
            {
                return i;
            }
        }

        return -1;
    }
}
