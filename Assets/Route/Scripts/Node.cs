using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Route {

    [ExecuteInEditMode]
    [AddComponentMenu("Route/Node")]
    public class Node : MonoBehaviour {

        public int cost = 1;
        public List<Node> neighbors = new List<Node>();

        //  path properties
        public int cheapestPathCost = int.MaxValue;
        public List<Node> shortestPath = new List<Node>();
        public bool visited = false;
        public bool isInShortestPath = false;

        //  draw a square for this gizmo
        void OnDrawGizmos() {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, 0.2f);

            foreach (Node neighbor in neighbors) {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }

        void OnDestroy() {
            foreach (Node n in neighbors) {
                n.neighbors.Remove(this);
            }
        }
    }
}
