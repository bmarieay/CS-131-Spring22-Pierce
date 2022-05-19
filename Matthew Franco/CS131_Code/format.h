#pragma once
#include <iostream>

//functions to help format the table
void display(){
    std::cout.width(17);
    std::cout << std::left << "n";
    std::cout.width(17);
    std::cout << std::left << "T(n) in microsec";
    std::cout.width(17);
    std::cout << std::left << "T(n)/f(n)\n" << std::endl;
}
//function that formats and displays the n numbers used, the running time, and the constant theta
void results(int n, int tot_time, double constant){
    std::cout.width(17);
    std::cout << std::left << n;
    std::cout.width(17);
    std::cout << std::left << tot_time;
    std::cout.width(17);
    std::cout << std::left << constant << std::endl;
}