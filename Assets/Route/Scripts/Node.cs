using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Route {

    interface INodeEventListener {
        void CustomDestroy(Node n);
    }

    [ExecuteInEditMode]
    [AddComponentMenu("Route/Node")]
    public class Node : MonoBehaviour {

        public static readonly string CUSTOM_DESTROY = "CustomDestroy";

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
                if (neighbor == null) {
                    directNeighbors.Remove(neighbor);
                    diagonalNeighbors.Remove(neighbor);
                    continue;
                }
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }

        /// <summary>
        /// Custom destroy called by editor when the backspace key is hit
        /// when this object is selected in edit mode
        /// </summary>
        public void CustomDestroy() {
            //  break the prefab instance and notify all components of being custom destroyed
            //  Inherit from INodeListener to receive this event
            //  must be using [ExecuteInEditMode] to receive it during edit mode
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            SendMessage(CUSTOM_DESTROY, this, SendMessageOptions.DontRequireReceiver);
            foreach (Node n in neighbors) {
                n.directNeighbors.Remove(this);
                n.diagonalNeighbors.Remove(this);
            }
            DestroyImmediate(this);
        }

        public void OnDestroy() {
            SendMessage(CUSTOM_DESTROY, this, SendMessageOptions.DontRequireReceiver);
            foreach (Node n in neighbors) {
                n.directNeighbors.Remove(this);
                n.diagonalNeighbors.Remove(this);
            }
        }
    }
}
