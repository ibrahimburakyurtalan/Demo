using UnityEngine;
using UnityEngine.EventSystems;

public class Buildings : MonoBehaviour
{
    #region Attributes

    // This attribute represents the color of the unit. It is filled by user.
    [SerializeField] private Color colorUnit;
    // This attribute represents the x size of the unit. It is filled by user.
    [SerializeField] private float sizeX;
    // This attribute represents the y size of the unit. It is filled by user.
    [SerializeField] private float sizeY;
    // This attribute represents the sprite renderer component.
    private SpriteRenderer spriteRenderer;
    // This attribute represents the static unit instance.
    public StaticUnit Building;
    // This attribute represents if movement is enabled or not..
    private bool movementEnable = true;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour class. </summary>
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Building = new StaticUnit(sizeX, sizeY, colorUnit, Managers.Instance.gridManager.Grids);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Update
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Update method of MonoBehaviour class. </summary>
    private void Update()
    {
        // If movement is disabled, loop will be ignored.
        if (!movementEnable) return;

        // If not...
        // Transform position will calculated by using mouse position.
        // Also snapping mechanism are done here.
        var position = Building.CalculateObjectPosition();
        transform.position = position;

        // Grids at the transform position will be checked if they are occupied or not.
        var areGridsFree = Building.CheckGrids(Managers.Instance.gridManager.Grids[(int)position.x, (int)position.y]);
        // Color will be selected according to the occupancy.
        spriteRenderer.color = Building.ColorizeObject(areGridsFree);

        // If there is no right click, loop will be ignored.
        if (!Input.GetMouseButtonDown(0)) return;

        // If not...
        // It will be checked that pointer is on UI element or not and the clicked grid is free or not. If so, game object will be destroyed.
        if (EventSystem.current.IsPointerOverGameObject() || !areGridsFree)
        {
            Destroy(gameObject);
            return;
        }

        // If not...
        // Building will be positioned to the click position. And the grids will be set as occupied.
        Building.OccupyGrids(Managers.Instance.gridManager.Grids[(int)position.x, (int)position.y]);
        // Default color of the building will be set.
        spriteRenderer.color = Building.ColorizeObject(false, true);
        // Movement will be disabled.
        movementEnable = false;
    }

    #endregion
}
