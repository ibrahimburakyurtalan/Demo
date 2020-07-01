using UnityEngine;

public class Managers : MonoBehaviour
{
    #region Attributes

    // This attribute represents the static instance.
    public static Managers Instance = null;
    // This attribute represents the event manager script, it is filled by user.
    public EventManager eventManager = null;
    // This attribute represents the grid manager script, it is filled by user.
    public GridManager gridManager = null;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Awake
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Awake method of MonoBehaviour interface. </summary>
    private void Awake()
    {
        // Singleton pattern here.
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion
}
