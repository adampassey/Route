using UnityEngine;
using Route;

/// <summary>
/// Used as a sample Route
/// </summary>
public class RouteInvoker : MonoBehaviour {

    public GameObject startNode;
    public GameObject endNode;

	// Use this for initialization
	void Start () {
        Router router = new Router();

        RouteResult result = router.Route(startNode.GetComponent<Node>(), endNode.GetComponent<Node>());
        Debug.Log("Path is the following nodes:");
        foreach (Node n in result.nodes) {
            Debug.Log(n.gameObject.name);
        }
	}
}
