#pragma once
/*
  A timer class using <chrono>.
  The template argument is for the units returned by count().
  The default units is std::chrono::milliseconds.
*/
#include <chrono>

template <typename Units = std::chrono::milliseconds>
class timer {
public:
  void start() {
    stored_time= clock::now();
  }
  void stop() {
    elapsed_time = clock::now() - stored_time;
  }
  typename Units::rep count() const {
    return std::chrono::duration_cast<Units>(elapsed_time).count();
  }
private:
  using clock = std::chrono::steady_clock;
  clock::time_point stored_time;
  clock::duration elapsed_time;
};
