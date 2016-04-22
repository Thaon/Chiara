using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//Tried to implement Dijkstra's algorithm, with no success. I have implemented it in C++ and it was working fine, unfortunately here I lack the use of pointers (as unsafe code cannot be produced in unity), thus preventing me to manage which nodes go where. This limitation is making so that finding the path generates a huge memory leak where sub-graphs are generated exponentially at every pass of the algorithm. I could not figure out a way to target specific addresses in memory, so I went for BFS instead. The code for Dijkstra can still be found at the end of this file, and is logically correct, apart from the huge world-eating memory leak, I mean.

public class Pathfinder : MonoBehaviour
{
    Vector3 m_startPos = new Vector3(0, 4, 1);
    Vector3 m_endPos = new Vector3(15, 4, 3);
    Vector3 m_offset = new Vector3(0.5f, 0.5f, 0.5f);

    bool IsTraversable(Vector3 voxel, int[,,] world, bool checkForDirt)
    {
        bool isEmpty = false;
        bool isBelowStone = false;
        bool isBelowDirt = false;

        // is block empty
        isEmpty = world[(int)voxel.x, (int)voxel.y, (int)voxel.z] == (int)m_voxelType.empty;
        // is block below stone
        if ((int)voxel.y >= 1)
        {
            isBelowStone = world[(int)voxel.x, (int)voxel.y - 1, (int)voxel.z] == (int)m_voxelType.stone;
            if (checkForDirt)
                isBelowDirt = world[(int)voxel.x, (int)voxel.y - 1, (int)voxel.z] == (int)m_voxelType.dirt; //added this to enable traversing of AssessmentChunk1
        }
        return isEmpty && (isBelowStone || (isBelowDirt && checkForDirt));
    }

    public Stack<Vector3> BreadthFirstSearch(Vector3 start, Vector3 end, ChunkBuilder chunk, bool checkForDirt)
    {
        Stack<Vector3> waypoints = new Stack<Vector3>();
        Dictionary<Vector3, Vector3> visitedParent = new Dictionary<Vector3, Vector3>();
        Queue<Vector3> q = new Queue<Vector3>();
        bool found = false;
        Vector3 current = start;
        q.Enqueue(start);

        while (q.Count > 0 && !found)
        {
            current = q.Dequeue();
            if (current != end)
            {
                // our adjacent nodes are x+1, x-1, z+1 and z-1
                List<Vector3> neighbourList = new List<Vector3>();
                neighbourList.Add(current + new Vector3(1, 0, 0)); // x+1
                neighbourList.Add(current + new Vector3(-1, 0, 0)); // x-1
                neighbourList.Add(current + new Vector3(0, 0, 1)); // z+1
                neighbourList.Add(current + new Vector3(0, 0, -1)); // z-1

                foreach (Vector3 n in neighbourList)
                {
                    // check if n is within the terrain array range
                    if ((n.x >= 0 && n.x < chunk.m_chunkSize) && n.z >= 0 && n.z < chunk.m_chunkSize)
                    {
                        // check if we can traverse over this
                        if (IsTraversable(n, chunk.m_terrainArray, checkForDirt))
                        {
                            // check if node is already processed
                            if (!visitedParent.ContainsKey(n))
                            {
                                visitedParent[n] = current;
                                q.Enqueue(n);
                            }
                        }
                    }
                }            }
            else
            {
                found = true;
            }        }
        // solution was found, so we can build a path of waypoints
        if (found)
        {
            while (current != start)
            {
                waypoints.Push(current + m_offset);
                current = visitedParent[current];
            }
            waypoints.Push(start + m_offset);
        }
        return waypoints;
    }}

//public class Node
//{
//    Vector3 m_position;
//    List<Node> m_connections = new List<Node>();
//    int m_cost;
//    int m_distance;
//    int m_time;
//    bool m_isVisited = false;

//    public Node(Vector3 pos, int cost) { m_position = pos; m_cost = cost; }

//    public void AddConnection(Node conn)
//    {
//        //add mutual connections between nodes
//        if (!m_connections.Contains(conn))
//        {
//            m_connections.Add(conn);
//            conn.AddConnection(this); //I am assuming that if a node does not have a connection, the connecting node will not have it either
//        }
//    }

//    public List<Node> GetConnections()
//    {
//        return m_connections;
//    }

//    public Vector3 GetPosition()
//    {
//        return m_position;
//    }

//    public int GetCost()
//    {
//        return m_cost;
//    }

//    public bool IsVisited()
//    {
//        return m_isVisited;
//    }

//    public void SetVisited()
//    {
//        m_isVisited = true;
//    }

//    public int GetDistance()
//    {
//        return m_distance;
//    }

//    public void SetDistance(int distance)
//    {
//        m_distance = distance;
//    }

//    public int GetTime()
//    {
//        return m_time;
//    }

//    public void SetTime(int time)
//    {
//        m_time = time;
//    }
//}

//public enum TraversingMode { quickest, shortest }

//public class Pathfinder : MonoBehaviour {

//    GameObject m_cube;
//    bool m_isTraversing = false;

//    Vector3 m_startPos = new Vector3(0, 4, 1);
//    Vector3 m_endPos = new Vector3(15, 4, 3);
//    Vector3 m_offset = new Vector3(0.5f, 0.5f, 0.5f);
//    List<Node> m_graph;

//    void Start ()
//    {
//        m_graph = new List<Node>();
//    }

//    bool IsTraversable(Vector3 voxel, int[,,] world)
//    {
//        bool isEmpty = false;
//        bool isBelowStone = false;

//        // is block empty
//        isEmpty = world[(int)voxel.x, (int)voxel.y, (int)voxel.z] == (int)m_voxelType.empty;
//        // is block below stone
//        if ((int)voxel.y >= 1)
//        {
//            isBelowStone = world[(int)voxel.x, (int)voxel.y - 1, (int)voxel.z] == (int)m_voxelType.stone;
//        }
//        return isEmpty && isBelowStone;
//    }

//    void BuildGraph(int[,,] world)
//    {
//        for (int z = 0; z < world.GetLength(2); z++)
//        {
//            for (int y = 0; y < world.GetLength(2); y++)
//            {
//                for (int x = 0; x < world.GetLength(2); x++)
//                {
//                    Vector3 currentVoxel = new Vector3(x, y, z);
//                    if (IsTraversable(currentVoxel, world))
//                    {
//                        //add the node to the graph
//                        m_graph.Add(new Node(currentVoxel, 1));
//                    }
//                }
//            }
//        }
//    }

//    void ConnectGraph()
//    {
//        foreach (Node node in m_graph)
//        {
//            foreach(Node possibleConnection in m_graph) //I know is not very cost efficient, but I could not figure out something better on the fly :/
//            {
//                if (node.GetPosition() + new Vector3(-1,0,0) == possibleConnection.GetPosition())
//                {
//                    node.AddConnection(possibleConnection);
//                }
//                if (node.GetPosition() + new Vector3(0, -1, 0) == possibleConnection.GetPosition())
//                {
//                    node.AddConnection(possibleConnection);
//                }
//                if (node.GetPosition() + new Vector3(1, 0, 0) == possibleConnection.GetPosition())
//                {
//                    node.AddConnection(possibleConnection);
//                }
//                if (node.GetPosition() + new Vector3(0, 1, 0) == possibleConnection.GetPosition())
//                {
//                    node.AddConnection(possibleConnection);
//                }
//            }
//        }
//    }

//    public List<Node> PathFind(int[,,] world, Vector3 start, Vector3 end, TraversingMode mode)
//    {
//        BuildGraph(world);
//        ConnectGraph();

//        int INFINITY = 999;

//        //build a list of unvisited nodes and set distances to INFINITY
//        List<Node> unvisited = m_graph;
//        foreach (Node node in unvisited)
//        {
//            node.SetDistance(INFINITY);
//            print(node.GetPosition());
//        }
//        //set start node distance to 0
//        //fisrt set start and end nodes
//        Node startNode = null;
//        Node endNode = null;
//        foreach (Node n in m_graph)
//        {
//            if (n.GetPosition() == start)
//            {
//                startNode = n;
//                break;
//            }
//        }
//        foreach (Node n in m_graph)
//        {
//            if (n.GetPosition() == end)
//            {
//                endNode = n;
//                break;
//            }
//        }
//        print(startNode);
//        print(endNode);
//        startNode.SetDistance(0);
//        startNode.SetTime(0);

//        Node current = null;

//        //until the list is empty, we get the smallest distance/cost and we make it our current
//        while (unvisited.Count > 0)
//        {
//            int min = INFINITY;
//            foreach (Node node in unvisited)
//            {
//                if (mode == TraversingMode.quickest)
//                {
//                    if (node.GetTime() < min)
//                    {
//                        current = node;
//                        min = node.GetTime();
//                    }
//                }

//                if (mode == TraversingMode.shortest)
//                {
//                    if (node.GetDistance() < min)
//                    {
//                        current = node;
//                        min = node.GetDistance();
//                    }
//                }
//            }

//            //we increment total distance/time for each node connected
//            foreach (Node connection in current.GetConnections())
//            {
//                if (!connection.IsVisited())
//                {
//                    int tentativeDistance = current.GetDistance() + 1;
//                    if (tentativeDistance < connection.GetDistance())
//                        connection.SetDistance(current.GetDistance() + 1); //we assume all connected nodes have a distance of 1 between eachother
//                }
//            }
//            //when we are done checking connections, we remove the current node and we mark it as visited
//            current.SetVisited();
//            unvisited.Remove(current);
//        }

//        //we now have weighted the graph, we can start going back and finding the path
//        Stack<Node> invertedPath = new Stack<Node>();
//        current = endNode;
//        while (current != startNode)
//        {
//            invertedPath.Push(current);
//            int min = INFINITY;
//            foreach(Node node in current.GetConnections())
//            {
//                if (mode == TraversingMode.quickest)
//                {
//                    if (node.GetTime() < min)
//                    {
//                        current = node;
//                        min = node.GetTime();
//                    }
//                }

//                if (mode == TraversingMode.shortest)
//                {
//                    if (node.GetDistance() < min)
//                    {
//                        current = node;
//                        min = node.GetDistance();
//                    }
//                }
//            }
//        }
//        invertedPath.Push(startNode);

//        List<Node> path = new List<Node>();
//        while(invertedPath.Count > 0)
//        {
//            path.Add(invertedPath.Pop());
//        }
//        return path;
//    }
//}
