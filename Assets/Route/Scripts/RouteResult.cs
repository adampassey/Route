﻿using System.Collections.Generic;


namespace Route {

    /// <summary>
    /// Holds the results of a route
    /// </summary>
    public struct RouteResult {
        public int cost;
        public List<Node> nodes;
    }
}
