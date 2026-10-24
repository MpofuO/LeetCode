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

**Next milestone:** Complete Question 10, confirm the dynamic-programming reasoning is understood, and then treat the first ten-question batch as complete. Update this document after each batch of ten completed questions using lessons from actual attempts and discussions.