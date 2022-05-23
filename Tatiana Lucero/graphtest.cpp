#include <cassert>
#include <fstream>
#include <iostream>
#include <vector>
#include <map>
#include "graphclass.h"
using namespace std;

int main() 
{
	{   
        //inputting # of airports, airports and connections data from a file
            string file_name = "airports_and_flights.txt";
            ifstream in(file_name);
            map<string, int> airportID;
            string a,c;
            int numAirports, b;
            in >> numAirports;
            cout<<"numAirports: "<<numAirports<<endl;
            vector<vector<int>> adj(numAirports);
            //here we're converting the three letter codes
            //into an ID which we'll use in our program
                if(in.good()){    
                    for(int i=1;i<numAirports+1;i++){
                        in>>a>>b;
                        cout<< "string: " <<a<< " ID: "<<b<<endl;
                        airportID.insert(pair<string,int>(a,b));
                    }
                }
        //we're putting the connections into an adjacency list.
        //which will be turned into a graph
            while(in.good())
            {

                in.ignore(100, '\n');
                in>>a>>c;
                adj[airportID[a]-1].push_back(airportID[c]);
            }
            in.close();
        //this converts the adj. list into a graph
            graph g(adj);
        //this basically prints the graph as an adj. list
            g.printGraph();
        //and this is our algorithm
            g.printShortestPath(airportID["FRA"], airportID["IST"],airportID);
        
	}
}

