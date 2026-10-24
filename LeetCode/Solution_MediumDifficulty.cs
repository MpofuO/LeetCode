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

}
