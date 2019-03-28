using UnityEngine;
using Route;

/// <summary>
/// Used as a sample Route
/// </summary>
public class RouteInvoker : MonoBehaviour {

    public GameObject startNode;
    public GameObject endNode;

    public void Route() {
        if (startNode != null && endNode != null) {
            Router router = new Router();
            RouteResult result = router.Route(startNode.GetComponent<Node>(), endNode.GetComponent<Node>());

            GameObject lineRendererObject = new GameObject();
            lineRendererObject.name = "Line Renderer";
            LineRenderer lineRenderer = lineRendererObject.AddComponent<LineRenderer>();

            Vector3[] points = new Vector3[result.nodes.Count];
            for (int i = 0; i < result.nodes.Count; i++) {
                points[i] = result.nodes[i].transform.position;
            }
            lineRenderer.positionCount = result.nodes.Count;
            lineRenderer.SetPositions(points);

            startNode = null;
            endNode = null;
        }
    }
}
