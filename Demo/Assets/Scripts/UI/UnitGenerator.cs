using UnityEngine;
using UnityEngine.EventSystems;

public class UnitGenerator : MonoBehaviour, IPointerClickHandler
{
    #region Attributes

    // This attributes represents the unit, it is filled by user.
    [SerializeField] private GameObject unit;
    // This attributes represents the unit name, it is filled by user.
    [SerializeField] private string unitName;
    // This attributes represents the unit count value, it is incremented when unit is instantiated.
    private int count = 0;
    // This attributes represents the bounded unit. (e.g. Soldier -> Barrack)
    private GameObject parentObject;
    // This attributes represents the bounded unit transform.
    public Transform BoundedUnitTransform;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OnPointerClick
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The OnPointerClick method of IPointerClickHandler interface. </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // If user is not entered the correct unit, returns.
        if (unit == null) return;

        // If first time. Parent object will be created in order to maintain order.
        if (count == 0) parentObject = new GameObject(unitName + "'s Parent");

        // The unit is instantiated.
        var unitObject = Instantiate(unit, parentObject.transform);
        // The unit is renamed.
        unitObject.name = unitName + " " + count;
        // The unit count value is incremented.
        count++;

        // If this script is not run by producer, returns.
        if (gameObject != null && gameObject.tag != "ProducerButton") return; ;
        // Soldier script is gotten here.
        var soldier = unitObject.GetComponent<Soldier>();
        // The bound transform is transferred to the soldier script.
        soldier.BoundedUnitTransform = BoundedUnitTransform;
    }

    #endregion
}
