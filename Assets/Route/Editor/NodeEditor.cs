using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Route {

    [CustomEditor(typeof(Node), true)]
    [CanEditMultipleObjects]
    public class NodeEditor : Editor {

        public void OnSceneGUI() {
            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Backspace) {
                Node n = target as Node;
                n.CustomDestroy();
            }
        }
    }
}