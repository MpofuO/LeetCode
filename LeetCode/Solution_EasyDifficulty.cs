using System.Collections.Generic;
using System.Text;

namespace LeetCode;

internal class Solution_EasyDifficulty
{
    #region TwoSum
    public int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int, int> kv = new Dictionary<int, int>();

        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (kv.TryGetValue(complement, out int index))
            {
                return new int[] { index, i };
            }
            if (!kv.ContainsKey(nums[i]))
                kv.Add(nums[i], i);
        }

        return new int[] { -1, -1 };
    }
    #endregion

    #region IsPalindrome Integer
    public bool IsPalindrome1(int x)
    {
        if (x < 0) return false;
        if (x < 10) return true;

        string s = x.ToString();

        int left = 0, right = s.Length - 1;
        while (left < right)
            if (s[left++] != s[right--]) return false;
        return true;
    }
    public bool IsPalindrome2(int x)
    {
        if (x < 0) return false;
        if (x < 10) return true;

        int reversed = 0, remainder = 0, original = x;

        while (x != 0)
        {
            remainder = x % 10;
            x /= 10;

            if (reversed > int.MaxValue / 10 ||
                (reversed == int.MaxValue / 10 && remainder > 7))
                return false;

            reversed = reversed * 10 + remainder;
        }

        return reversed == original;
    }
    public bool IsPalindrome3(int x)
    {
        if (x < 0 || (x % 10 == 0 && x != 0)) return false;
        if (x < 10) return true;

        int reversed = 0;

        while (x > reversed)
        {
            reversed = reversed * 10 + x % 10;
            x /= 10;
        }

        return reversed == x || reversed / 10 == x;
    }
    #endregion

    #region Roman To Integer
    public int RomanToInt(string s)//character by character approach with special case handling
    {
        /* So I am thinking that the special cases are that I, X and C can be placed before other
         * letters so I have to handle them with care
         * 
         * I will go character by character for my first approach
         */
        int result = 0;
        for (int i = 0; i < s.Length; i++)
        {
            switch (s[i])
            {
                case 'M':
                    result += 1000;
                    break;
                case 'D':
                    result += 500;
                    break;
                case 'C':
                    if (i + 1 < s.Length)
                    {
                        if (s[i + 1] == 'D')
                        {
                            result += 400;
                            i++;
                        }
                        else if (s[i + 1] == 'M')
                        {
                            result += 900;
                            i++;
                        }
                        else
                        {
                            result += 100;
                        }
                    }
                    else
                    {
                        result += 100;
                    }
                    break;
                case 'L':
                    result += 50;
                    break;
                case 'X':
                    if (i + 1 < s.Length)
                    {
                        if (s[i + 1] == 'L')
                        {
                            result += 40;
                            i++;
                        }
                        else if (s[i + 1] == 'C')
                        {
                            result += 90;
                            i++;
                        }
                        else
                        {
                            result += 10;
                        }
                    }
                    else
                    {
                        result += 10;
                    }
                    break;
                case 'V':
                    result += 5;
                    break;
                case 'I':
                    if (i + 1 < s.Length)
                    {
                        if (s[i + 1] == 'V')
                        {
                            result += 4;
                            i++;
                        }
                        else if (s[i + 1] == 'X')
                        {
                            result += 9;
                            i++;
                        }
                        else
                        {
                            result += 1;
                        }
                    }
                    else
                    {
                        result += 1;
                    }
                    break;
            }

        }
        return result;
    }

    public int RomanToInt2(string s)
    {
        /* after review: use a dictionary after review made me see that you can reduce the duplication
         * by looking at how to see subtractive pairs. The character before is less than the one after
         */
        Dictionary<char, int> valueSymbols = new Dictionary<char, int>(new[]
        {
            new KeyValuePair<char, int>('I', 1),
            new KeyValuePair<char, int>('V', 5),
            new KeyValuePair<char, int>('X', 10),
            new KeyValuePair<char, int>('L', 50),
            new KeyValuePair<char, int>('C', 100),
            new KeyValuePair<char, int>('D', 500),
            new KeyValuePair<char, int>('M', 1000),
        });

        int result = 0;
        int i = 0;
        while (i < s.Length - 1)
        {
            if (valueSymbols[s[i]] < valueSymbols[s[i + 1]])
            {
                result += valueSymbols[s[i + 1]] - valueSymbols[s[i]];
                i += 2;
            }
            else
            {
                result += valueSymbols[s[i]];
                i++;
            }
        }

        return i < s.Length ? result += valueSymbols[s[i]] : result;
    }//This performed poorer than the first one. I think that it's because of the dictionary maybe

    public int RomanToInt3(string s)//merging the two above
    {
        //Learned that a map does not have to be a dictionary. It's a concept

        int result = 0;
        int GetValue(char symbol) => symbol switch
        {
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            'M' => 1000,
            _ => 0
        };

        for (int i = 0; i < s.Length; i++)
        {
            int current = GetValue(s[i]);

            if (i + 1 < s.Length && current < GetValue(s[i + 1]))
            {
                result += GetValue(s[i + 1]) - current;
                i++;
            }
            else
            {
                result += current;
            }
        }

        return result;
    }
    #endregion

    #region Longest Common Prefix
    public string LongestCommonPrefix(string[] strs)//at first glance use a for loop while there's a match
    {
        StringBuilder result = new StringBuilder();

        int shortestLen = strs.Select(str => str.Length).Min();
        bool common = true;
        char current;

        for (int i = 0; i < shortestLen && common; i++)
        {
            current = strs[0][i];
            foreach (string str in strs)
            {
                if (str[i] != current)
                    return result.ToString();
            }
            result.Append(current);
        }

        return result.ToString();
    }//performed badly for me, I'm gonna now try to optimise

    public string LongestCommonPrefix2(string[] strs)//after submission of 1, got 8ms, beats 10.96%...trying to optimise
    {//done with hints from ChatGPT
        string prefix = strs[0];

        for (int i = 1; i < strs.Length; i++)
        {
            while (!strs[i].StartsWith(prefix))
            {
                prefix = prefix.Substring(0, prefix.Length - 1);

                if (prefix.Length == 0)
                    return "";
            }
        }

        return prefix;
    }
    #endregion

}
