# LeetCode Learnings

This document records reusable patterns, lessons from my attempts, code-quality improvements, and algorithmic techniques learned while solving LeetCode problems in C#.

The goal is not simply to collect accepted solutions. It is to understand why solutions work, recognise patterns in unfamiliar problems, and become better at solving problems independently.

---

## 1. Two Sum

### Pattern
Use a dictionary to find a complement in constant average time.

### Key learnings

- For each number, calculate its complement: `target - current`.
- Check whether the complement exists in the dictionary before adding the current number.
- A dictionary can map a number to its index, allowing the solution to return the required indices.
- `TryGetValue` can combine checking for a key and retrieving its value in one operation.
- Think about whether the current element should be available as its own complement. Checking before inserting it avoids accidentally using the same element twice.

### Complexity

- Time: **O(n)** average.
- Extra space: **O(n)**.

---

## 2. Add Two Numbers

### Pattern
Traverse two linked lists while maintaining a carry.

### Key learnings

- Use a dummy head node to simplify linked-list construction. The actual result starts at `dummy.next`.
- Keep a pointer to the tail so each new digit can be appended efficiently.
- Continue while either list has digits remaining or a carry still needs to be processed.
- If one list is shorter than the other, treat its missing digit as `0`.
- Calculate the digit with `sum % 10` and the carry with `sum / 10`.
- Remember to handle a final carry after both lists have ended.

### Complexity

Let $n$ and $m$ be the lengths of the two lists.

- Time: **O(max(n, m))**.
- Extra space: **O(max(n, m))** for the result, excluding the input lists.

---

## 3. Longest Substring Without Repeating Characters

### Pattern
Sliding window with a dictionary that tracks the latest index of each character.

### Key learnings

- Maintain a window using `left` and `right` pointers.
- A dictionary can store the most recent index where each character appeared.
- When a repeated character appears inside the current window, move `left` past its previous occurrence.
- The left pointer must never move backwards. Use `Math.Max` when updating it:

  ```csharp
  left = Math.Max(left, lastSeen[s[right]] + 1);
  ```

- Calculate the current window length from the boundaries:

  ```csharp
  int currentLength = right - left + 1;
  ```
- You do not necessarily need to remove old dictionary entries when the window moves. An old entry can remain as long as you check whether its index is still inside the current window.

### Complexity

- Time: **O(n)** average.
- Extra space: **O(min(n, character set size))**.

---

## 4. Median of Two Sorted Arrays

### Pattern
Merge two sorted sequences using two pointers, stopping when the middle is reached.

### Key learnings

- Two sorted arrays can be traversed together by comparing their current elements.
- You do not necessarily need to build the entire merged array if you only need the median.
- Track the previous and current merged values until reaching the middle position.
- Use `/ 2.0` when calculating an average that may need to preserve a fractional result.
- A partial merge can reduce unnecessary work, but it remains linear in the number of elements processed.
- The optimal solution uses binary-search partitioning, which is more complex than a straightforward merge. Understanding the simpler approach first is useful.

### Complexity

For a partial merge that processes up to the middle:

- Time: **O(n + m)** in the worst case.
- Extra space: **O(1)**.

The optimal binary-search approach can achieve **O(log(min(n, m)))** time.

---

## 5. Longest Palindromic Substring

### Pattern
Expand around the centre of a potential palindrome.

### Key learnings

- A brute-force approach checks many substrings and repeatedly tests whether they are palindromes. This can lead to **O(n³)** time.
- Every palindrome has a centre, so instead of checking every substring, expand outward from each possible centre.
- There are two centre types:
  - One character for odd-length palindromes.
  - Two adjacent characters for even-length palindromes.
- Move the left and right pointers outward while the characters match.
- Track the best palindrome using its starting index and length instead of repeatedly creating substrings.
- A local function can keep a helper operation close to the method that uses it. For example, a local `UpdateLongestLen` function can update the best result.
- Keep responsibilities clear: expanding around a centre and updating the best result are related but distinct operations.

### Complexity

- Time: **O(n²)**.
- Extra space: **O(1)**, excluding the returned substring.

---

## 6. Zigzag Conversion

### Pattern
Simulate movement through rows while tracking the current row and direction.

### Key learnings

- A `StringBuilder[]` is a natural representation when each character is assigned to a row and the final result is the rows concatenated in order.
- Track the current row and a direction:
  - `1` means moving down.
  - `-1` means moving up.
- Reverse direction when reaching the top or bottom boundary.
- Handle `numRows <= 1` with an early return. With one row, the result is simply the original string.
- Boundary cases can break otherwise-correct traversal logic. Test the smallest valid inputs, not only the examples.
- Prefer `StringBuilder` over repeated `string +=` when building strings in a loop. Strings are immutable, so repeated concatenation can cause additional allocations and copying.
- `StringBuilder.Append(row)` can append another `StringBuilder`'s contents directly.

### Complexity

- Time: **O(n)**.
- Extra space: **O(n)**.

---

## 7. Reverse Integer

### Pattern
Extract digits from right to left and build the reversed number incrementally.

### Key learnings

- `x % 10` extracts the last digit.
- Integer division `x /= 10` removes the last digit.
- Build the reversed number with:

  ```csharp
  reversed = reversed * 10 + remainder;
  ```

- Check for overflow **before** multiplying and adding.
- For a 32-bit signed integer, the final digit boundary is `7` for `int.MaxValue` and `-8` for `int.MinValue`.
- Negative inputs matter, so both positive and negative overflow must be considered.
- `int` is `System.Int32`; `long` is `System.Int64`.
- The minimum `int` value has a magnitude one greater than the maximum `int` value, so its positive magnitude cannot fit in an `int`.
- Prefer direct digit extraction over arrays or powers of ten when those extra steps are unnecessary.

### Complexity

- Time: **O(d)**, where $d$ is the number of digits.
- Extra space: **O(1)**.

---

## 8. String to Integer (atoi)

### Pattern
Read the input in stages: leading spaces, optional sign, then consecutive digits.

### Key learnings

- Separating parsing into stages can make the code easier to reason about than mixing many conditions and state flags.
- The sign can be checked directly:

  ```csharp
  if (s[i] == '+' || s[i] == '-')
  ```

- After confirming a character is a digit, character arithmetic can convert it to its numeric value:

  ```csharp
  int digit = s[i] - '0';
  ```

  See the character-arithmetic section below.
- A `long` can hold intermediate values outside the `int` range, but it is not unlimited. Clamp the result as soon as it exceeds the permitted range so a very long input cannot overflow the intermediate type.
- Check positive and negative limits separately:
  - `int.MaxValue` is `2147483647`.
  - The magnitude of `int.MinValue` is `2147483648`.
- Manual conversion allows control over whitespace, signs, invalid characters, and overflow without relying on `int.Parse` or similar parsing helpers.
- The parsing process should stop when a non-digit is encountered after the number has started.

### Complexity

- Time: **O(n)**, where $n$ is the input length.
- Extra space: **O(1)**.

---

## 9. Palindrome Number

### Pattern
Compare digits from opposite ends, or reverse only half of the number.

### Key learnings

- Converting a number to a string and comparing characters with two pointers is a clear, valid approach.
- String conversion uses **O(d)** extra space, where $d$ is the number of digits.
- Reversing the entire number numerically can overflow even when the original number is valid.
- Reversing only half of the number avoids this overflow risk and uses constant extra space.
- Reject negative numbers immediately.
- A nonzero number ending in `0` cannot be a palindrome because its first digit would have to be `0`.
- For the half-reversal approach:
  - Extract the last digit using `% 10`.
  - Append it to the reversed half.
  - Remove the digit from the original number using `/ 10`.
  - Stop when the remaining half is no longer larger than the reversed half.
- For an even number of digits, compare the two halves directly.
- For an odd number of digits, remove the middle digit using `reversedHalf / 10` before comparing.

### Complexity

- String and two pointers: **O(d)** time and **O(d)** extra space.
- Reverse half numerically: **O(d)** time and **O(1)** extra space.

---

## 10. Regular Expression Matching - In Progress

This problem is not yet marked completed. The dynamic-programming solution has been introduced, but I plan to revisit it, understand the reasoning, and implement it independently before counting the problem as complete.

### Current learning goal

Understand the dynamic-programming state and transitions rather than memorising the code.

Topics to revisit:

- Define what `dp[i, j]` means for prefixes of the input string and pattern.
- Understand why `.` matches any single character.
- Understand why `*` creates two possibilities:
  1. Match zero occurrences of the preceding pattern character.
  2. If the preceding pattern character matches the current input character, consume one input character while keeping `*` available for additional matches.
- Understand why matching the entire string requires considering the whole pattern, not just a matching prefix.
- Rebuild the solution from scratch and test it against examples before marking the problem complete.

### Update (6 November 2026): technique taught, implementation still pending

I decided to be taught the technique directly instead of discovering it, because the idea that a **2D table** applies here is not something I would have found on my own. Still not marked complete: I must implement it independently.

- **Why dynamic programming applies:** the question about the whole inputs can be restated about *prefixes*; `*` creates a choice (zero copies or one more); the same sub-questions repeat; and the answer for a prefix pair does not depend on how it was reached.
- **Why two dimensions:** two independent quantities change as matching progresses, the amount of `s` used (`i`) and the amount of `p` used (`j`). The number of independent changing quantities is the number of dimensions.
- **State:** `dp[i][j]` is true if the first `i` characters of `s` fully match the first `j` characters of `p`. The table is `(m + 1) x (n + 1)`; row and column 0 are the empty prefix. The answer is `dp[m][n]`.
- **Base cases:** `dp[0][0]` is true; `dp[i][0]` is false for `i > 0`; `dp[0][j]` can be true only through `x*` pairs matching zero times.
- **Transition by the last pattern character `p[j-1]`:**
  - Letter or `.`: true if the last characters match and `dp[i-1][j-1]` is true.
  - `*` (repeating `p[j-2]`): zero copies, `dp[i][j-2]`, OR one more copy when `p[j-2]` matches `s[i-1]`, `dp[i-1][j]`. The star stays available after eating a character, which is why it is `j` and not `j-2`.
- **Why greedy fails:** `"aa"` against `"a*a"`; the star must not eat both characters.
- **Fill order:** row by row, left to right. Cost O(m·n) time and space (a rolling row can reduce space).
- **Tests to run:** `("aa","a")` false, `("aa","a*")` true, `("ab",".*")` true, `("aa","a*a")` true, `("ab",".*c")` false.
- **Recipe for DP problems:** smallest version and state; define `dp[...]` in one sentence; base cases; choices at the last step; fill order; where the answer is.

---

## 11. Container With Most Water

### Pattern
Two pointers, starting at opposite ends and moving the shorter boundary inward.

### Key learnings

- The area is determined by the shorter height multiplied by the width: `Math.Min(height[left], height[right]) * (right - left)`.
- Start with the widest possible container by placing `left` at the beginning and `right` at the end.
- After calculating the current area, move the pointer at the shorter boundary.
- Why this works: the shorter boundary limits the current area. Keeping it while moving the taller boundary inward reduces the width and cannot increase the limiting height. Moving the shorter boundary gives the possibility of finding a taller one.
- Update the maximum area at every valid pair. The loop condition `left < right` ensures the pointers never refer to the same position and naturally includes the final adjacent pair.
- Brute force checks every pair in $O(n^2)$ time. The two-pointer approach reduces this to $O(n)$ time with $O(1)$ auxiliary space.
- Be careful with the width calculation: it is `right - left`, not `left - right`.

### General lesson
When an algorithm eliminates candidates, understand why the discarded possibilities cannot contain a better answer. Pointer movement should be justified by the problem's constraints, not chosen merely because it seems reasonable.

---

## 12. Integer to Roman

### Pattern
Greedy selection using an ordered sequence of values and their Roman numeral representations.

### Key learnings

- Represent the valid value-symbol pairs in descending order, including the subtractive forms: CM, CD, XC, XL, IX, and IV.
- An array or list is appropriate when the algorithm needs to process entries in a specific order. A dictionary is more appropriate when the primary operation is lookup by key.
- For each value-symbol pair, repeatedly append the symbol while the remaining number is at least that value, subtracting the value after each append.
- An alternative is to calculate `count = num / value` and then update the remainder with `num %= value`. Append the corresponding symbol count times.
- Greedy works here because the standard Roman numeral representation can be constructed by repeatedly selecting the largest valid value-symbol pair that does not exceed the remaining number.
- Do not assume that a locally sensible choice is always optimal in every problem. Greedy algorithms require a reason that each choice is safe.
- A loop must make progress towards termination. If the loop condition depends on `num`, ensure that `num` changes appropriately; otherwise, the loop may run indefinitely.
- Check API documentation for method overloads instead of assuming that similar methods accept the same arguments. For example, `StringBuilder.Append(char, repeatCount)` supports repeating a character, but there is no equivalent `StringBuilder.Append(string, repeatCount)` overload.
- Avoid unnecessary branching to distinguish one-character symbols from multi-character symbols. Appending the string uniformly in a small loop is clear and sufficient.
- Keep the implementation simple. My initial approach became more complicated than necessary, and the straightforward greedy implementation was accepted.

### Complexity
There are 13 fixed value-symbol pairs, and the problem constrains the input to 1–3999. Under these fixed constraints, the runtime is bounded by a constant. More generally, runtime depends on the number of value-symbol pairs and the number of output symbols; output storage is proportional to the result length.

### General lesson
Prefer the simplest correct approach before optimising. Optimisation should address a meaningful inefficiency or improve clarity, rather than introduce complexity without a clear benefit.

---

## 13. Roman to Integer

### Pattern
Left-to-right scan that uses one general rule (compare a symbol with the next one) instead of listing every special case.

### Key learnings

- First solution: a `switch` over each character, with explicit look-ahead for the six subtractive pairs (`IV`, `IX`, `XL`, `XC`, `CD`, `CM`). When a pair is consumed, `i++` stops the second character from being processed again. Accepted at 1 ms, beating 99.93% of submissions (48.30 MB).
- The first failure was a typo (`'i'` instead of `'I'`). I found it myself. It is a debugging observation, not a conceptual gap.
- The `I`, `X` and `C` branches all had the same shape. Repeated structure is a signal to look for the rule behind it. The rule: if a symbol's value is smaller than the next symbol's value, the pair contributes `next - current`; otherwise the symbol's own value is added.
- Second solution: `Dictionary<char, int>` plus the adjacent-value rule. Bug: the line after the loop always added the last character, even when the loop had already consumed it as the second half of a pair. Fix: track `i` explicitly with a `while` loop and add the last character only if `i < s.Length`.
- The dictionary version was accepted but slower than the `switch` version. Likely reasons: the dictionary is built on every call, and each lookup pays for hashing. Both versions are still O(n) time and O(1) auxiliary space (seven fixed entries); only the constant costs differ.
- Final refactor: a local function `GetValue(char)` written as a `switch` expression, combined with the adjacent-value rule. One mapping, one rule, no repetition. This version was not benchmarked.
- A cleaner-looking data structure does not automatically give better measured performance, and judge timings vary between runs.

### Complexity

- Time: **O(n)**.
- Auxiliary space: **O(1)**.

### General lesson
When the same special-case code appears several times, derive the general rule that produces all of them. Keep the mapping (symbol to value) separate from the rule (how adjacent values combine).

---

## 14. Longest Common Prefix

### Pattern
Compare characters position by position across all strings, stopping at the first mismatch.

### Key learnings

- First solution: loop over indices up to the shortest string's length, compare `strs[0][i]` with the same position in every string, append to a `StringBuilder`, and return at the first mismatch. Accepted at 8 ms, beating 10.62%.
- Ideas I tried for improving it: sorting by length so the shortest string is checked first, and checking two characters per iteration. Neither reduces the real work. Sorting adds work before any comparing starts, and fewer loop iterations is not the same as fewer comparisons.
- Nested loops do not automatically mean an inefficient solution. The cost is **O(n · m)**, where `n` is the number of strings and `m` is the length of the shortest string, and early termination already stops at the first mismatch. For this straightforward comparison strategy it is already asymptotically optimal.
- Alternative approach: start with the first string as a candidate prefix and shorten it until each following string starts with it (`StartsWith`, `Substring`). Same worst-case complexity, but not necessarily faster: `Substring` allocates a new string every time it shortens the candidate. My original version is likely the more memory-friendly one.
- An elegant approach is not automatically a more efficient one. Compare approaches by complexity, allocations and readability.
- `str[i]` reads a character without allocating. `Substring` creates a new string.

### Complexity

- Time: **O(n · m)** worst case for both approaches.
- Extra space: **O(m)** for the result or candidate prefix; the shrinking-prefix approach also creates intermediate strings.

### Process lessons from this problem

- I tend to avoid built-in string operations because using them feels like getting help, and I avoid allocations. Using `StartsWith`, `Substring`, `Contains` and `IndexOf` is legitimate when I understand what they do and what they cost. **Be allocation-aware, not allocation-averse.**
- Coming up with a different representation of a problem (shrinking a candidate instead of building one character at a time) is a skill that grows with exposure to patterns. Judge progress by whether I understand why an approach works and can recognise it again, not by whether I invent it on the spot.
- Six questions to ask before coding:
  1. What is the input, and what exactly must I return?
  2. What is the simplest correct approach?
  3. What information can I reuse instead of recalculating?
  4. When can I stop early?
  5. Is there a different way to represent or narrow down the problem?
  6. What is the time and space complexity of my proposal?
- Three stages of getting better: finding a solution, finding alternatives, evaluating trade-offs (runtime, memory, readability).

### General lesson
Optimise for a specific reason. Reducing loop iterations is not the same as reducing total work, and a different-looking approach is not always a better one.

---

## 15. 3Sum

### Pattern
Sort, fix one value, then use two pointers to find the remaining pair. Prevent duplicates by skipping equal values instead of searching the result list.

### Key learnings

- Brute force: three nested loops, **O(n³)**. With up to 3,000 elements that is about 4.5 billion triples.
- My first duplicate check (`result.Any(l => l.Contains(nums[i]) && l.Contains(nums[j]))`) compared only two values of a triplet. Two different triplets can share two values. Compare the complete combination.
- Sorting the array first still left three loops (311/316 tests, then time limit exceeded). Sorting made equal values adjacent and made order usable: if a sum is too large, move the right pointer down; if too small, move the left pointer up.
- Fixing `a` turns the problem into finding two values that sum to `-a`. I connected the property of the input (sorted) to a technique (two pointers) myself.
- Mistakes on the way to the final version:
  - Out of Memory: the second condition repeated `> 0` instead of `< 0`, and the pointers did not move after a match.
  - Skipped fixed values by writing `i++` inside the `if` while the `for` loop also increments `i`, which jumped over a valid element. Use `continue` to skip an iteration.
  - `left` started at `i`; it must start at `i + 1` so three distinct indices are used.
- Duplicates can come from two places:
  1. The fixed element: `if (i > 0 && nums[i] == nums[i - 1]) continue;`
  2. The pointers after a match: move first (`left++; right--;`), then skip repeated values by comparing with the value just used:

```csharp
while (left < right && nums[left] == nums[left - 1]) left++;
while (left < right && nums[right] == nums[right + 1]) right--;
```

- Rule of thumb: moving right, compare with the element before; moving left, compare with the element after.
- Reference version also uses `i < nums.Length - 2`, breaks when `nums[i] > 0` (the array is sorted, so every later value is positive and three positives cannot sum to zero), and calculates the sum once per iteration.
- `IList<T>` has no `Sort` method. `Sort` belongs to `List<T>`; with an `IList<T>` variable use `OrderBy(...).ToList()` or declare it as a `List<T>`.

### Complexity

- Sorting: O(n log n). Search: O(n) per fixed element. Overall: **O(n²)**.
- Auxiliary space: O(1) beyond the sort implementation and the returned result.

### Recognising two pointers

- Can the data be sorted?
- Am I looking for a pair that meets a condition?
- Does moving one pointer predictably increase or decrease the result?
- Can I discard possibilities without examining them one by one?

### General lesson
Sorting is more than tidiness: order becomes information. And preventing duplicate answers from being generated is better than detecting them afterwards. The question that finds it: *after finding a valid answer, what happens if I meet the same values again?* Only skip values when doing so cannot remove a genuinely different valid answer.

---

## 16. 3Sum Closest

### Pattern
Same sorted two-pointer structure as 3Sum, but tracking the best (closest) sum instead of collecting exact matches.

### Key learnings

- First attempt: time limit exceeded. The duplicate-skip `while` loops were the only code moving the pointers, so when `nums[left] != nums[left - 1]` neither pointer moved and the loop never ended. Moving a pointer is part of the main search; skipping duplicates is an extra step. **Move first, then skip.**
- Index out of bounds: `closest` was initialised with `nums[0] + nums[1] + nums[3]`. With a length of 3 there is no index 3; it should be `nums[2]`.
- Wrong signal: I chose the pointer direction by comparing `closest` (the best so far) with the target. The direction must come from the `sum` just calculated.
- Accepted after fixing those: 16 ms, beating 41.47%.
- Duplicate skipping is not needed for correctness here, because only one sum is returned and not a list of unique triplets. It only saves iterations, and the complexity stays O(n²). I removed the skipping loops.
- Early-termination discussion, with a **correction**. ChatGPT suggested breaking the outer loop when the maximum sum for the current `i` is below the target and worse than `closest`. That is wrong: the maximum sum *grows* as `i` grows, so later fixed values could produce a closer sum. Safe pruning depends on the direction a quantity moves:
  - If `nums[i] + nums[i+1] + nums[i+2] > target` (the smallest triplet for this `i` is already above the target), every later triplet is larger, so check that one and stop.
  - If the maximum sum for this `i` is below the target, skip the inner search for this `i` after comparing that maximum with `closest`, but continue with the next `i`, do not break.
  - My accepted solution did not use any pruning.

### Complexity

- Sorting: O(n log n). Search: O(n) per fixed element. Overall: **O(n²)**.

### General lesson
Before pruning or breaking early, ask which way the quantity moves and whether the discarded cases could still be better. Decide pointer movement from the current combination, not from the best result so far.

---

## 17. Letter Combinations of a Phone Number

### Pattern
Backtracking: choose a letter, explore the next digit, then undo the choice. Build one combination at a time in a single shared builder.

### Key learnings

- My first idea was nested loops, but the number of loops would depend on the number of digits. Recognising that the number of repeated steps is variable is the key insight; recursion handles it.
- First attempt: a dictionary of digit to `string[]` and a `CrossProduct` method using `StringBuilder`. It returned the right number of results but the wrong strings. `Append` mutates the builder and returns the same instance, so every result pointed at one shared object.
- Second attempt: plain `string` concatenation in `CrossProduct`. Accepted, but slow because it builds and copies intermediate strings.
- Third attempt: reuse one `StringBuilder` and remove the last character after each result. Better, but still stores all intermediate lists.
- Final solution: a recursive local function `BuildCombinations(int position)`.
  - **State:** the current builder and the digit index.
  - **Stopping condition:** `position == digits.Length`, so one letter has been chosen for every digit. Save `current.ToString()` and return.
  - **Repeated action:** for each letter of the current digit: append it, recurse with `position + 1`, then remove it.

```csharp
foreach (char letter in numberLetters[digits[position]])
{
    current.Append(letter);
    BuildCombinations(position + 1);
    current.Remove(current.Length - 1, 1);
}
```

- `Remove` comes after the recursive call because the letter must stay in the combination while every combination that starts with it is explored. A method call pauses the caller until the called method returns.
- As a local function, `BuildCombinations` can use `digits`, `numberLetters`, `results` and `current` from the enclosing method, so only `position` is passed. My earlier attempt used a private method and I was solving the algorithm and the plumbing at the same time.
- A string can be iterated by character, so the dictionary can map to `"abc"` instead of `string[]`.
- Read the constraints before adding defensive checks: `1 <= digits.length`, so no empty-input check is needed.
- Traced on paper with `"23"`: all of `a` is exhausted (`ad`, `ae`, `af`) before moving to `b`.

### Complexity

- Time: about **O(4^k · k)** for `k` digits (at most 4 letters per digit, and each result of length `k` is converted to a string).
- Space: **O(k)** for the recursion and builder, plus the output.

### General lesson
When the idea is clear but the code is not, identify three things before writing: the **state** (where am I right now), the **stopping condition** (when is one result complete), and the **repeated action** (what do I do for each choice, including undoing it). Separate the algorithm from the implementation, and do not worry about solving the next level before starting the current one.

---

## 18. 4Sum

### Pattern
Sort, fix two values with two loops, then use two pointers for the remaining pair (3Sum nested inside 4Sum).

### Key learnings

- First attempt: four pointers (`a`, `b` in the first half, `c`, `d` in the second half), moving them toward the target. It found some answers and missed others. Two problems:
  - Splitting the array in halves means quadruplets entirely inside one half can never be reached.
  - After a match, deciding which of four pointers to move without skipping valid quadruplets is not trivial. Pointers that move in one direction can only be guided by a sorted array when they move toward each other.
- Breakthrough: fix two values and the rest is the 2-sum search I already had. Nest the 3Sum structure inside a new outer loop.
- Loop structure: `i` from 0 to `length - 3`, `j` from `i + 1` to `length - 2`, `remaining = target - nums[i] - nums[j]`, `left = j + 1`, `right = length - 1`.
- Duplicate skipping at each fixed level uses the rule **counter > where that loop starts**:
  - `i > 0 && nums[i] == nums[i - 1]`
  - `j > i + 1 && nums[j] == nums[j - 1]`. The inner loop's first value must be allowed even if it equals `nums[i]`.
- Fixing the same first value more than once is fine; only repeating an identical value as a fixed element repeats the same search.
- After a match: move the pointers first, then skip duplicates by comparing with the value just used. Mistakes: moving `r++` instead of `r--`, and comparing with the next element before moving, which produced a duplicate quadruplet (`[-2,-1,1,2]` twice on `[-2,-1,-1,1,1,2,2]`).
- Integer overflow: four values up to 10⁹ do not fit in `int`. Failing case: four values of `1000000000` with target `-294967296`. Use `long` for the sum and the remaining target, and cast **before** the arithmetic: `(long)nums[l] + nums[r]` and `(long)target - nums[i] - nums[j]`. Assigning an already overflowed `int` to a `long` does not fix it.

### Complexity

- Sorting: O(n log n). Two nested loops with a two-pointer scan: **O(n³)**.
- Auxiliary space: O(1) beyond the sort implementation and the result.

### General lesson
When juggling several pointers becomes unmanageable, reduce the problem to one already solved by fixing values. A duplicate-skip condition must respect where its own loop starts. And check value ranges against the data type before summing.

---

## 19. Remove Nth Node From End of List

### Pattern
Collect the nodes in a list for index access, then relink the node before the target to the node after it.

### Key learnings

- First attempt: store every node in a `List<ListNode>`, compute `indexToRemove = Count - n`, then relink. Issues in the first version: the conditional that picked the previous node was inverted, and removing the head (`indexToRemove == 0`) was not handled because the head has no previous node.
- Final structure has three cases: remove the head (`head = head.next`), remove the last node (`previous.next = null`), remove a middle node (`previous.next = nodeAfter`).
- An intermediate bug assigned to the list slot (`listNodes[i - 1] = listNodes[i + 1]`), which changes the list and not the links. The relink must be `listNodes[i - 1].next = listNodes[i + 1]`.
- Accepted at 0 ms, beating 100%.
- Only the head changes when the head is removed; everything else can be relinked in place.

### Complexity

- Time: **O(L)** for a list of `L` nodes.
- Extra space: **O(L)** because every node is stored in a list.

### Warning to remember
Storing every node in a `List<ListNode>` costs O(L) extra space for a structure that already holds its own nodes. It was a fine first approach that worked, but next time, ask whether the extra collection is really needed. A solution with O(1) extra space exists; I have left it to rediscover on a later pass.

### General lesson
For linked lists, assigning to a collection slot is not the same as changing links, and the head is the special case. A working first approach is worth having, but check what extra memory it costs.

---

## 20. Valid Parentheses

### Pattern
Stack: the most recent unmatched opener is always the first one that must close.

### Key learnings

- First solution: push openers, and on a closer pop an opener, translate it to its closing bracket with a `switch`, and compare. It also had an early exit for odd length. It failed on `"(("`: the loop finishing without a mismatch only means nothing went wrong, not that everything was matched. Fix: `return stk.Count == 0;`. Accepted at 6 ms, beating 29.5%.
- `stk.Count == 0 ||` before the pop matters: `||` short-circuits, so `Pop()` is never called on an empty stack (for example on `")"`).
- Improvement: translate at **push** time. Push the closer that is *owed* (`(` pushes `)`). A closing character then only needs `stk.Count == 0 || stk.Pop() != current`. The mapping runs only for openers, and the stack holds exactly what the string still owes. Accepted at 2 ms, beating 93%. Choose what the stack stores so the check at the decision point is as simple as possible.
- A generalised early exit: at the top of each iteration, `if (stk.Count > s.Length - i) return false;`. Pending openers cannot exceed the characters left, since each remaining character closes at most one.
  - Use `>`, not `>=`: an exact fit like `"(())"` can still succeed.
  - It is safe because `stk.Count - remaining` never decreases: each step changes the count by +1 or -1 while the remaining characters drop by exactly 1.
  - The odd-length check is a special case of the same idea.
  - It does not change the worst case. On `"(((("` it saves one iteration.
- The 6 ms to 2 ms difference and the percentiles are noisy at `n ≤ 10⁴`; the better result is the simpler code.

### Complexity

- Time: **O(n)**.
- Space: **O(n)** for the stack.

### General lesson
Practise recognising when continuing to check is pointless, and write the exact condition that proves it. This is the same idea as the sorted-order pruning in 3Sum and 3Sum Closest, and it is only valid when the quantity you rely on moves in a known direction.

---

## Patterns seen in Questions 11–20

| Pattern | Problems | Recognition clue |
|---|---|---|
| Two pointers from both ends | 11 Container With Most Water | Moving the limiting side is the only move that can improve the answer |
| Greedy over an ordered table | 12 Integer to Roman | Take the largest valid piece that fits, repeatedly |
| One general rule instead of special cases | 13 Roman to Integer | The same special-case code repeats |
| Position-by-position comparison with early exit | 14 Longest Common Prefix | Stop at the first mismatch |
| Sort, fix one, two pointers | 15 3Sum, 16 3Sum Closest | Sortable input, searching for a pair or nearest sum |
| Fix two, two pointers | 18 4Sum | Too many pointers to manage at once |
| Backtracking (choose, explore, undo) | 17 Letter Combinations | A variable number of nested choices |
| Linked-list relinking with a head special case | 19 Remove Nth Node | The head has no previous node |
| Stack | 20 Valid Parentheses | The latest unmatched item must be resolved first |

---

# General C# and Problem-Solving Notes

## C# Local Functions

A local function is a method declared inside another method.

```csharp
public string Example(string input)
{
    int count = 0;

    void IncrementCount()
    {
        count++;
    }

    IncrementCount();

    return $"{input}: {count}";
}
```

### Key learnings

- Local functions can access variables declared in the enclosing method.
- They are useful for helper logic that belongs specifically to one method.
- A separate private method is often more suitable when the helper needs to be reused by multiple methods.
- Keep helpers focused. Avoid introducing one if it makes a simple operation harder to understand.
- A local function is a good fit for a recursive helper, because it can use the enclosing method's collections and builders directly, so only the changing value (such as an index) needs to be passed. Used in Letter Combinations.

## C# Using a `switch` as a map

A map is a concept (a relationship between keys and values); a dictionary is only one data structure that can represent it.

```csharp
int GetValue(char symbol) => symbol switch
{
    'I' => 1, 'V' => 5, 'X' => 10, 'L' => 50,
    'C' => 100, 'D' => 500, 'M' => 1000,
    _ => 0
};
```

### Key learnings

- A `switch` expression suits a small, fixed set of known cases. A dictionary suits mappings that change at runtime or have arbitrary keys.
- A `switch` is not automatically faster than a dictionary; the point is not building a collection that the problem does not need.
- Questions to ask: Are the inputs fixed and known? Will the mapping stay fixed? Is it small enough to read in code? Do I need to add or change entries at runtime?
- Other ways to map: an array (compact numeric keys), a formula such as `c - '0'` (character arithmetic), a dictionary (flexible keys).
- `_ => 0` is a fallback; throwing for an invalid symbol is stricter when invalid input is possible.

## C# Interfaces versus concrete types

`IList<T>` has no `Sort` method; `Sort` belongs to `List<T>`. A variable's declared type decides which members are available, whatever object it holds. Declare `List<T>` when concrete members are needed, or use LINQ such as `OrderBy(...).ToList()`.

## C# Mutable objects and shared references

`StringBuilder.Append` changes the builder and returns the same instance. Adding that return value to a list several times adds the same object several times. Strings are immutable, so concatenation always creates a new, independent string. `StringBuilder.ToString()` also creates an independent string, which is why it is safe to store in a results list.

## C# Integer overflow and casting

`int` holds up to 2,147,483,647. Summing several large values can overflow silently. Use `long` for sums and cast **before** the arithmetic: `(long)a + b`. Casting the result of an `int` expression that already overflowed does not fix it. Check the problem's value ranges before choosing the type.

## C# String operations and allocations

`str[i]` reads a character without allocating. `Substring` allocates a new string every time. Built-in methods (`StartsWith`, `Contains`, `IndexOf`, `Substring`) are legitimate tools: know what they do and what they cost, and be allocation-aware rather than allocation-averse.

## C# Collection initialisers (short forms)

```csharp
Dictionary<char, string> map = new() { ['2'] = "abc", ['3'] = "def" };
List<int> numbers = [1, 2, 3, 4];     // collection expression, C# 12+
HashSet<string> names = ["Oliver", "Alex"];
int[] scores = [85, 92, 78];
Queue<int> queue = new([1, 2, 3]);    // via the constructor
Stack<int> stack = new([1, 2, 3]);
```

The `[]` collection-expression syntax needs C# 12 or later.

---

## C# Character Arithmetic

Characters in C# are represented by `char` values. A `char` represents a UTF-16 code unit, and character literals can be used in arithmetic expressions because they can be converted to numeric values.

For the basic Latin digits and letters, character values are consecutive. This makes it possible to calculate a digit's value or a letter's zero-based position by subtracting a known starting character.

### 1. Converting a digit character to an integer

The characters `'0'` through `'9'` have consecutive numeric values. Therefore, after confirming that a character is a digit, subtracting `'0'` gives its numeric value.

```csharp
char c = '7';

int digit = c - '0'; // 7
```

This does not convert the character `'7'` to the number 7 through parsing. It calculates the difference between the character values.

**Always validate the range first:**

```csharp
if (c >= '0' && c <= '9')
{
    int digit = c - '0';
}
```

Without the range check, subtracting `'0'` from an arbitrary character does not guarantee a valid digit.

### 2. Getting a letter's zero-based position

The same technique works for basic Latin letters.

```csharp
char lower = 'c';
int lowerIndex = lower - 'a'; // 2

char upper = 'C';
int upperIndex = upper - 'A'; // 2
```

This gives a zero-based index:

| Character | Expression | Result |
|--------|--------|--------|
| `'a'` | `'a' - 'a'` | 0 |
| `'b'` | `'b' - 'a'` | 1 |
| `'c'` | `'c' - 'a'` | 2 |
| `'z'` | `'z' - 'a'` | 25 |

For uppercase letters, subtract `'A'` instead.

If the input may contain characters outside the expected range, validate them first:

```csharp
if (c >= 'a' && c <= 'z')
{
    int index = c - 'a';
}
```

### 3. Common uses

Character arithmetic is useful for:

- Converting digit characters to numeric values when parsing a string manually.
- Mapping lowercase letters to array indices.
- Building character-frequency arrays.
- Solving problems involving alphabet positions.
- Avoiding unnecessary searches through arrays when a direct calculation is sufficient.

For example, counting lowercase English letters can use a fixed-size array:

```csharp
int[] frequencies = new int[26];

foreach (char c in s)
{
    if (c >= 'a' && c <= 'z')
    {
        int index = c - 'a';
        frequencies[index]++;
    }
}
```

This maps `'a'` to index `0`, `'b'` to index `1`, and so on.

### 4. Important limitations

- Character arithmetic is not the same as converting arbitrary text to a number.
- Validate the character range before using the calculated result as an array index.
- Subtracting `'a'` is suitable for the basic Latin lowercase alphabet, not every writing system or every Unicode character.
- C# `char` represents a UTF-16 code unit, so some Unicode characters require more than one `char`.
- For general text processing, use appropriate Unicode-aware APIs rather than assuming every character belongs to a simple consecutive range.

### Key takeaway

When characters are known to belong to a consecutive range, subtraction can convert them into useful numeric values or indices. First check that the character belongs to the expected range, then perform the calculation.

---

# General Habits to Keep Practising

- **Get a working solution first.** Improve it after correctness is established instead of trying to find the perfect approach immediately.
- **Look for the simplest suitable approach.** Avoid adding arrays, state variables, or complex conditions when a direct operation is sufficient.
- **Understand the pattern.** Knowing why an approach works makes it easier to recognise when to use it again.
- **Check edge cases.** Empty input, one-element input, boundary values, repeated characters, and overflow can expose mistakes that normal examples do not.
- **Use complexity analysis.** Consider both time and extra space, and distinguish between a simpler solution and an optimally efficient one.
- **Keep code readable.** Clear names, focused methods, and straightforward control flow make problems easier to debug and review.
- **Learn from alternatives.** An accepted solution is a useful starting point, not proof that no clearer or more efficient solution exists.
- **Practise independently.** After reviewing an explanation, close it and try to reconstruct the solution and explain the reasoning without copying.
- **Move first, then skip.** When skipping duplicates with pointers, make sure the pointer still moves on every path through the loop.
- **Decide from the current value.** Pointer direction should depend on the sum just calculated, not the best result so far.
- **Check the direction before pruning.** A `break` or early exit is only safe if the discarded cases cannot be better.
- **Read the constraints first.** They define the inputs to handle and the value ranges that decide the data type.
- **Reduce to a solved problem.** If managing several pointers is unmanageable, fix some values and reuse a known search (3Sum inside 4Sum).
- **Ask what extra memory costs.** A working first approach is good, but note when it stores more than the problem needs.
- **Separate state, stopping condition and repeated action** when an idea is clear but the code is not.
- **Practise before asking for techniques.** Try to reason towards a technique before it is named.

---

# Progress Tracker

1. Two Sum - completed
2. Add Two Numbers - completed
3. Longest Substring Without Repeating Characters - completed
4. Median of Two Sorted Arrays - completed
5. Longest Palindromic Substring - completed
6. Zigzag Conversion - completed
7. Reverse Integer - completed
8. String to Integer (atoi) - completed
9. Palindrome Number - completed
10. Regular Expression Matching - in progress; revisit dynamic programming and implement independently
11. Container With Most Water - completed
12. Integer to Roman - completed
13. Roman to Integer - completed
14. Longest Common Prefix - completed
15. 3Sum - completed
16. 3Sum Closest - completed
17. Letter Combinations of a Phone Number - completed
18. 4Sum - completed
19. Remove Nth Node From End of List - completed (O(L) extra space; revisit for an O(1)-space approach)
20. Valid Parentheses - completed

**Next milestone:** Return to Question 10, Regular Expression Matching. The dynamic-programming technique has been taught (see the Q10 update), but it must still be implemented independently and tested before the problem counts as complete. Do this before starting the next batch. Do not invent lessons for problems that have not been reviewed.
