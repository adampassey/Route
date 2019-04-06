using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Route;

[ExecuteInEditMode]
public class GridListener : MonoBehaviour, IGridListener
{
    public void OnGridBuilt(Node[,] grid) {
        Debug.Log("Grid created.");
        Debug.Log(grid.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
