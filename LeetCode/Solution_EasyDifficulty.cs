using System.Collections.Generic;

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

}
