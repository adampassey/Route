using UnityEngine;

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

                    GameObject node = Instantiate(nodePrefab, nodePos, Quaternion.identity) as GameObject;
                    node.gameObject.name = $"{x}, {y}";
                    node.transform.SetParent(transform);

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
                        neighbor(current, x + 1, y + 1);
                        neighbor(current, x - 1, y + 1);
                        neighbor(current, x - 1, y - 1);
                        neighbor(current, x + 1, y - 1);
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
        protected void neighbor(Node current, int x, int z) {
            if (x >= 0 && x < width && z >= 0 && z < height) {
                Node neighbor = grid[x, z];
                current.neighbors.Add(neighbor);
            }
        }
    }
}
