using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode;

#region Classes Given
public class ListNode //For Add Two Numbers
{
    public int val;
    public ListNode next;
    public ListNode(int val = 0, ListNode next = null)
    {
        this.val = val;
        this.next = next;
    }
}
#endregion
internal class Solution_MediumDifficulty
{
    #region AddtwoNumbers
    public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
    {
        ListNode result = new ListNode();
        ListNode current_l1 = l1;
        ListNode current_l2 = l2;
        ListNode current_result = result;

        int carryOver = 0;
        while (current_l1 != null || current_l2 != null)
        {
            int currentSum = (current_l1?.val ?? 0) + (current_l2?.val ?? 0) + carryOver;

            carryOver = currentSum / 10;

            current_result.next = new ListNode(currentSum % 10);
            current_result = current_result.next;

            current_l1 = current_l1?.next;
            current_l2 = current_l2?.next;
        }
        if (carryOver > 0)
            current_result.next = new ListNode(carryOver);

        return result.next;
    }
    #endregion

    #region LengthOfLongestSubstring
    public int LengthOfLongestSubstring1(string s)
    {
        if (s.Length <= 1)
            return s.Length;

        Dictionary<char, int> substringDict = new Dictionary<char, int>();
        int longest = 0, left = 0, right = 0;

        while (right < s.Length)
        {
            if (!substringDict.TryAdd(s[right], right))
            {
                longest = Math.Max(longest, right - left + 1);
                left = substringDict[s[right]];//+ 1
            }
            right++;
        }

        return longest;
    }
    public int LengthOfLongestSubstring(string s)//after review of initial code
    {
        Dictionary<char, int> substringDict = new Dictionary<char, int>();
        int longest = 0, left = 0;

        for (int right = 0; right < s.Length; right++)
        {
            if (substringDict.TryGetValue(s[right], out int previousIndex))
            {
                left = Math.Max(left, previousIndex + 1);
            }

            substringDict[s[right]] = right;
            longest = Math.Max(longest, right - left + 1);
        }

        return longest;
    }
    #endregion

    #region LongestPalindrome
    public string LongestPalindrome1(string s)
    {
        int longestlen = 0, start = 0, end = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int substringlen;
            for (int j = i; j < s.Length; j++)
            {
                substringlen = j - i + 1;
                if (IsPalindrome(s, i, j) && substringlen > longestlen)
                {
                    start = i; end = j;
                    longestlen = substringlen;
                }
            }
        }

        return s.Substring(start, end - start + 1);
    }

    private bool IsPalindrome(string s, int left, int right)
    {
        while (left < right)
        {
            if (s[left++] != s[right--])
                return false;
        }
        return true;
    }

    public string LongestPalindrome(string s)//second version after multiple reviews: learned about local functions
    {
        if (s.Length <= 1)
            return s;

        int longestlen = 0, start = 0, end = 0;

        for (int i = 0; i < s.Length; i++)
        {
            (int left, int right) = LongestPalindromeIndexes(s, i, i);
            UpdateLongestLen(left, right);

            (left, right) = LongestPalindromeIndexes(s, i, i + 1);
            UpdateLongestLen(left, right);
        }

        return s.Substring(start, end - start + 1);

        void UpdateLongestLen(int left, int right)
        {
            int substringlen = right - left + 1;

            if (substringlen > longestlen)
            {
                longestlen = substringlen;
                start = left;
                end = right;
            }
        }
    }

    private (int, int) LongestPalindromeIndexes(string s, int left, int right)
    {
        while (left >= 0 && right < s.Length)
        {
            if (s[left] != s[right])
                break;

            left--;
            right++;
        }

        return (left + 1, right - 1);
    }
    #endregion

    #region Convert ZigZag
    public string Convert(string s, int numRows)
    {
        if (numRows <= 1)
            return s;

        //I am thinking two loops that fill the array lists down and back up should work.
        List<char>[] arr = new List<char>[numRows];

        for (int i = 0; i < numRows; i++)
        {
            arr[i] = new List<char>();
        }

        int insertIdx = 0, direction = 1;//which list to add to and direction is for going down or up (-1)

        for (int i = 0; i < s.Length; i++)
        {
            arr[insertIdx].Add(s[i]);
            insertIdx += direction;

            if (insertIdx == numRows || insertIdx < 0)
            {
                insertIdx -= direction * 2;
                direction *= -1;
            }

        }

        string zigzag = "";
        foreach (List<char> row in arr)
        {
            zigzag += string.Join("", row);
        }

        return zigzag;
    }
    public string Convert2(string s, int numRows)//Use string builder when working with strings in a loop
    {
        if (numRows <= 1)
            return s;

        StringBuilder[] arr = new StringBuilder[numRows];

        for (int i = 0; i < numRows; i++)
        {
            arr[i] = new StringBuilder();
        }

        int insertIdx = 0, direction = 1;//which list to add to and direction is for going down or up (-1)

        for (int i = 0; i < s.Length; i++)
        {
            arr[insertIdx].Append(s[i]);
            insertIdx += direction;

            if (insertIdx == numRows || insertIdx < 0)
            {
                insertIdx -= direction * 2;
                direction *= -1;
            }

        }

        StringBuilder zigzag = new StringBuilder();

        foreach (StringBuilder row in arr)
        {
            zigzag.Append(row);
        }

        return zigzag.ToString();
    }
    #endregion

    #region Reverse Integer
    public int Reverse1(int x)
    {
        int pos = 0;//position of digit counting from the end
        int multiplier = x < 0 ? -1 : 1;
        int digitDivident, divident, quotient, remainder;
        SetQuotientAndRemainder();

        StringBuilder s = new StringBuilder(multiplier < 0 ? "-" : "");

        while (quotient != 0)
        {
            s.Append(remainder);
            SetQuotientAndRemainder();
        }

        s.Append(remainder);

        return int.Parse(s.ToString());

        void SetQuotientAndRemainder()
        {
            digitDivident = (int)Math.Pow(10, pos++);
            divident = (int)Math.Pow(10, pos);
            quotient = multiplier * x / divident;
            remainder = ((multiplier * x) % divident) / digitDivident;
        }
    }
    public int Reverse2(int x)
    {
        StringBuilder s = new StringBuilder(x < 0 ? "-" : "");
        x = x * int.Sign(x);
        int remainder = x % 10;

        while (x != 0)
        {
            s.Append(remainder);
            x /= 10;
            remainder = x % 10;
        }

        return int.Parse(s.ToString());
    }

    public int Reverse3(int x)
    {
        int reversed = 0, remainder = 0;

        while (x != 0)
        {
            remainder = x % 10;
            x /= 10;

            if (reversed > int.MaxValue / 10 ||
                (reversed == int.MaxValue / 10 && remainder > 7))
                return 0;

            if (reversed < int.MinValue / 10 ||
                (reversed == int.MinValue / 10 && remainder < -8))
                return 0;

            reversed = reversed * 10 + remainder;
        }

        return reversed;
    }
    #endregion

    #region String to Integer (atoi)
    public int MyAtoi(string s)
    {
        long number = 0, numberSign = 1;

        char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char[] signs = { '+', '-' };

        bool stopLoop = false, startedNumber = false;
        char current;
        for (int i = 0; i < s.Length && !stopLoop; i++)
        {
            current = s[i];
            int digitIndex = Array.IndexOf(digits, current);
            if (digitIndex >= 0)
            {
                startedNumber = true;
                number = number * 10 + digitIndex;
                if (number > int.MaxValue)
                {
                    number = (long)int.MinValue * -1;
                    break;
                }
            }
            else if (startedNumber)
            {
                stopLoop = true;
            }
            else if (Array.IndexOf(signs, current) is int sign && sign >= 0)
            {
                numberSign -= 2 * sign;
                startedNumber = true;
            }
            else if (current != ' ')
                stopLoop = true;
        }
        if (-number == int.MinValue)
            return numberSign < 0 ? int.MinValue : int.MaxValue;

        number *= numberSign;
        return (int)number;
    }

    public int MyAtoi2(string s)//After review
    {
        int i = 0;
        int sign = 1;
        long number = 0;

        while (i < s.Length && s[i] == ' ')
            i++;

        if (i < s.Length && (s[i] == '+' || s[i] == '-'))
        {
            sign = s[i] == '-' ? -1 : 1;
            i++;
        }

        while (i < s.Length && s[i] >= '0' && s[i] <= '9')//I think this was one important lesson, the relationship between char and int in this sense
        {
            int digit = s[i] - '0';
            number = number * 10 + digit;

            if (sign == 1 && number > int.MaxValue)
                return int.MaxValue;

            if (sign == -1 && number > 2147483648L)
                return int.MinValue;

            i++;
        }

        return (int)(number * sign);
    }
    #endregion

    #region Container With Most Water
    public int MaxArea(int[] height)//I'll start with the most basic solution first, a double loop
    {
        int maxArea = 0;
        for (int i = 0; i < height.Length; i++)
        {
            for (int j = i + 1; j < height.Length; j++)
            {
                int currentArea = Math.Min(height[i], height[j]) * (j - i);
                maxArea = Math.Max(maxArea, currentArea);
            }
        }

        return maxArea;
    }

    public int MaxArea2(int[] height)//Done before reviewing with ChatGPT, reducing to two pointers
    {
        int maxArea = 0;
        int left = 0, right = height.Length - 1;

        while (left + 1 < right - 1)
        {
            maxArea = Math.Max(maxArea, getArea(height[left], height[left + 1], 1));
            maxArea = Math.Max(maxArea, getArea(height[left], height[left + 2], 2));
            maxArea = Math.Max(maxArea, getArea(height[right], height[right - 1], 1));
            maxArea = Math.Max(maxArea, getArea(height[right], height[right - 2], 2));
            maxArea = Math.Max(maxArea, getArea(height[left], height[right], right - left));

            left++; right--;
        }

        return Math.Max(maxArea, getArea(height[left], height[right], right - left));
    }
    private int getArea(int h1, int h2, int w) => Math.Min(h1, h2) * w;

    public int MaxArea3(int[] height)//after review, realised that I am still missing some heights
    {
        int maxArea = 0;
        int left = 0, right = height.Length - 1;

        while (left < right)
        {
            maxArea = Math.Max(maxArea, getArea(height[left], height[right], right - left));
            if (height[left] <= height[right])
                left++;
            else right--;
        }

        return maxArea;
    }
    #endregion

    #region Integer to Roman
    public string IntToRoman(int num)//First approach
    {
        StringBuilder sb = new StringBuilder();
        Dictionary<int, char> valueSymbols = new Dictionary<int, char>(new[]
        {
            new KeyValuePair<int, char>(1, 'I'),
            new KeyValuePair<int, char>(5, 'V'),
            new KeyValuePair<int, char>(10, 'X'),
            new KeyValuePair<int, char>(50, 'L'),
            new KeyValuePair<int, char>(100, 'C'),
            new KeyValuePair<int, char>(500, 'D'),
            new KeyValuePair<int, char>(1000, 'M'),
        });

        for (int i = valueSymbols.Keys.Count; i > 0; i--)
        {
            KeyValuePair<int, char> valueSymbol = valueSymbols.ElementAt(i);
            int symbolRepeats = num / valueSymbol.Key;

            if (symbolRepeats < 4)
            {
                sb.Append(valueSymbol.Value, symbolRepeats);
            }
            else if (symbolRepeats == 4)
            {
                sb.Append(valueSymbol.Value + valueSymbols[5 * valueSymbol.Key]);
            }
            else if (symbolRepeats == 9)
            {
                //sb.Append()//Got stuck realising that I might not even ever hit this case. Went for review with ChatGPT
            }

            num %= valueSymbol.Value * symbolRepeats;
        }

        return sb.ToString();
    }

    public string IntToRoman2(int num)//After review. Instead of building the edge cases rather add then to the dictionary
    {
        StringBuilder sb = new StringBuilder();
        Dictionary<int, string> valueSymbols = new Dictionary<int, string>(new[]
        {
            new KeyValuePair<int, string>(1000, "M"),
            new KeyValuePair<int, string>(900, "CM"),
            new KeyValuePair<int, string>(500, "D"),
            new KeyValuePair<int, string>(400, "CD"),
            new KeyValuePair<int, string>(100, "C"),
            new KeyValuePair<int, string>(90, "XC"),
            new KeyValuePair<int, string>(50, "L"),
            new KeyValuePair<int, string>(40, "XL"),
            new KeyValuePair<int, string>(10, "X"),
            new KeyValuePair<int, string>(9, "IX"),
            new KeyValuePair<int, string>(5, "V"),
            new KeyValuePair<int, string>(4, "IV"),
            new KeyValuePair<int, string>(1, "I"),
        });

        KeyValuePair<int, string> valueSymbol;
        for (int i = 0; i < valueSymbols.Count; i++)
        {
            valueSymbol = valueSymbols.ElementAt(i);

            while (num >= valueSymbol.Key)
            {
                sb.Append(valueSymbol.Value);
                num -= valueSymbol.Key;
            }
        }

        return sb.ToString();
    }
    public string IntToRoman3(int num)//Better that 2 mainly because of the foreach loop. I did not need indexes
    {
        StringBuilder sb = new StringBuilder();
        Dictionary<int, string> valueSymbols = new Dictionary<int, string>(new[]
        {
            new KeyValuePair<int, string>(1000, "M"),
            new KeyValuePair<int, string>(900, "CM"),
            new KeyValuePair<int, string>(500, "D"),
            new KeyValuePair<int, string>(400, "CD"),
            new KeyValuePair<int, string>(100, "C"),
            new KeyValuePair<int, string>(90, "XC"),
            new KeyValuePair<int, string>(50, "L"),
            new KeyValuePair<int, string>(40, "XL"),
            new KeyValuePair<int, string>(10, "X"),
            new KeyValuePair<int, string>(9, "IX"),
            new KeyValuePair<int, string>(5, "V"),
            new KeyValuePair<int, string>(4, "IV"),
            new KeyValuePair<int, string>(1, "I"),
        });

        foreach (KeyValuePair<int, string> valueSymbol in valueSymbols)
        {
            while (num >= valueSymbol.Key)
            {
                sb.Append(valueSymbol.Value);
                num -= valueSymbol.Key;
            }
        }

        return sb.ToString();
    }
    public string IntToRoman4(int num)//switched to a dictionary since I don't use/need lookups anywhere
    {
        StringBuilder sb = new StringBuilder();
        KeyValuePair<int, string>[] valueSymbols =
        {
            new KeyValuePair<int, string>(1000, "M"),
            new KeyValuePair<int, string>(900, "CM"),
            new KeyValuePair<int, string>(500, "D"),
            new KeyValuePair<int, string>(400, "CD"),
            new KeyValuePair<int, string>(100, "C"),
            new KeyValuePair<int, string>(90, "XC"),
            new KeyValuePair<int, string>(50, "L"),
            new KeyValuePair<int, string>(40, "XL"),
            new KeyValuePair<int, string>(10, "X"),
            new KeyValuePair<int, string>(9, "IX"),
            new KeyValuePair<int, string>(5, "V"),
            new KeyValuePair<int, string>(4, "IV"),
            new KeyValuePair<int, string>(1, "I"),
        };

        foreach (KeyValuePair<int, string> valueSymbol in valueSymbols)
        {
            char value;
            if (char.TryParse(valueSymbol.Value, out value))
            {
                sb.Append(value, num / valueSymbol.Key);
                num %= valueSymbol.Key;
            }
            else
            {
                while (num >= valueSymbol.Key)
                {
                    sb.Append(valueSymbol.Value);
                    num -= valueSymbol.Key;
                }
            }
        }

        return sb.ToString();
    }
    public string IntToRoman5(int num)
    {
        StringBuilder sb = new StringBuilder();
        KeyValuePair<int, string>[] valueSymbols =
        {
            new KeyValuePair<int, string>(1000, "M"),
            new KeyValuePair<int, string>(900, "CM"),
            new KeyValuePair<int, string>(500, "D"),
            new KeyValuePair<int, string>(400, "CD"),
            new KeyValuePair<int, string>(100, "C"),
            new KeyValuePair<int, string>(90, "XC"),
            new KeyValuePair<int, string>(50, "L"),
            new KeyValuePair<int, string>(40, "XL"),
            new KeyValuePair<int, string>(10, "X"),
            new KeyValuePair<int, string>(9, "IX"),
            new KeyValuePair<int, string>(5, "V"),
            new KeyValuePair<int, string>(4, "IV"),
            new KeyValuePair<int, string>(1, "I"),
        };

        foreach (KeyValuePair<int, string> valueSymbol in valueSymbols)
        {
            int count = num / valueSymbol.Key;

            for (int i = 0; i < count; i++)
            {
                sb.Append(valueSymbol.Value);
            }

            num %= valueSymbol.Key;
        }

        return sb.ToString();
    }
    #endregion

    #region 3Sum
    public IList<IList<int>> ThreeSum(int[] nums)//trying the first solution that I think works:nested loops
    {
        IList<IList<int>> result = new List<IList<int>>();

        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = i + 1; j < nums.Length; j++)
            {
                for (int k = j + 1; k < nums.Length; k++)
                {
                    if (nums[i] + nums[j] + nums[k] == 0)
                    {
                        if (!result.Any(l => l.Contains(nums[i]) && l.Contains(nums[j])))
                            result.Add(new List<int>() { nums[i], nums[j], nums[k] });
                    }

                }
            }
        }

        return result;
    }

    public IList<IList<int>> ThreeSum2(int[] nums)//after failed submission
    {
        List<IList<int>> result = new List<IList<int>>();

        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = i + 1; j < nums.Length; j++)
            {
                for (int k = j + 1; k < nums.Length; k++)
                {
                    if (nums[i] + nums[j] + nums[k] == 0)
                    {
                        if (!result.Any(l => l.Contains(nums[i]) && l.Contains(nums[j])))
                        {
                            List<int> list = new List<int>() { nums[i], nums[j], nums[k] };
                            list.Sort();
                            result.Add(list);
                        }
                    }

                }
            }
        }
        result = result.OrderBy(l => l[0]).ToList();
        return result;
    }//kept failing again:
    /*Output
     * [[-5,0,5],[-4,2,2],[-3,1,2],[-3,-2,5],[-2,0,2]]
     * Expected
     * [[-10,5,5],[-5,0,5],[-4,2,2],[-3,-2,5],[-3,1,2],[-2,0,2]]
     * */

    public IList<IList<int>> ThreeSum3(int[] nums)//perhaps sorting before I start traversing might be helpful
    {
        Array.Sort(nums);
        IList<IList<int>> result = new List<IList<int>>();

        for (int i = 0; i < nums.Length; i++)
            for (int j = i + 1; j < nums.Length; j++)
                for (int k = j + 1; k < nums.Length; k++)
                    if (nums[i] + nums[j] + nums[k] == 0)
                        if (!result.Any(l => l[0] == nums[i] && l[1] == nums[j]))
                            result.Add(new List<int>() { nums[i], nums[j], nums[k] });
        return result;
    }//Ohkay now some progress, time limit exceeded but 311/316 testcases passed

    public IList<IList<int>> ThreeSum4(int[] nums)//I think sorting also introduces an opening to use two-pointers
    {
        Array.Sort(nums);
        IList<IList<int>> result = new List<IList<int>>();

        for (int i = 0; i < nums.Length - 2 && nums[i] <= 0; i++)
        {
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            int left = i + 1, right = nums.Length - 1;
            while (left < right)
            {
                int sum = nums[i] + nums[left] + nums[right];
                if (sum > 0)//then I need a smaller number
                    right--;
                else if (sum < 0)//bigger number needed
                    left++;
                else
                {
                    result.Add(new List<int>() { nums[i], nums[left], nums[right] });
                    left++; right--;
                    while (left < right && nums[left] == nums[left - 1])//need to skip duplicates
                        left++;
                    while (left < right && nums[right] == nums[right + 1])
                        right--;
                }
            }
        }
        return result;
    }
    #endregion

    #region 3Sum Closest
    public int ThreeSumClosest(int[] nums, int target)//should be very similar to 3Sum
    {
        Array.Sort(nums);
        int closest = nums[0] + nums[1] + nums[2];

        for (int i = 0; i < nums.Length - 2; i++)
        {
            int currentFixed = nums[i];

            int left = i + 1, right = nums.Length - 1;
            while (left < right)
            {
                int sum = currentFixed + nums[left] + nums[right];
                if (Math.Abs(target - sum) < Math.Abs(target - closest))
                    closest = sum;

                if (closest == target)
                    return closest;
                else if (closest < target)
                    while (left < right && nums[left] == nums[left - 1])
                        left++;
                else
                    while (left < right && nums[right] == nums[right + 1])
                        right--;
            }
        }
        return closest;
    }//Time limit exceeded

    public int ThreeSumClosest2(int[] nums, int target)
    {
        Array.Sort(nums);
        int closest = nums[0] + nums[1] + nums[3];

        for (int i = 0; i < nums.Length - 2; i++)
        {
            int currentFixed = nums[i];

            int left = i + 1, right = nums.Length - 1;
            while (left < right)
            {
                int sum = currentFixed + nums[left] + nums[right];
                if (Math.Abs(target - sum) < Math.Abs(target - closest))
                    closest = sum;

                if (closest == target)
                    return closest;

                if (sum < target)//also check again current sum not best
                {
                    left++;//need to at least move once then check for duplicates
                    while (left < right && nums[left] == nums[left - 1])
                        left++;
                }
                else
                {
                    right--;
                    while (left < right && nums[right] == nums[right + 1])
                        right--;
                }
            }
        }
        return closest;
    }//Accepted: 16ms beats 41.47%

    public int ThreeSumClosest3(int[] nums, int target)//final attempt to improve speed and memory
    {
        Array.Sort(nums);
        int closest = nums[0] + nums[1] + nums[2];

        for (int i = 0; i < nums.Length - 2; i++)
        {
            int currentFixed = nums[i];

            //Maximum possible sum for this fixed number
            int maxSum = currentFixed + nums[nums.Length - 2] + nums[nums.Length - 1];

            //Future fixed numbers cannot produce a better result
            if (maxSum > target && target - maxSum > Math.Abs(target - closest))
                break;

            int left = i + 1;
            int right = nums.Length - 1;

            while (left < right)
            {
                int sum = currentFixed + nums[left] + nums[right];

                if (Math.Abs(target - sum) < Math.Abs(target - closest))
                    closest = sum;

                if (closest == target)
                    return closest;

                if (sum < target)
                    left++;
                else
                    right--;
            }
        }

        return closest;
    }
    #endregion

    #region Letter Combinations of a Phone number
    public IList<string> LetterCombinations(string digits)//at first glance I thought nested loops, but how many
    {//So I thought recursion
        Dictionary<char, string[]> numberLetters = new(
            [
                new KeyValuePair<char, string[]>('2', ["a", "b", "c"]),
                new KeyValuePair<char, string[]>('3', ["d", "e", "f"]),
                new KeyValuePair<char, string[]>('4', ["g", "h", "i"]),
                new KeyValuePair<char, string[]>('5', ["j", "k", "l"]),
                new KeyValuePair<char, string[]>('6', ["m", "n", "o"]),
                new KeyValuePair<char, string[]>('7', ["p", "q", "r", "s"]),
                new KeyValuePair<char, string[]>('8', ["t", "u", "v"]),
                new KeyValuePair<char, string[]>('9', ["w", "x", "y", "z"]),
            ]);

        IList<StringBuilder> results = new List<StringBuilder>();
        //Could not come up with the best recursive approach so I went with Appending the next part of the combinations over and over again
        IList<string[]> combinedLists = new List<string[]>();
        for (int i = 0; i < digits.Length; i++)
            combinedLists.Add(numberLetters[digits[i]]);

        foreach (string letter in combinedLists[0])
            results.Add(new StringBuilder(letter));

        for (int i = 1; i < combinedLists.Count; i++)
            results = CrossProduct(results, combinedLists[i]);

        return results.Select(sb => sb.ToString()).ToList();
    }
    private IList<StringBuilder> CrossProduct(IList<StringBuilder> currentResult, string[] addition)
    {
        IList<StringBuilder> newResult = new List<StringBuilder>();

        foreach (StringBuilder sb in currentResult)
            foreach (string s in addition)
                newResult.Add(sb.Append(s));

        return newResult;
    }

    //Second approach. StringBuilder being mutable seems to fail. I'm gonna try first with a string
    public IList<string> LetterCombinations2(string digits)
    {
        Dictionary<char, string[]> numberLetters = new(
            [
                new KeyValuePair<char, string[]>('2', ["a", "b", "c"]),
                    new KeyValuePair<char, string[]>('3', ["d", "e", "f"]),
                    new KeyValuePair<char, string[]>('4', ["g", "h", "i"]),
                    new KeyValuePair<char, string[]>('5', ["j", "k", "l"]),
                    new KeyValuePair<char, string[]>('6', ["m", "n", "o"]),
                    new KeyValuePair<char, string[]>('7', ["p", "q", "r", "s"]),
                    new KeyValuePair<char, string[]>('8', ["t", "u", "v"]),
                    new KeyValuePair<char, string[]>('9', ["w", "x", "y", "z"]),
                ]);

        IList<string> results = new List<string>();

        IList<string[]> combinedLists = new List<string[]>();
        for (int i = 0; i < digits.Length; i++)
            combinedLists.Add(numberLetters[digits[i]]);

        foreach (string letter in combinedLists[0])
            results.Add(letter);

        for (int i = 1; i < combinedLists.Count; i++)
            results = CrossProduct(results, combinedLists[i]);

        return results;
    }
    private IList<string> CrossProduct(IList<string> currentResult, string[] addition)
    {
        IList<string> newResult = new List<string>();
        StringBuilder sb = new StringBuilder();
        foreach (string s1 in currentResult)
        {
            sb.Append(s1);
            foreach (string s2 in addition)
            {
                newResult.Add(sb.Append(s2).ToString());
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Clear();
        }

        return newResult;
    }//Ran and Accepted but went to review with chatgpt to see if I can get my initial recursion idea

    public IList<string> LetterCombinations3(string digits)
    {
        Dictionary<char, string> numberLetters = new()
        {
            ['2'] = "abc",
            ['3'] = "def",
            ['4'] = "ghi",
            ['5'] = "jkl",
            ['6'] = "mno",
            ['7'] = "pqrs",
            ['8'] = "tuv",
            ['9'] = "wxyz"
        };

        IList<string> results = new List<string>();
        StringBuilder current = new();

        void BuildCombinations(int position)
        {
            if (position == digits.Length)
            {
                results.Add(current.ToString());
                return;
            }

            foreach (char letter in numberLetters[digits[position]])
            {
                current.Append(letter);
                BuildCombinations(position + 1);
                current.Remove(current.Length - 1, 1);
            }
        }

        BuildCombinations(0);
        return results;
    }
    #endregion

    #region 4Sum
    public IList<IList<int>> FourSum(int[] nums, int target)//First I will treat this almost like 3Sum, but have 4 pointers
    {
        IList<IList<int>> result = [];
        int length = nums.Length;
        if (length < 4) return result;
        //I don't need indexes so I'll start with sorting and work through a sorted array
        Array.Sort(nums);
        int a = 0, b = length / 2 - 1, c = length / 2, d = length - 1;

        while (a < b && c < d)
        {
            int sum = nums[a] + nums[b] + nums[c] + nums[d];
            if (sum == target)
            {
                result.Add([nums[a++], nums[b--], nums[c++], nums[d--]]);
            }
            if (sum < target)//then either a or c need to move up
            {
                if (target - sum <= nums[length / 2])
                    a++;
                else
                    c++;
            }
            else//either b or d should move down
            {
                if (sum - target <= nums[length / 2])
                    d--;
                else
                    b--;
            }
        }


        return result;
    }

    public IList<IList<int>> FourSum2(int[] nums, int target)//approach 2: ThreeSum nested in FourSum
    {
        Array.Sort(nums);
        IList<IList<int>> results = [];
        int length = nums.Length;

        if (length < 4) return results;

        for (int i = 0; i < length - 3; i++)
        {
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            for (int j = i + 1; j < length - 2; j++)
            {
                if (j > i + 1 && nums[j] == nums[j - 1]) continue;

                int remainingToTarget = target - nums[i] - nums[j];
                int l = j + 1, r = length - 1;

                while (l < r)
                {
                    int sum = nums[l] + nums[r];

                    if (sum < remainingToTarget)
                        l++;
                    else if (sum > remainingToTarget)
                        r--;
                    else
                    {
                        results.Add([nums[i], nums[j], nums[l], nums[r]]);

                        l++;
                        r--;

                        while (l < r && nums[l] == nums[l - 1]) l++;
                        while (l < r && nums[r] == nums[r + 1]) r--;
                    }
                }
            }
        }

        return results;
    }

    public IList<IList<int>> FourSum3(int[] nums, int target)//handle overflow
    {
        Array.Sort(nums);
        IList<IList<int>> results = [];
        int length = nums.Length;

        if (length < 4) return results;

        for (int i = 0; i < length - 3; i++)
        {
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            for (int j = i + 1; j < length - 2; j++)
            {
                if (j > i + 1 && nums[j] == nums[j - 1]) continue;

                long remainingToTarget = (long)target - nums[i] - nums[j];
                int l = j + 1, r = length - 1;

                while (l < r)
                {
                    long sum = (long)nums[l] + nums[r];

                    if (sum < remainingToTarget)
                        l++;
                    else if (sum > remainingToTarget)
                        r--;
                    else
                    {
                        results.Add([nums[i], nums[j], nums[l], nums[r]]);

                        l++;
                        r--;

                        while (l < r && nums[l] == nums[l - 1]) l++;
                        while (l < r && nums[r] == nums[r + 1]) r--;
                    }
                }
            }
        }

        return results;
    }
    #endregion

    #region Remove Nth Node From End of List
    public ListNode RemoveNthFromEnd(ListNode head, int n)//Seems basic enough
    {//We will still return head but with the skipped node.
        //Firstly I have to get to the end then come back, I don't have previous though
        List<ListNode> listNodes = [];
        //Let's get to the end first. Realised I need to know how to come nack so I use a list
        ListNode current = head;
        listNodes.Add(current);
        while (current.next != null)
        {
            current = current.next;
            listNodes.Add(current);
        }

        int indexToRemove = listNodes.Count - n;
        if (indexToRemove == 0)
            head = head.next;
        else if (indexToRemove == listNodes.Count - 1)
            listNodes[indexToRemove - 1].next = null!;
        else
            listNodes[indexToRemove - 1].next = listNodes[indexToRemove + 1];

        return head;
    }//0ms Beats 100% on this first approach...very proud of this
    #endregion

    #region Valid Parenthesis
    public bool IsValid(string s)
    {//I think this is similar to the parenthesis method I know but just needs modification to cater for three different ones
        if (s.Length % 2 == 1)
            return false;

        Stack<char> stk = [];
        char getClosingBracket(char c)
        {
            return c switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                _ => ' '
            };
        }
        string closingBrackets = ")]}";
        for (int i = 0; i < s.Length; i++)
        {
            char current = s[i];
            if (closingBrackets.Contains(current))
            {
                if (stk.Count == 0 || getClosingBracket(stk.Pop()) != current)
                    return false;
            }
            else
                stk.Push(current);
        }

        return stk.Count == 0;
    }//6ms Beats 29.50%...needs improvement. Going for review

    public bool IsValid2(string s)//After review: translate at push time instead of pop time
    {
        if (s.Length % 2 == 1)
            return false;

        Stack<char> stk = [];//track the owed brackets now
        char getClosingBracket(char c)
        {
            return c switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                _ => ' '
            };
        }
        string openingBrackets = "([{";
        for (int i = 0; i < s.Length; i++)
        {
            if (stk.Count > s.Length - i) return false;//early exit. There's no way it can still be valid

            char current = s[i];
            if (openingBrackets.Contains(current))
            {
                stk.Push(getClosingBracket(current));
            }
            else
            {
                if (stk.Count == 0 || stk.Pop() != current)
                    return false;
            }
        }

        return stk.Count == 0;
    }//2ms beats 93% I think maybe I can settle for that
    #endregion
}