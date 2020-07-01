using UnityEngine;

public class Unit
{
    #region Attributes

    // This attributes represents the x size of the unit.
    protected float SizeX;
    // This attributes represents the y size of the unit.
    protected float SizeY;
    // This attributes represents the color of the unit.
    private readonly Color color;
    // This attributes represents the grid array.
    public Node[,] ActualGridArray;

    #endregion

    #region Constructor

    /// <summary> This constructor initializes class with sizes, color and grid array. </summary>
    /// <param name="sizeX"> X size of the unit parameter. </param>
    /// <param name="sizeY"> Y size of the unit parameter. </param>
    /// <param name="color"> Color value of the unit parameter. </param>
    /// <param name="actualGridArray"> Array of grids parameter. </param>
    public Unit(float sizeX, float sizeY, Color color, Node[,] actualGridArray)
    {
        ActualGridArray = actualGridArray;
        SizeX = sizeX;
        SizeY = sizeY;
        this.color = color;
    }

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// GetMouseWorldPosition
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method gets the mouse position in world points. </summary>
    /// <returns> Returns the mouse position. </returns>
    public Vector2 GetMouseWorldPosition()
    {
        // Gets the mouse position in world points.
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Returns the related position as a Vector2.
        var position = new Vector2(mousePosition.x, mousePosition.y);
        return position;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// SnapToGrid
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method re-calculates the position values(x and y) in order to provide snapping mechanism. </summary>
    /// <param name="value"> The position value parameter. (x or y) </param>
    /// <param name="isSizeEven"> If unit size is even or not parameter. </param>
    /// <returns> Returns the shortest path as a list. </returns>
    public float SnapToGrid(float value, bool isSizeEven = true)
    {
        // The value is floored here.
        var floorValue = Mathf.Floor(value);
        // The difference between the actual and floored values is calculated here.
        var commaValue = value - floorValue;

        // Checking if size is even or not. If so, returns floor value.
        if (!isSizeEven) { return floorValue; }

        // If not...
        // If the difference is greater or equal than the .5f, floor value is incremented by .5f. If not, decremented by .5f.
        var gridValue = (commaValue >= 0.5) ? (floorValue + 0.5f) : (floorValue - 0.5f);
        return gridValue;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// CheckGrids
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> Gets the occupancies of related nodes by checking unit's size.  </summary>
    /// <param name="node"> The related node parameter. </param>
    /// <returns> Returns the occupancies of the related node. </returns>
    public virtual bool CheckGrids(Node node)
    {
        // The edges of unit are calculated here.
        var lowerXValue = (int)(Mathf.Floor(node.Position.x - (SizeX / 2)) + 1);
        var lowerYValue = (int)(Mathf.Floor(node.Position.y - (SizeY / 2)) + 1);
        var upperXValue = (int)(Mathf.Floor(node.Position.x + (SizeX / 2)) + 1);
        var upperYValue = (int)(Mathf.Floor(node.Position.y + (SizeY / 2)) + 1);

        // All nodes in the unit area are checked if any one of them is occupied or not.
        // If so, returns as occupied. If not, returns as free.
        for (var x = lowerXValue; x < upperXValue; x++)
        {
            for (var y = lowerYValue; y < upperYValue; y++)
            {
                var isOccupied = ActualGridArray[x, y].IsOccupied;
                if (isOccupied) { return false; }
            }
        }
        return true;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OccupyGrids
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> Sets the occupancies of related nodes by checking unit's size. </summary>
    /// <param name="node"> Node parameter. </param>
    /// <param name="occupyType"> Occupancy set parameter. </param>
    /// <returns> Sets the occupancy of the related nodes. </returns>
    public virtual void OccupyGrids(Node node, bool occupyType = true)
    {
        // The edges of unit are calculated here.
        var lowerXValue = (int)(Mathf.Floor(node.Position.x - (SizeX / 2)) + 1);
        var lowerYValue = (int)(Mathf.Floor(node.Position.y - (SizeY / 2)) + 1);
        var upperXValue = (int)(Mathf.Floor(node.Position.x + (SizeX / 2)) + 1);
        var upperYValue = (int)(Mathf.Floor(node.Position.y + (SizeY / 2)) + 1);

        // All nodes in the unit area will be set as "occupyType".
        for (var x = lowerXValue; x < upperXValue; x++)
        {
            for (var y = lowerYValue; y < upperYValue; y++)
            {
                ActualGridArray[x, y].IsOccupied = occupyType;
            }
        }
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// ColorizeObject
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> Sets the color to the red, green or default value of the unit. </summary>
    /// <param name="change"> Change parameter. </param>
    /// <param name="backToInitial"> Back to initial parameter. </param>
    /// <returns> Returns the color. </returns>
    public Color ColorizeObject(bool change = false, bool backToInitial = false)
    {
        // Checks "backToInitial" parameter. If it is true, color will be set as default color of the unit.
        if (backToInitial)
        {
            return color;
        }

        // If not...
        // If change is requested, color will be set as green. If not, it will be set as red.
        return change ? Color.green : Color.red;
    }

    #endregion
}
