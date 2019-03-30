using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Route.Example {
    public class NodeEventListener : MonoBehaviour {
        private RouteInvoker Router;
        private Node node;

        void Start() {
            node = GetComponent<Node>();
            Router = GameObject.Find("Route Invoker").GetComponent<RouteInvoker>();
        }

        public void OnMouseDown() {
            if (Router.startNode == null) {
                Router.startNode = gameObject;
            } else if (Router.endNode == null) {
                Router.endNode = gameObject;
                Router.Route();
            }
        }
    }
}
