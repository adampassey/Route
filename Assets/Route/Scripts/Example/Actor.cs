using UnityEngine;

namespace Route.Example {
    public class Actor : MonoBehaviour {

        public Node startingNode;
        public float Speed = 1.0f;

        private Router router;
        public RouteResult route;
        private ActorSpawner spawner;
        public int currentNodeIndex = 0;

        public void StartPathing(ActorSpawner s) {
            router = new Router();
            spawner = s;

            Vector3 pos = startingNode.transform.position;
            transform.position = pos;

            Node targetNode = spawner.RandomNode();
           
            //  now we have the route
            route = router.Route(startingNode, targetNode);
        }

        // Update is called once per frame
        void Update() {
            //  move the actor along the route
            Node targetNode = route.nodes[currentNodeIndex];
            Vector3 newPos = Vector3.Lerp(transform.position, targetNode.transform.position, Speed * Time.deltaTime);
            transform.position = newPos;

            if (transform.position.Equals(targetNode.transform.position)) {
                currentNodeIndex++;

                if (currentNodeIndex >= route.nodes.Count) {
                    Node newTargetNode = spawner.RandomNode();
                    router.ClearPaths();
                    route = router.Route(targetNode, newTargetNode);

                    currentNodeIndex = 0;
                }
            }
        }
    }
}
