using UnityEngine;
using System.Collections;

namespace Route {

    /// <summary>
    /// Static class for interacting with Nodes
    /// </summary>
    public static class Router {

        /// <summary>
        /// Given a starting and ending node, determine the 
        /// shortest path between the two
        /// </summary>
        /// <param name="start">The starting node</param>
        /// <param name="end">The ending node</param>
        /// <returns>A RouteResult</returns>
        public static RouteResult Route(Node start, Node end) {
            RouteResult result = new RouteResult();

            start.cheapestPathCost = 0;
            start.shortestPath.Add(start);

            Queue nodes = new Queue();
            nodes.Enqueue(start);

            while (nodes.Count > 0) {
                Node current = nodes.Dequeue() as Node;

                foreach (Node n in current.neighbors) {
                    int cost = current.cheapestPathCost + n.cost;
                    if (cost < n.cheapestPathCost) {
                        n.cheapestPathCost = cost;
                        n.shortestPath.Clear();
                        foreach (Node shortestNode in current.shortestPath) {
                            n.shortestPath.Add(shortestNode);
                        }
                        n.shortestPath.Add(n);
                    }

                    if (!n.visited) {
                        nodes.Enqueue(n);
                    }
                }

                current.visited = true;

                if (current == end) {
                    result.cost = current.cheapestPathCost;
                    result.nodes = current.shortestPath;
                    return result;
                }
            }

            //  didn't find a path?
            return result;
        }
    }
}
