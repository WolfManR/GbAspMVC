using System.Collections.Generic;
using System.Linq;
using static System.Console;

string a = "AABBJJOODD";
string b = "ABJODABJOD";

WriteLine(Compare(a, b));

// There still problem with comparing symbols in different unicode tables
bool Compare(string left, string right)
{
    Dictionary<char, int> tempCounter = new();

    foreach (var character in left)
    {
        if (!tempCounter.ContainsKey(character))
        {
            tempCounter.Add(character, 1);
            continue;
        }
        tempCounter[character]++;
    }

    foreach (var character in right)
    {
        if (!tempCounter.ContainsKey(character)) return false;
        tempCounter[character]--;
        if (tempCounter[character] < 0) return false;
    }
    
    return !tempCounter.Values.Any(i => i > 0);
}