using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    // This attribute represents the name of the unit. It is filled by user in inspector.
    [SerializeField] private string unitName;
    // This attribute represents the color of the unit. It is filled by user in inspector.
    [SerializeField] private Color unitColor;
    // This attribute represents the color of the product. It is filled by user in inspector.
    [SerializeField] private Color productionColor;

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OnMouseDown
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The OnMouseDown method of MonoBehaviour class. </summary>
    private void OnMouseDown()
    {
        // If the pointer is on the UI element, returns.
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        // If not..
        // Setting information event will be triggered.
        var gameObjectTransform = transform;
        Managers.Instance.eventManager.EventSetInformation.Invoke(unitName, unitColor, productionColor, gameObjectTransform);
    }
}
