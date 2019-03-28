using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Route {

    /// <summary>
    /// Used to determine paths between Nodes
    /// </summary>
    public class Router {

        private class Path {
            public int cheapestPathCost = int.MaxValue;
            public List<Node> nodes = new List<Node>();
            public bool visited = false;
            public bool isInShortestPath = false;
        }

        /// <summary>
        /// Given a starting and ending node, determine the 
        /// shortest path between the two
        /// </summary>
        /// <param name="start">The starting node</param>
        /// <param name="end">The ending node</param>
        /// <returns>A RouteResult</returns>
        public RouteResult Route(Node start, Node end) {

            //  create the result object
            RouteResult result = new RouteResult();

            //  create the dictionary to store each node's properties
            Dictionary<int, Path> paths = new Dictionary<int, Path>();

            //  create the path for the starting node
            Path startPath = new Path {
                cheapestPathCost = 0
            };
            startPath.nodes.Add(start);

            //  add that path to the dict
            paths.Add(start.GetInstanceID(), startPath);

            //  create a queue of nodes and queue the starting node
            Queue nodes = new Queue();
            nodes.Enqueue(start);

            //  while we have nodes...
            while (nodes.Count > 0) {

                Node current = nodes.Dequeue() as Node;

                //  retrieve the path for this node if exists in the dictionary
                paths.TryGetValue(current.GetInstanceID(), out Path currentPath);
                if (!paths.ContainsKey(current.GetInstanceID())) {
                    paths[current.GetInstanceID()] = currentPath;
                }

                //  mark this node as visited in the path
                currentPath.visited = true;

                //  for each one fo the nodes neighbors:
                foreach (Node neighborNode in current.neighbors) {

                    //  calculate the cost- the current cost to get here + the new nodes cost
                    paths.TryGetValue(neighborNode.GetInstanceID(), out Path neighborPath);
                    
                    //  if neighborpath is null, create it and add it
                    if (neighborPath == null) {
                        neighborPath = new Path();
                        paths.Add(neighborNode.GetInstanceID(), neighborPath);
                    }

                    //  determine the cost to get to this node
                    int cost = currentPath.cheapestPathCost + neighborNode.cost;

                    //  if the calculated cost is less than the current n.cost (defaults to int.max),
                    //  update this as the shortest path to this node
                    if (cost < neighborPath.cheapestPathCost) {
                        neighborPath.cheapestPathCost = cost;
                        neighborPath.nodes.Clear();

                        //  add all nodes in current path to this neighbors nodes
                        foreach (Node shortestNode in currentPath.nodes) {
                            neighborPath.nodes.Add(shortestNode);
                        }

                        //  now add this node as well
                        neighborPath.nodes.Add(neighborNode);
                    }

                    //  mark as visited if not already
                    if (!neighborPath.visited) {
                        nodes.Enqueue(neighborNode);
                    }
                }

                //  if this is the end node, set the result and return
                if (current == end) {
                    result.cost = currentPath.cheapestPathCost;
                    result.nodes = currentPath.nodes;
                    return result;
                }
            }

            //  if no return, return empty result...
            return result;
        }
    }
}
