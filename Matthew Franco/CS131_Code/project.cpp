/*This program takes a vector of n numbers & sorts the vector with 
  Bubble Sort, Insertion Sort, Selection Sort, Merge Sort, & Quick Sort.
  The running time will be recorded, and the constant theta will be calculated
  & displayed to prove that the sorting algorithm complexity is constant.*/
#include "timer.h"
#include "quicksort.h"
#include "bubblesort.h"
#include "mergesort.h"
#include "insertionsort.h"
#include "selectionsort.h"
#include "format.h"
#include <chrono>
#include <cmath>
#include <vector>
#include <iostream>
#include <functional>
#include <random>
#include <algorithm>

int main()
{
    //psuedo random generator
    std::mt19937 e;
    std::uniform_int_distribution<int> u(1,99);
    auto rng = std::bind(u,e);
    timer<std::chrono::microseconds> stopwatch;
    //bubble sort 
    std::cout << "Bubble sort\n-----------\n";
    display();
    //loop for displaying bubble sort
    for(size_t i=1; i<6; i++){
        std::vector <int> data(10000*i);
        //bubble sort timer & algorithm
        for(int& d: data) d = u(e); //fill vector
        stopwatch.start();
        bubble_sort(data);
        stopwatch.stop();
        //get T(n)/f(n)
        int n = 10000*i;
        int tot_time = stopwatch.count();
        double function = (pow(n*1.0, 2))*0.0023;
        double constant = (tot_time)*1.0/function;
        //display time take for bubble sort
        results(n, tot_time, constant);
    }
    //Insertion Sort
    std::cout << "\nInsertion Sort\n--------------\n";
    display();
    for(size_t i=1; i<6; i++){
        std::vector <int> data(10000*i);
        //insertion sort timer & algorithm
        for(int& d: data) d = u(e); //fill vector
        stopwatch.start();
        insertion_sort(data);
        stopwatch.stop();
        //get T(n)/f(n)
        int n = 10000*i;
        int tot_time = stopwatch.count();
        double function = (pow(n*1.0, 2))*0.00023;
        double constant = (tot_time)*1.0/function;
        //display time take for insertion sort
        results(n, tot_time, constant);
    }
    //Selection Sort
    std::cout << "\nSelection sort\n--------------\n";
    display();
    for(size_t i=1; i<6; i++){
        std::vector <int> data(1000*i);
        //selection sort timer & algorithm
        for(int& d: data) d = u(e); //fill vector
        stopwatch.start();
        selection_sort(data);
        stopwatch.stop();
        //get T(n)/f(n)
        int n = 1000*i;
        int tot_time = stopwatch.count();
        double function = (pow(n*1.0, 2)*0.00234);
        double constant = (tot_time)*1.0/function;
        //display time take for selection sort
        results(n, tot_time, constant);
    }
    //Merge Sort
    std::cout << "\nMerge Sort\n----------\n";
    display();
    //loop for displaying merge sort
    for(size_t i=1; i<6; i++){
        std::vector <int> data(10000*i);
        //merge sort timer & algorithm
        for(int& d: data) d = u(e); //fill vector
        stopwatch.start();
        merge_sort(data, 0, data.size()-1);
        stopwatch.stop();
        //get T(n)/f(n)
        int n = 10000*i;
        int tot_time = stopwatch.count();
        double function = (n*log(n))*0.023;
        double constant = (tot_time)*1.0/function;
        //display time take for merge sort
        results(n, tot_time, constant);
    }
    //Quick Sort
    std::cout << "\nQuick Sort\n----------\n";
    display();
    //loop for displaying quick sort
    for(size_t i=1; i<6; i++){
        std::vector <int> data(10000*i);
        //quick sort timer & algorithm
        for(int& d: data) d = u(e); //fill vector
        stopwatch.start();
        quicksort(data.begin(), data.end());
        stopwatch.stop();
        //get T(n)/f(n)
        int n = 10000*i;
        int tot_time = stopwatch.count();
        double function = (n*log(n))*0.01;
        double constant = (tot_time)*1.0/function;
        //display time take for quick sort
        results(n, tot_time, constant);
    }
}