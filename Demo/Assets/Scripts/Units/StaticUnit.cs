using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUnit : Unit
{
    #region Constructor

    /// <summary> This constructor initializes class with sizes, color and grid array. </summary>
    /// <param name="sizeX"> X size of the unit parameter. </param>
    /// <param name="sizeY"> Y size of the unit parameter. </param>
    /// <param name="color"> Color value of the unit parameter. </param>
    /// <param name="actualGridArray"> Array of grids parameter. </param>
    public StaticUnit(float sizeX, float sizeY, Color color, Node[,] actualGridArray) : base(sizeX, sizeY, color, actualGridArray)
    {
    }

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// CalculateObjectPosition
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method calculates the object position and snaps them to the grids. </summary>
    /// <returns> Returns the object position. </returns>
    public Vector3 CalculateObjectPosition()
    {
        // Bool variables that represent the x and y sizes are even or not.
        var isSizeEvenX = (Math.Abs(SizeX % 2) < 0.1);
        var isSizeEvenY = (Math.Abs(SizeY % 2) < 0.1);

        // Snapped mouse position is calculated here.
        var objectPosition = GetMouseWorldPosition();
        objectPosition.x = SnapToGrid(objectPosition.x, isSizeEvenX);
        objectPosition.y = SnapToGrid(objectPosition.y, isSizeEvenY);

        // Returning the snapped position.
        return objectPosition;
    }

    #endregion
}
