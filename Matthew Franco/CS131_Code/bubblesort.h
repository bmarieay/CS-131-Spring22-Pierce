#pragma once

/*
  Bubble Sort Algorithm.
  Demonstrates bubble sort is in Theta(n^2)
*/

#include <vector>

template <typename T>
void bubble_sort(std::vector<T>& data)
{
  for (size_t i=0; i<data.size(); i++) {
    for (size_t j=0; j<data.size()-1; j++) {
      if (data[j] > data[j+1]) std::swap(data[j],data[j+1]);
    }
  }
}
