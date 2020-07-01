using System.Collections.Generic;
using UnityEngine;

public class DynamicUnit : Unit
{
    #region Attributes

    // This attribute represents the pathfinder instance.
    public Pathfinder Pathfinder;

    #endregion

    #region Constructor
    /// <summary> This constructor initializes class with sizes, color and grid array. </summary>
    /// <param name="sizeX"> X size of the unit parameter. </param>
    /// <param name="sizeY"> Y size of the unit parameter. </param>
    /// <param name="color"> Color value of the unit parameter. </param>
    /// <param name="actualGridArray"> Array of grids parameter. </param>
    public DynamicUnit(float sizeX, float sizeY, Color color, Node[,] actualGridArray) : base(sizeX, sizeY, color, actualGridArray)
    {
        // New pathfinder instance is created.
        Pathfinder = new Pathfinder(actualGridArray);
    }

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// FindTheShortestPath
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method calls the "A* pathfinding" algorithm. </summary>
    /// <param name="startPosition"> Start position parameter. </param>
    /// <param name="endPosition"> End position parameter. </param>
    /// <returns> Returns the shortest path as a list. </returns>
    public List<Node> FindTheShortestPath(Vector2Int startPosition, Vector2Int endPosition)
    {
        // Calls the "A* pathfinding" algorithm for two positions.
        return Pathfinder.AStarPathfinder(startPosition, endPosition);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// CheckGrids
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method overrides the actual method in unit class. Basically gets the occupancy of related node. </summary>
    /// <param name="node"> Node parameter. </param>
    /// <returns> Returns the occupancy of the related node. </returns>
    public override bool CheckGrids(Node node)
    {
        // Gets the occupancy of the related node.
        return ActualGridArray[(int)node.Position.x, (int)node.Position.y].IsOccupied;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OccupyGrids
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method overrides the actual method in unit class. Basically sets the occupancy of related node. </summary>
    /// <param name="node"> Node parameter. </param>
    /// <param name="occupyType"> Occupancy set parameter. </param>
    /// <returns> Sets the occupancy of the related node. </returns>
    public override void OccupyGrids(Node node, bool occupyType = true)
    {
        // Sets the occupancy of the related node.
        ActualGridArray[(int)node.Position.x, (int)node.Position.y].IsOccupied = occupyType;
    }

    #endregion
}
