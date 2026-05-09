using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab9.Green;

public class Task4 : Green
{
    private string[] _output;

    public string[] Output => _output.ToArray();

    public Task4(string text) : base(text)
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

        string[] surnames = Input
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(item => item.Trim())
            .Where(item => item.Length > 0)
            .ToArray();

        for (int i = 0; i < surnames.Length - 1; i++)
        {
            for (int j = 0; j < surnames.Length - i - 1; j++)
            {
                if (CompareStrings(surnames[j], surnames[j + 1]) > 0)
                {
                    (surnames[j], surnames[j + 1]) = (surnames[j + 1], surnames[j]);
                }
            }
        }

        _output = surnames;
    }

    public override string ToString()
    {
        if (_output == null || _output.Length == 0)
        {
            return string.Empty;
        }

        return string.Join(Environment.NewLine, _output);
    }

    private static int CompareStrings(string left, string right)
    {
        if (left == null && right == null)
        {
            return 0;
        }

        if (left == null)
        {
            return -1;
        }

        if (right == null)
        {
            return 1;
        }

        int minLength = Math.Min(left.Length, right.Length);
        for (int i = 0; i < minLength; i++)
        {
            char l = char.ToLower(left[i]);
            char r = char.ToLower(right[i]);

            if (l < r)
            {
                return -1;
            }

            if (l > r)
            {
                return 1;
            }
        }

        if (left.Length < right.Length)
        {
            return -1;
        }

        if (left.Length > right.Length)
        {
            return 1;
        }

        return 0;
    }
}