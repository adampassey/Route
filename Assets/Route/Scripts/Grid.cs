﻿using UnityEngine;
using UnityEditor;

namespace Route {

    public interface IGridListener {
        void OnGridBuilt(Node[,] grid);
    }

    /// <summary>
    /// Exposes creation and manipulation of nodes in a graph.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("Route/Grid")]
    public class Grid : MonoBehaviour {

        public GameObject nodePrefab;

        [Tooltip("Whether to maintain prefab link when building the grid")]
        public bool MaintainPrefabLink = false;
        public int width = 10;
        public int height = 10;
        public float spacing = 2.0f;
        public bool diagonalMovement = true;
        public bool isometric = false;
        public GameObject gridListener;

        protected Node[,] grid;

        /// <summary>
        /// Build the graph based on the specified width/height
        /// </summary>
        public virtual void BuildGrid() {
            grid = new Node[width, height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    Vector3 nodePos = new Vector3(
                        x * spacing, 
                        y * spacing, 
                        transform.position.z
                    );

                    if (isometric) {
                        nodePos = new Vector3(
                            x - y,
                            (x + y) / 2f,
                            transform.position.z
                        );
                    }

                    GameObject node;

                    if (!MaintainPrefabLink) {
                        node = Instantiate(nodePrefab, nodePos, Quaternion.identity) as GameObject;
                        node.transform.SetParent(transform);
                    } else {
                        node = PrefabUtility.InstantiatePrefab(nodePrefab, transform) as GameObject;
                        node.transform.position = nodePos;
                    }
                    node.gameObject.name = $"{x}, {y}";

                    grid[x, y] = node.GetComponent<Node>();
                }
            }

            //  now connect the graph
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Node current = grid[x, y];

                    neighbor(current, x, y - 1);
                    neighbor(current, x, y + 1);
                    neighbor(current, x - 1, y);
                    neighbor(current, x + 1, y);

                    //  if we're supporting diagonals
                    if (diagonalMovement) {
                        neighbor(current, x + 1, y + 1, true);
                        neighbor(current, x - 1, y + 1, true);
                        neighbor(current, x - 1, y - 1, true);
                        neighbor(current, x + 1, y - 1, true);
                    }
                }
            }

            if (gridListener != null) {
                gridListener.SendMessage("OnGridBuilt", grid, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// Clear all child GameObject's in the graph
        /// </summary>
        public void ClearGrid() {
            foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
                if (t == transform) {
                    continue;
                }
                DestroyImmediate(t.gameObject);
            }
        }

        /// <summary>
        /// Sets the node as a neighbor if it exists
        /// </summary>
        /// <param name="current">The current node</param>
        /// <param name="x">The neighboring x position</param>
        /// <param name="z">The neighboring z position</param>
        /// <param name="diagonal">Set if this is a diagonal node</param>
        protected void neighbor(Node current, int x, int z, bool diagonal = false) {
            if (x >= 0 && x < width && z >= 0 && z < height) {
                Node neighbor = grid[x, z];
                if (!diagonal) {
                    current.directNeighbors.Add(neighbor);
                } else {
                    current.diagonalNeighbors.Add(neighbor);
                }
            }
        }
    }
}
