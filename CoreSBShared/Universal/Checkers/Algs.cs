using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace InfrastructureCheckers
{
    public class Sortings
    {
        public int[] QuickSort(int[] arr)
        {
            if (arr == null || arr.Length <= 1)
                return arr;

            return toSort(arr, 0, arr.Length - 1);
        }

        public int[] toSort(int[] arr, int l, int r)
        {
            if (l >= r)
                return arr;

            int p = Partition(arr, l, r);

            toSort(arr, l, p - 1); // <-- use l instead of 0
            toSort(arr, p + 1, r);

            return arr;
        }

        public int Partition(int[] arr, int l, int r)
        {
            int p = arr[r]; // pivot
            int j = l - 1;

            for (int i = l; i < r; i++) // iterate only from l to r-1
            {
                if (arr[i] < p)
                {
                    j++;
                    (arr[j], arr[i]) = (arr[i], arr[j]); // swap
                }
            }

            j++;
            (arr[j], arr[r]) = (arr[r], arr[j]); // place pivot in correct position
            return j;
        }
    }

    public class Arr
    {
        public string ReverseString(string s)
        {
            // Convert string to char array
            char[] arr = s.ToCharArray();

            // Reverse array in place
            Array.Reverse(arr);

            // Construct new string from reversed array
            return new string(arr);
        }

        public string RevWithArr(string s)
        {
            int l = 0;
            int r = s.Length - 1; // last valid index
            char[] arr = s.ToCharArray();

            while (l < r)
            {
                (arr[l], arr[r]) = (arr[r], arr[l]);
                l++;
                r--;
            }

            return new string(arr);
        }

        public bool IsPalindrome(string s)
        {
            int l = 0, r = s.Length - 1;
            while (l < r)
            {
                if (s[l] != s[r]) return false;
                l++;
                r--;
            }

            return true;
        }

        public bool IsAnagram(string s1, string s2)
        {
            if (s1 == null || s2 == null) return false;
            if (s1.Length != s2.Length) return false;

            var count = new Dictionary<char, int>();

            foreach (var c in s1)
            {
                if (count.ContainsKey(c))
                    count[c]++;
                else
                    count[c] = 1;
            }

            foreach (var c in s2)
            {
                if (!count.ContainsKey(c)) return false;
                count[c]--;
                if (count[c] < 0) return false;
            }

            return true;
        }
    }

    public class SubStrings
    {
        public int LengthOfLongestSubstring(string s)
        {
            var set = new HashSet<char>();
            int l = 0, maxLen = 0;
            for (int r = 0; r < s.Length; r++)
            {
                while (set.Contains(s[r]))
                {
                    set.Remove(s[l]);
                    l++;
                }

                set.Add(s[r]);
                maxLen = Math.Max(maxLen, r - l + 1);
            }

            return maxLen;
        }
    }

    public class HashMaps
    {
        public Dictionary<char, int> CharFreq(string s)
        {
            var dict = new Dictionary<char, int>();
            foreach (var c in s)
            {
                // initialize
                if (!dict.ContainsKey(c)) dict[c] = 0;
                // set
                dict[c]++;
            }

            return dict;
        }
    }
}
