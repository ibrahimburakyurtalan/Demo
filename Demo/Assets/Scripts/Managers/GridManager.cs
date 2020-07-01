using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Attributes

    // This attribute represents the grid array.
    public Node[,] Grids;
    [Header("Node sizes here.")]
    // This attribute represents the grid x size. It is filled by user.
    [SerializeField] [Min(32)] private int gridSizeX = 32;
    // This attribute represents the grid y size. It is filled by user.
    [SerializeField] [Min(32)] private int gridSizeY = 32;
    // This attribute represents the grid prefab. It is filled by user.
    [Header("Node prefab here.")]
    [SerializeField] private GameObject grid = null;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour interface. </summary>
    private void Start()
    {
        // Grid array is initialized with the given x and y size.
        Grids = new Node[gridSizeX, gridSizeY];

        // Parent object will be created
        var parent = new GameObject { name = "Grids" };

        // Grid array is generated here. Prefabs are placed as grid.
        for (var x = 0; x < gridSizeX; x++)
        {
            for (var y = 0; y < gridSizeY; y++)
            {
                var gameObj = Instantiate(grid, parent.transform);
                gameObj.transform.position = new Vector2(x, y);
                gameObj.name = "Node " + x + "," + y;

                Grids[x, y] = new Node { Position = new Vector2(x, y), IsOccupied = false };
            }
        }

        // Set camera event is invoked here.
        Managers.Instance.eventManager.EventSetCamera.Invoke(gridSizeX / 2, gridSizeY / 2);
    }

    #endregion
}
