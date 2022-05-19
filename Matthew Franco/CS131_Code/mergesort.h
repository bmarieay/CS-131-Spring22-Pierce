#pragma once
#include <vector>
#include <algorithm>
/* 
	Merge Sort Algorithm.
	Divide & Conquer Algorithm
	- Functions for separating, sorting, and merging
	Demonstrates Merge sort is in Theta(nlogn)
*/

template <typename T>
void merge(std::vector<T>& nums, int start, int mid, int end) {
	std::vector<int> temp;

	int i, j;
	i = start;
	j = mid + 1;

	while (i <= mid && j <= end) {

		if (nums[i] <= nums[j]) {
			temp.push_back(nums[i]);
			++i;
		}
		else {
			temp.push_back(nums[j]);
			++j;
		}

	}

	while (i <= mid) {
		temp.push_back(nums[i]);
		++i;
	}

	while (j <= end) {
		temp.push_back(nums[j]);
		++j;
	}

	for (int i = start; i <= end; ++i)
		nums[i] = temp[i - start];

}

template <typename T>
void merge_sort(std::vector<T>& nums, int start, int end) {
	if (start < end) {
		int mid = (start + end) / 2;
		merge_sort(nums, start, mid);
		merge_sort(nums, mid + 1, end);
		merge(nums, start, mid, end);
	}
}