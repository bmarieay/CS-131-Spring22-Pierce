#pragma once
#include <vector>
#include <algorithm>

/*
  Insertion sort Algorithm.
  Demonstrates Insertion sort is in Theta(n^2)
*/

template <typename T>
void insertion_sort(std::vector<T>& nums)
{
    int i, key, j;
    for (i = 1; i < nums.size(); i++)
    {
        key = nums[i];
        j = i - 1;
 
        while (j >= 0 && nums[j] > key)
        {
            nums[j + 1] = nums[j];
            j = j - 1;
        }
        nums[j + 1] = key;
    }
}