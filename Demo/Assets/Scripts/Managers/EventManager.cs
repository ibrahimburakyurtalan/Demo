using UnityEngine;
using UnityEngine.Events;

public class CameraEvent : UnityEvent<int, int> { }
public class InformationEvent : UnityEvent<string, Color, Color, Transform> { }
public class EventManager : MonoBehaviour
{
    #region Attributes

    // This attribute represent the camera event.
    public CameraEvent EventSetCamera = null;
    // This attribute represent the information event.
    public InformationEvent EventSetInformation = null;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Awake
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Awake method of MonoBehaviour interface. </summary>
    private void Awake()
    {
        // Event instances are created.
        EventSetCamera = new CameraEvent();
        EventSetInformation = new InformationEvent();
    }

    #endregion
}