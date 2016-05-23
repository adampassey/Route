using UnityEngine;
using UnityEditor;

namespace Route {

    /// <summary>
    /// Extend the editor, creating buttons for building
    /// and clearing the grid/graph
    /// </summary>
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Grid pathfindingGrid = (Grid) target;
            if (GUILayout.Button("Build Grid")) {
                pathfindingGrid.BuildGrid();
            }

            if (GUILayout.Button("Clear Grid")) {
                pathfindingGrid.ClearGrid();
            }
        }
    }
}
