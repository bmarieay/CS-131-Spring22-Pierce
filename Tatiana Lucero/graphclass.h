#include <iostream>
#include <fstream>
#include <iomanip>
#include <iterator>
#include <list>
#include <queue>
#include <string>
#include <vector>
#include <stack>
#include <queue>
#include <map>
using namespace std;

class graph
{
public:

    void clearGraph();
      //Function to clear graph.
      //Postcondition: The memory occupied by each vertex is deallocated.

    void printGraph() const;
      //Function to print graph.
      //Postcondition: The graph is printed.

    void breadthFirstTraversal();
      //Function to perform the breadth first traversal of
      //the entire graph.
      //Postcondition: The vertices of the graph are printed 
      //    using the breadth first traversal algorithm.
    void printShortestPath(int S, int F, map<string, int> airportID);
        //prints the shortest path given a starting place S, source
        //and a destination F, final.

	graph(const vector<vector<int>>& adjacencylists);
	   //Constructor
	   //PostCondition : ?
	graph(int size = 0); 
      //Constructor
      //Postcondition: gSize = 0,  graph is an array of pointers to linked lists.

    ~graph();
      //Destructor
      //The storage occupied by the vertices is deallocated.

protected:
    int gSize;      //current number of vertices
    list<int> *g; //array to create of adjacency lists 

private:
    bool bft(int S, int F, int tV, int p[], int nfs[]);
    //^used by shortest path to store pecesor of e/a node, and the distance from source.
};

void graph::clearGraph()
{
    for (int index = 0; index < gSize; index++)
        g[index].clear();

} //end clearGraph

void graph::printGraph() const
{
    for (int index = 0; index < gSize; index++)
    {
        cout << index+1<< "-> ";
		for (list<int>::const_iterator i = g[index].begin(); i != g[index].end(); ++i)
			cout << *i<< ' ';
        cout << endl;
    }
    cout << endl;
} //end printGraph

void graph::breadthFirstTraversal()
{
  //declare queue
  bool visited[10];
  //the size should be like a const variable assigned to the value of gSize but visual studio doesn't like that
  queue<int> q;
  //start at 0
  //put 0 in queue
  //iterate through connections
  //go to top of queue after adding
  for (int i = 0; i < gSize; i++){
    visited[i] = false;
  }

  visited[0] = true;
  q.push(0);

  
  list<int>::iterator i;
  while (!q.empty()) {
    int currv = q.front();
    cout << currv << " ";
    q.pop();

    for (i = g[currv].begin(); i != g[currv].end(); ++i) {
      if (!visited[*i]) {
        visited[*i] = true;
        q.push(*i);
      }
    }

  }

} //end breadthFirstTraversal

void graph::printShortestPath(int S, int F, map<string, int> airportID){
    int p[gSize], nfs[gSize], j=F; //j= basically i for the translation of the previous storing list to the shortest path list.
    vector<int> sPath; //shortest path.

    if (bft(S, F, gSize, p, nfs) == false) {
        cout << "No path between start and final"; return;
    }

    sPath.push_back(j);
        while (p[j] != -1) {
        sPath.push_back(p[j]);
        j = p[j];
    }

    cout<<"Number of connections needed to get to airport ID "<< F <<" is: "<<nfs[F]<<endl<<"Path is as follows: ";
    for(int i=nfs[F];i>=0;i--){
        cout<<sPath[i]<<" ";
    }
}

//S= ID of source node,
//F= ID of final node,
//tV=total vertices
//p= list that stores the shortest path, contains the previous vertex but needs to be translated
//nfs, nodes from source (distance)
bool graph::bft(int S, int F, int tV, int p[], int nfs[])
{
  //declare queue
  queue<int> q;
  bool visited[tV];

    for (int i = 0; i < gSize; i++) {
        visited[i] = false;
        nfs[i] = 100;
        p[i] = -1;
    }

    visited[S] = true; //adding the source node to our path
    nfs[S]=0; //distance from S to S =0
    q.push(S); //pushin onto queue


    list<int>::iterator i;
    while (!q.empty()) {
        int currv = q.front();
        //cout << currv << " ";
        q.pop();

        for (i = g[currv].begin(); i != g[currv].end(); ++i) {
            if (!visited[*i]) {
                visited[*i] = true;
                nfs[*i]= nfs[currv]+1;
                p[*i]=currv;
                q.push(*i);
                if(*i==F){
                    return true;
                }
            }
        }

    }
    return false;
} //end bft


//Constructors
    graph::graph(int size)
    {
        gSize = size;
        g = new list<int>[gSize];
    }

    graph::graph(const vector<vector<int>>& adjacencylists)
    {
        gSize = adjacencylists.size();
        g = new list<int>[gSize];
        for (int i = 0; i < gSize; ++i)
            for (int j = 0; j < adjacencylists[i].size(); ++j)
                g[i].push_back(adjacencylists[i][j]);
    }
        //Destructor
    graph::~graph()
    {
        clearGraph();
    }
