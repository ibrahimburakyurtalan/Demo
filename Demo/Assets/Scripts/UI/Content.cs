using UnityEngine;

public class Content : MonoBehaviour
{
    #region Attributes

    // This attributes represents the contents in this game object.
    [HideInInspector] public RectTransform[] Contents;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour class. </summary>
    private void Start()
    {
        // RectTransform component is gotten here.
        var rectTransform = GetComponent<RectTransform>();
        // The RectTransform array is generated here.
        Contents = new RectTransform[rectTransform.childCount];
        // This loop iterates until it reaches to the final child game object.
        for (var i = 0; i < rectTransform.childCount; i++)
        {
            // Array is filled with the RectTransform component of child game objects.
            Contents[i] = rectTransform.GetChild(i) as RectTransform;
        }
    }

    #endregion
}
