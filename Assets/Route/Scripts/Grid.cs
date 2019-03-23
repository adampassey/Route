﻿using UnityEngine;

namespace Route {

    /// <summary>
    /// Exposes creation and manipulation of nodes in a graph.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("Route/Grid")]
    public class Grid : MonoBehaviour {

        public GameObject nodePrefab;
        public int width = 10;
        public int height = 10;
        public float spacing = 2.0f;
        public bool diagonalMovement = true;

        private Node[,] grid;

        /// <summary>
        /// Build the graph based on the specified width/height
        /// </summary>
        public void BuildGrid() {
            grid = new Node[width, height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    GameObject node = GameObject.Instantiate(nodePrefab, new Vector3(x * spacing, y* spacing, transform.position.z), Quaternion.identity) as GameObject;
                    node.transform.SetParent(transform);
                    grid[x, y] = node.GetComponent<Node>();
                }
            }

            //  now connect the graph
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < height; z++) {
                    Node current = grid[x, z];

                    neighbor(current, x, z - 1);
                    neighbor(current, x, z + 1);
                    neighbor(current, x - 1, z);
                    neighbor(current, x + 1, z);

                    //  if we're supporting diagonals
                    if (diagonalMovement) {
                        neighbor(current, x + 1, z + 1);
                        neighbor(current, x - 1, z + 1);
                        neighbor(current, x - 1, z - 1);
                        neighbor(current, x + 1, z - 1);
                    }
                }
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
        private void neighbor(Node current, int x, int z) {
            if (x >= 0 && x < width && z >= 0 && z < height) {
                Node neighbor = grid[x, z];
                current.neighbors.Add(neighbor);
            }
        }
    }
}
