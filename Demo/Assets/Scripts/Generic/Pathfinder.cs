using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    #region Attributes

    // This attribute represents the array of grids reference.
    public Node[,] ActualGridArray;
    // This attribute represents the cost value for straight movement.
    private const int MovementCostStraight = 10;
    // This attribute represents the cost value for diagonal movement.
    private const int MovementCostDiagonal = 14;

    #endregion

    #region Constructor

    /// <summary> This constructor initializes class with the array of grids. </summary>
    /// <param name="actualGridArray"> Array of grids parameter. </param>
    public Pathfinder(Node[,] actualGridArray)
    {
        ActualGridArray = actualGridArray;
    }

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// AStarPathfinder
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method implements "A* Pathfinding" algorithm in order to find path between two position. </summary>
    /// <param name="startPosition"> Start position parameter. </param>
    /// <param name="endPosition"> End position parameter. </param>
    /// <returns> Returns the shortest path as a list. </returns>
    public List<Node> AStarPathfinder(Vector2Int startPosition, Vector2Int endPosition)
    {
        // The start and end nodes are assigned here.
        var startNode = ActualGridArray[startPosition.x, startPosition.y];
        var endNode = ActualGridArray[endPosition.x, endPosition.y];

        // The open and closed lists are created here.
        var openList = new List<Node> { startNode };
        var closedList = new List<Node>();

        // It is checked that the end node is occupied or not.
        if (ActualGridArray[endPosition.x, endPosition.y].IsOccupied) return null;

        // Array of grids is reset here.
        // TODO: If the node array is large, this part will take a quite while. Resetting the updated grids which may stored in list will take less time.
        for (var x = 0; x < ActualGridArray.GetLength(0); x++)
        {
            for (var y = 0; y < ActualGridArray.GetLength(1); y++)
            {
                ActualGridArray[x, y].GCost = int.MaxValue;
                ActualGridArray[x, y].FCost = ActualGridArray[x, y].GCost + ActualGridArray[x, y].HCost;
                ActualGridArray[x, y].PreviousValidNode = null;
            }
        }

        // The cost values of for start node is calculated.
        startNode.GCost = 0;
        startNode.HCost = CalculateDistance(startNode, endNode);
        startNode.FCost = startNode.GCost + startNode.HCost;

        // This loop continues until there is no node in open list.
        while (openList.Count != 0)
        {
            // Adjacent nodes are compared in order to select the one with the lowest fCode.
            // The node with the lowest fCode is assigned as a current node.
            var currentNode = GetNodeWithLowestFCost(openList);

            // If current node is the end node, it means that the algorithm finds the path.
            if (currentNode == endNode)
            {
                // The path will be calculated and stored as a list.
                return CalculatePath(endNode);
            }

            // If not...
            // Current node is stored in closed list and removed from open list.
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Adjacent nodes are get as a list.
            var adjacentNodeList = GetAdjacentNodes(currentNode);

            // Each node in adjacent node list is iterates here.
            foreach (var adjacentNode in adjacentNodeList)
            {
                // If adjacent node is in the closed list, loop will ignore this step and continue.
                if (closedList.Contains(adjacentNode)) continue;

                // If not...
                // The gCost of adjacent node will be calculated.
                var adjacentGCost = currentNode.GCost + CalculateDistance(currentNode, adjacentNode);
                // If the calculated gCost is greater or equal than the current gCost of the adjacent node, loop will ignore this step and continue.
                if (adjacentGCost >= adjacentNode.GCost) continue;

                // If not...
                // Attributes of adjacent node are updated.
                adjacentNode.PreviousValidNode = currentNode;
                adjacentNode.GCost = adjacentGCost;
                adjacentNode.HCost = CalculateDistance(adjacentNode, endNode);
                adjacentNode.FCost = adjacentNode.GCost + adjacentNode.HCost;

                // If the open list contains adjacent node, loop will ignore this step and continue.
                // If not, adjacent node will be added to the open list.
                if (!openList.Contains(adjacentNode))
                {
                    openList.Add(adjacentNode);
                }
            }
        }

        // Returns null
        return null;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// CalculatePath
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method calculates the path and stores it in a list. </summary>
    /// <param name="endNode"> The end node parameter </param>
    /// <returns> Returns the shortest path as a list. </returns>
    private List<Node> CalculatePath(Node endNode)
    {
        // New node list of nodes is created here and the end node is added to the list.
        var path = new List<Node> { endNode };
        // The end node is assigned as current node.
        var currentNode = endNode;

        // This loop iterates back until it reaches to the start node.
        while (currentNode.PreviousValidNode != null)
        {
            // The previous node is added to the node list.
            path.Add(currentNode.PreviousValidNode);
            // The current node is assigned as a previous node.
            currentNode = currentNode.PreviousValidNode;
        }

        // List is reversed here.
        path.Reverse();
        // Returns list.
        return path;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// GetAdjacentNodes
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method gets adjacent nodes that is not occupied. </summary>
    /// <param name="node"> The center node parameter. </param>
    /// <returns> Returns the adjacent nodes as a list. </returns>
    internal List<Node> GetAdjacentNodes(Node node)
    {
        // New list of nodes is created here.
        var adjacentNodesList = new List<Node>();

        // Adjacent nodes are iterates here
        for (var x = (int)node.Position.x - 1; x <= (int)node.Position.x + 1; x++)
        {
            for (var y = (int)node.Position.y - 1; y <= (int)node.Position.y + 1; y++)
            {
                // Ignores itself.
                if ((node.Position.x == x && node.Position.y == y)) continue;

                // Ignores nodes that is occupied already.
                if (ActualGridArray[x, y].IsOccupied) continue;

                // The rest are added to the list.
                adjacentNodesList.Add(ActualGridArray[x, y]);
            }
        }

        return adjacentNodesList;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// CalculateDistance
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method calculates the distance between two nodes with using movement costs. </summary>
    /// <param name="startNode"> The start node parameter. </param>
    /// <param name="endNode"> The end node parameter. </param>
    /// <returns> Returns the distance as an integer value </returns>
    internal int CalculateDistance(Node startNode, Node endNode)
    {
        var distanceX = Mathf.Abs(startNode.Position.x - endNode.Position.x);
        var distanceY = Mathf.Abs(startNode.Position.y - endNode.Position.y);
        var remaining = Mathf.Abs(distanceX - distanceY);
        return (int)((MovementCostStraight * remaining) +
                     MovementCostDiagonal * (Mathf.Min(distanceX, distanceY)));
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// GetNodeWithLowestFCost
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method find the node with the lowest fCost in a list. </summary>
    /// <param name="gridList"> The node list parameter. </param>
    /// <returns> Returns the node with the lowest fCost. </returns>
    internal Node GetNodeWithLowestFCost(List<Node> gridList)
    {
        // The first element in list is assigned as a node with the lowest fCode.
        var nodeWithLowestFCost = gridList[0];

        // The rest of the list iterates here in order to find the lowest one.
        for (var i = 1; i < gridList.Count; i++)
        {
            if (gridList[i].FCost < nodeWithLowestFCost.FCost)
            {
                nodeWithLowestFCost = gridList[i];
            }
        }

        // Return the node with the lowest fCode.
        return nodeWithLowestFCost;
    }

    #endregion

}
