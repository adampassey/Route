# Route
Simple weighted graph pathfinding package for [Unity3d](http://unity3d.com).

### Usage

Download or clone the `Route` repository locally and import [route.unitypackage](https://github.com/adampassey/Route/blob/master/route.unitypackage)
by selecting `Assets -> Import Package -> Custom Package`. Drop a `Route Grid` object into your scene and press the `Build Grid` button.
This will generate a graph that can be used to route the shortest path from one node to another.

Once you've created your node grid, your can find the shortest path between two nodes using
[Router.cs](https://github.com/adampassey/Route/blob/master/Assets/Route/Scripts/Router.cs):

```c#
using Route;

void ExampleMethod() {
  Router router = new  Router();
  RouteResult result = router.Route(startNode, endNode);

  //  shows the total cost of this route
  Debug.Log(result.cost);

  //  print every node in this path
  foreach (Node n in result.nodes) {
    Debug.Log(n.gameObject.name);
  }
}
```

![alt tag](http://i.imgur.com/j1aNO2W.jpg)
