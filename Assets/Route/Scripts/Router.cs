using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Route {

    /// <summary>
    /// Used to determine paths between Nodes
    /// </summary>
    public class Router {

        //  dictionary to store all paths for each node
        Dictionary<int, Path> paths = new Dictionary<int, Path>();

        /// <summary>
        /// Path data for each node
        /// </summary>
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

            //  create the path for the starting node
            Path startPath = new Path {
                cheapestPathCost = 0
            };
            startPath.nodes.Add(start);

            //  add that path to the dict
            paths.Add(start.GetInstanceID(), startPath);

            //  create a stack of nodes and queue the starting node
            Stack nodes = new Stack();
            nodes.Push(start);

            //  while we have nodes...
            while (nodes.Count > 0) {

                Node current = nodes.Pop() as Node;

                //  retrieve the path for this node if exists in the dictionary
                pathForKey(current.GetInstanceID(), out Path currentPath);

                //  mark this node as visited in the path
                currentPath.visited = true;

                //  list of nodes to sort and prioritize next
                List<Node> nextNodes = new List<Node>();

                //  for each one fo the nodes neighbors:
                foreach (Node neighborNode in current.neighbors) {

                    //  create a new path for this neighbor
                    pathForKey(neighborNode.GetInstanceID(), out Path neighborPath);

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

                    //  if this node isn't visited, add it to 
                    //  nextNodes to be sorted and processed
                    if (!neighborPath.visited) {
                        nextNodes.Add(neighborNode);
                    }
                }

                //  sort all neighbor nodes that haven't been processed
                //  by their distance to the end node and add them to 
                //  the stack
                nextNodes = sortNodesByDistanceToNode(nextNodes, end);
                foreach (Node nextNode in nextNodes) {
                    nodes.Push(nextNode);
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

        /// <summary>
        /// Create a Path object for a given key. Places path in the `places` dictionary
        /// to be queried.
        /// </summary>
        /// <param name="key">Unique identifer of the node, expected gameObject.GetInstanceID()</param>
        /// <param name="path">Out Path object</param>
        private void pathForKey(int key, out Path path) {
            paths.TryGetValue(key, out path);
            if (path == null) {
                path = new Path();
                paths.Add(key, path);
            }
        }

        /// <summary>
        /// Heuristic sort to choose highest priority node in neighbor by
        /// prioritizing the neighboring node that is closest to the end node
        /// </summary>
        /// <param name="nodes">List of nodes to sort</param>
        /// <param name="endNode">The end node</param>
        /// <returns></returns>
        private List<Node> sortNodesByDistanceToNode(List<Node> nodes, Node endNode) {

            nodes.Sort(delegate (Node a, Node b) {
                float aDistance = Vector3.Distance(a.transform.position, endNode.transform.position);
                float bDistance = Vector3.Distance(b.transform.position, endNode.transform.position);

                if (aDistance.Equals(bDistance)) {
                    return 0;
                } else return aDistance.CompareTo(bDistance);
            });

            return nodes;
        }
    }
}
