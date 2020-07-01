using UnityEditor.Build.Reporting;
using UnityEngine;

public class InitialCameraPosition : MonoBehaviour
{
    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour class. </summary>
    private void Start()
    {
        Managers.Instance.eventManager.EventSetCamera.AddListener(EventSetCameraTriggered);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// EventSetCameraTriggered
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method positions the main camera to the center of the nodes. </summary>
    /// <param name="x"> Start position parameter. </param>
    /// <param name="y"> End position parameter. </param>
    private void EventSetCameraTriggered(int x, int y)
    {
        // Camera position is set here.
        transform.position = new Vector3(x, y, -10);
        // All listeners in "EventSetCamera" event are removed.
        Managers.Instance.eventManager.EventSetCamera.RemoveAllListeners();
        // The script is destroyed.
        Destroy(this);
    }

    #endregion
}
