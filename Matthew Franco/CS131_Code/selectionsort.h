#pragma once
#include <algorithm>

/*
  Selection sort Algorithm.
  Demonstrates selection sort is in Theta(n^2)
*/

template <typename Iterator>
void selection_sort_helper(Iterator start, Iterator end)
{
  if (start == end) return;
  auto min = std::min_element(start, end);
  std::swap(*min,*start);
  selection_sort_helper(++start, end);
}

template <typename Container>
void selection_sort(Container& items)
{
  selection_sort_helper(items.begin(), items.end());
}