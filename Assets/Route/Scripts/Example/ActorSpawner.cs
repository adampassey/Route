using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Route.Example {
    public class ActorSpawner : MonoBehaviour {

        public int ActorsToSpawn = 5;
        public GameObject ActorPrefab;
        public Node[] nodes;

        // Start is called before the first frame update
        void Start() {
            nodes = GameObject.FindObjectsOfType<Node>();

            for (int i = 0; i < ActorsToSpawn; i++) {
                GameObject actorObject = Instantiate(ActorPrefab);
                Actor actor = actorObject.GetComponent<Actor>();

                actor.startingNode = RandomNode();
                actor.StartPathing(this);
            }
        }

        public Node RandomNode() {
            int index = (int) Random.Range(0f, nodes.Length - 1);
            return nodes[index];
        }
    }
}
