using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Route {

    [AddComponentMenu("Route/Actor")]
    public class Actor : MonoBehaviour {

        [Tooltip("Time to navigate between Nodes")]
        public float Speed = 1.0f;

        private RouteResult routeResult;
        public RouteResult RouteResult {
            get {
                return routeResult;
            } set {
                routeResult = value;
                currentNodeIndex = 0;
            }
        }

        private int currentNodeIndex = 0;
        private Node currentNode;
        private Node targetNode;
        private float startMoveTime;
        private Router router = new Router();

        // Update is called once per frame
        void Update() {
            if (targetNode != null) {
                float time = ((startMoveTime + Speed) - Time.deltaTime) / (startMoveTime + Speed);
                transform.position = Vector3.Lerp(currentNode.transform.position, targetNode.transform.position, time);

                //  if we've reached our destination Node, stop or set next node
                if (transform.position.Equals(targetNode.transform.position)) {
                    if (targetNode == routeResult.nodes[routeResult.nodes.Count]) {
                        targetNode = null;
                        return;
                    }

                    startMoveTime = Time.deltaTime;
                    currentNode = targetNode;
                    currentNodeIndex++;
                    targetNode = routeResult.nodes[currentNodeIndex];
                }
            }
        }

        public void SetRoute(RouteResult route) {
            //  if we're instructed to move to this tile, we're here!
            if (route.nodes.Count <= 0) {
                return;
            }

            startMoveTime = Time.deltaTime;
            currentNodeIndex = 0;

            //  assume we're on the starting Node
            currentNode = route.nodes[0];
            targetNode = route.nodes[1];
        }
    }
}
