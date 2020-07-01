using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    #region Attributes
    
    // This attribute represents the text component. It is filled by user.
    [SerializeField] private TextMeshProUGUI textUnit;
    // This attribute represents the unit image component. It is filled by user.
    [SerializeField] private Image imageUnit;
    // This attribute represents the production image component. It is filled by user.
    [SerializeField] private Image imageProduction;
    // This attribute represents the unit generator script. It is filled by user.
    [SerializeField] private UnitGenerator unitGenerator;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour class. </summary>
    private void Start()
    {
        // Listener is added to the "EventSetInformation" event.
        Managers.Instance.eventManager.EventSetInformation.AddListener(EventSetInformationTriggered);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// EventSetInformationTriggered
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This callback method organizes the information menu. When user clicks an object this event is invoked. </summary>
    /// <param name="name"> Unit name parameter </param>
    /// <param name="imageUnitColor"> Unit image parameter </param>
    /// <param name="imageProductionColor"> Production color parameter </param>
    /// <param name="boundedUnitTransform"> Bounded unit transform parameter </param>
    /// <returns> Returns true if exceeds </returns>
    private void EventSetInformationTriggered(string name, Color imageUnitColor, Color imageProductionColor, Transform boundedUnitTransform)
    {
        switch (name)
        {
            // When barrack is chosen, information menu is organized with image and production. Also, corresponding barrack unit is referenced to the production.
            case "BARRACK":
                textUnit.text = name;
                imageUnit.color = imageUnitColor;
                imageProduction.color = imageProductionColor;
                imageUnit.gameObject.SetActive(true);
                imageProduction.gameObject.SetActive(true);
                unitGenerator.BoundedUnitTransform = boundedUnitTransform;
                break;
            // When power plant is chosen, information menu is organized with image.
            case "POWER\nPLANT":
                textUnit.text = name;
                imageUnit.color = imageUnitColor;
                imageUnit.gameObject.SetActive(true);
                imageProduction.gameObject.SetActive(false);
                break;
            // When others, information menu is cleared.
            default:
                imageUnit.gameObject.SetActive(false);
                imageProduction.gameObject.SetActive(false);
                break;
        }
    }

    #endregion
}
