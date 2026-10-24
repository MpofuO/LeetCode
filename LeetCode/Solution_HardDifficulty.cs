using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode;

internal class Solution_HardDifficulty
{
    #region FindMedianSortedArrays
    public double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        //I think merging the arrays then returning the median afterwards should work.
        int mergedlen = nums1.Length + nums2.Length;
        int[] merged = new int[mergedlen];
        int idx1 = 0, idx2 = 0;

        for (int i = 0; i < mergedlen; i++)
        {
            int next;
            if (idx1 >= nums1.Length)
                next = nums2[idx2++];
            else if (idx2 >= nums2.Length)
                next = nums1[idx1++];
            else
                next = nums1[idx1] <= nums2[idx2] ? nums1[idx1++] : nums2[idx2++];

            merged[i] = next;
        }

        int midIdx = mergedlen / 2;
        double median = mergedlen % 2 == 0 ? (merged[midIdx] + merged[midIdx - 1]) / 2.0 : merged[midIdx];

        return median;
    }

    public double FindMedianSortedArrays2(int[] nums1, int[] nums2)//After review: I don't have to run until the end
    {
        int totalLength = nums1.Length + nums2.Length;
        int midIdx = totalLength / 2;

        int idx1 = 0, idx2 = 0;
        int previous = 0, current = 0;

        for (int i = 0; i <= midIdx; i++)
        {
            previous = current;

            if (idx1 >= nums1.Length)
                current = nums2[idx2++];
            else if (idx2 >= nums2.Length)
                current = nums1[idx1++];
            else
                current = nums1[idx1] <= nums2[idx2] ? nums1[idx1++] : nums2[idx2++];
        }

        return totalLength % 2 == 0 ? ((double)previous + current) / 2 //cast for overflow safety
                                    : current;
    }
    #endregion

    #region Regular Expression Matching
    public bool IsMatch(string s, string p)
    {
        return default;
    }
    #endregion
}
