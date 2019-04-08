using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Route {

    [ExecuteInEditMode]
    [AddComponentMenu("Route/Node")]
    public class Node : MonoBehaviour {

        public int cost = 1;

        public List<Node> neighbors {
            get {
                
                return directNeighbors.Concat(diagonalNeighbors).ToList<Node>();
            }
        }

        public List<Node> directNeighbors = new List<Node>();
        public List<Node> diagonalNeighbors = new List<Node>();

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
