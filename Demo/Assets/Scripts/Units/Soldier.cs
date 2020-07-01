using System.Collections;
using UnityEngine;
using static System.String;

public class Soldier : MonoBehaviour
{
    #region Attributes

    // This attribute represents the bounded unit transform.
    public Transform BoundedUnitTransform;
    // This attribute represents the state diagram of the soldier.
    enum StatesOfSoldier { Place, Select, Move }
    private StatesOfSoldier soldierState = StatesOfSoldier.Place;
    // This attribute represents the color of the soldier.
    [SerializeField] private Color colorUnit;
    // This attribute represents the sprite renderer component.
    private SpriteRenderer spriteRenderer;
    // This attribute represents to dynamic unit instance.
    private DynamicUnit dynamicUnit;
    // This attribute represents if soldier is selected or not.
    private bool isSoldierSelected;
    // This attribute represents if soldier is moving or not.
    private bool isSoldierMoving;
    // This attribute represents sizes of the soldier..
    private const float SizeXSoldier = 1f;
    private const float SizeYSoldier = 1f;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour class. </summary>
    private void Start()
    {
        // Sprite renderer component is gotten here.
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Dynamic unit instance is created here.
        dynamicUnit = new DynamicUnit(SizeXSoldier, SizeYSoldier, colorUnit, Managers.Instance.gridManager.Grids);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Update
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Update method of MonoBehaviour class. </summary>
    private void Update()
    {
        // Soldier state diagram
        switch (soldierState)
        {
            // This state represent the soldier is being placed right now.
            case StatesOfSoldier.Place:
                if (!PlaceSoldier()) return;

                soldierState = StatesOfSoldier.Select;
                break;

            // This state represent the soldier is being selected right now.
            case StatesOfSoldier.Select:
                SelectSoldier();
                soldierState = StatesOfSoldier.Move;
                break;

            // This state represent the soldier is being moved right now.
            case StatesOfSoldier.Move:
                SelectSoldier();
                MoveSoldier(isSoldierSelected);
                break;

            default:
                break;
        }

    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// PlaceSoldier
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This position check the surrounds of the bounded unit (barrack) and searches a free spot to position soldier. </summary>
    /// <returns> Returns true if free spot is found. </returns>
    /// TODO: while(true) loop may cause infinite loop. There has to be limit.
    private bool PlaceSoldier()
    {
        // The closest surroundings of the bounded unit are calculated here.
        var searchAreaMinX = (int)Mathf.Floor(BoundedUnitTransform.position.x - 2);
        var searchAreaMinY = (int)Mathf.Floor(BoundedUnitTransform.position.y - 2);
        var searchAreaMaxX = (int)Mathf.Ceil(BoundedUnitTransform.position.x + 2);
        var searchAreaMaxY = (int)Mathf.Ceil(BoundedUnitTransform.position.y + 2);

        while (true)
        {
            // Checks the surroundings area in order to find a free spot.
            for (var x = searchAreaMinX; x <= searchAreaMaxX; x++)
            {
                for (var y = searchAreaMinY; y <= searchAreaMaxY; y++)
                {
                    // This if condition ensures only outer area will be searched.
                    // Searching the inner area is both unnecessary and time consuming.
                    if ((x != searchAreaMinX) && (x != searchAreaMaxX) && (y != searchAreaMinY) && (y != searchAreaMaxY)) continue;
                    // This is condition ensured the related node is not occupied.
                    if (dynamicUnit.CheckGrids(dynamicUnit.ActualGridArray[x, y])) continue;

                    // If the if conditions above are satisfied. Soldier will be placed and related node will be occupied.
                    transform.position = new Vector3(x, y, 0);
                    dynamicUnit.OccupyGrids(dynamicUnit.ActualGridArray[x, y]);
                    return true;
                }
            }

            // If there is no free space in the area. Search space will be incremented.
            searchAreaMinX--;
            searchAreaMinY--;
            searchAreaMaxX++;
            searchAreaMaxY++;
        }
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// SelectSoldier
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> Soldier is selected by using ray cast. </summary>
    private void SelectSoldier()
    {
        // A ray cast is generated at the mouse click position.
        if (!Input.GetMouseButtonDown(0)) return;
        var rayPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        var hit = Physics2D.Raycast(rayPosition, Vector2.zero, 0f);

        // If ray cast hits a game object, it will be checked if the game object is soldier or not.
        isSoldierSelected = hit && (Compare(hit.transform.name, gameObject.name) == 0);

        // If so, soldier is colored as green. If not, colored as its default value.
        spriteRenderer.color = isSoldierSelected ? dynamicUnit.ColorizeObject(true) : dynamicUnit.ColorizeObject(false, true);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// MoveSoldier
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> When user clicks right button on mouse. Character movement is enabled. </summary>
    /// <param name="isSelected"> Soldier is selected or not parameter. </param>
    private void MoveSoldier(bool isSelected)
    {
        // If soldier is not selected, returns.
        if (!isSelected) return;
        // If there is no right click on mouse, returns.
        if (!Input.GetMouseButtonDown(1)) return;
        // If soldier is moving already, returns.
        if (isSoldierMoving) return;

        // If the conditions above are satisfied..
        // The transform position is set as start position.
        var startPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        // The current mouse position is set as end position.
        var endPositionVector2 = dynamicUnit.GetMouseWorldPosition();
        endPositionVector2.x = dynamicUnit.SnapToGrid(endPositionVector2.x);
        endPositionVector2.y = dynamicUnit.SnapToGrid(endPositionVector2.y);

        var endPositionX = Mathf.CeilToInt(endPositionVector2.x);
        var endPositionY = Mathf.CeilToInt(endPositionVector2.y);

        var endPosition = new Vector2Int(endPositionX, endPositionY);

        // Movement co-routine is called here.
        StartCoroutine(Move(startPosition, endPosition, 0.05f));
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Move
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> Shortest path is calculated and the soldier is moved step by step by getting the returning list indexes. </summary>
    /// <param name="startPosition"> Start position parameter.</param>
    /// <param name="endPosition"> End position parameter. </param>
    private IEnumerator Move(Vector2Int startPosition, Vector2Int endPosition, float waitValue)
    {
        // Soldier is moving right now.
        isSoldierMoving = true;

        // Shortest path is calculated and the path is assigned in a list.
        var shortestPath = dynamicUnit.Pathfinder.AStarPathfinder(startPosition, endPosition);
        
        // If there is no shortest path calculated, breaks.
        if (shortestPath == null)
        {
            isSoldierMoving = false;
            yield break;
        }

        // The node where the soldier is currently positioned are free right now.
        dynamicUnit.OccupyGrids(dynamicUnit.ActualGridArray[startPosition.x, startPosition.y], false);

        // Soldier will be moved by following the positions in the list step by step.
        while (shortestPath.Count != 0)
        {
            var firstElementInList = shortestPath[0];
            transform.position = firstElementInList.Position;
            shortestPath.Remove(firstElementInList);
            yield return new WaitForSeconds(waitValue);
        }

        // The reached node will be occupied.
        dynamicUnit.OccupyGrids(dynamicUnit.ActualGridArray[endPosition.x, endPosition.y]);

        // Soldier is not moving right now.
        isSoldierMoving = false;
    }

    #endregion
}
