using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroller : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{
    #region Attributes

    // This attribute represent the content script of content game object. It is filled by user.
    [SerializeField] private Content content;
    // This attribute represents the RectTransform of canvas. It is filled by user.
    [SerializeField] private RectTransform canvas;
    // This attribute represents the pooling threshold. It is filled by user.
    [SerializeField] private float threshold;
    // This attribute represents the ScrollRect component.
    private ScrollRect scrollRect;
    // This attribute represents the input mouse position when dragging.
    private float dragPositionY;
    // This attribute represents dragging is positive or negative.
    private bool isDragPositive;
    // This attribute represents upper limit of the plane.
    private const float UpperLimit = 880f;
    // This attribute represents lower limit of the plane.
    private const float LowerLimit = -160f;
    // This attribute represents lower gap between contents.
    private const float GapBetweenContents = 48f;

    #endregion

    #region Methods

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Start
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The Start method of MonoBehaviour class. </summary>
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.scrollSensitivity = 32;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OnBeginDrag
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The OnBeginDrag method of IBeginDragHandler interface. </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPositionY = eventData.position.y;
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OnDrag
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The OnDrag method of IDragHandler interface. </summary>
    public void OnDrag(PointerEventData eventData)
    {
        isDragPositive = eventData.position.y > dragPositionY;
        dragPositionY = eventData.position.y;
        OrganizeContents();
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OnScroll
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> The OnScroll method of IScrollHandler interface. </summary>
    public void OnScroll(PointerEventData eventData)
    {
        isDragPositive = !(eventData.scrollDelta.y > 0);
        OrganizeContents();
    }


    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// OrganizeContents
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method organizes the contents in order to obtain infinite scrolling by using object pooling method. </summary>
    private void OrganizeContents()
    {
        // UI scale factor is set here.
        var uiScaleFactor = canvas.localScale.x;

        // Indexes of current and next contents are set here by checking the isDragPositive booelan.
        var currentRightIndex = isDragPositive ? (scrollRect.content.childCount / 2) : (scrollRect.content.childCount - 1);
        var currentLeftIndex = isDragPositive ? 0 : ((scrollRect.content.childCount / 2) - 1);
        var nextRightIndex = isDragPositive ? (scrollRect.content.childCount - 1) : (scrollRect.content.childCount / 2);
        var nextLeftIndex = isDragPositive ? ((scrollRect.content.childCount / 2) - 1) : 0;

        // Current and next contents are called here by using indexes.
        var currentRightContent = scrollRect.content.GetChild(currentRightIndex);
        var currentLeftContent = scrollRect.content.GetChild(currentLeftIndex);
        var nextRightContent = scrollRect.content.GetChild(nextRightIndex);
        var nextLeftContent = scrollRect.content.GetChild(nextLeftIndex);

        // If the scrolling position do not exceed the threshold, the pooling is ignored.
        if (!CheckThresholds(currentRightContent.position.y, uiScaleFactor)) { return; }

        // If not..
        // Content height is gotten here.
        var objectHeight = nextRightContent.GetComponent<RectTransform>().rect.height;
        // If scrolling is in positive side, contents in bottom side are repositioned to the upper side.
        if (isDragPositive)
        {
            var newValueRight = nextRightContent.position.y - ((objectHeight + GapBetweenContents) * uiScaleFactor);
            var newValueLeft = nextLeftContent.position.y - ((objectHeight + GapBetweenContents) * uiScaleFactor);

            currentRightContent.position = new Vector3(currentRightContent.position.x, newValueRight, currentRightContent.position.z);
            currentLeftContent.position = new Vector3(currentLeftContent.position.x, newValueLeft, currentLeftContent.position.z);
        }
        // If scrolling is in negative side, contents in upper side are repositioned to the bottom side.
        else
        {
            var newValueRight = nextRightContent.position.y + ((objectHeight + GapBetweenContents) * uiScaleFactor);
            var newValueLeft = nextLeftContent.position.y + ((objectHeight + GapBetweenContents) * uiScaleFactor);

            currentRightContent.position = new Vector3(currentRightContent.position.x, newValueRight, currentRightContent.position.z);
            currentLeftContent.position = new Vector3(currentLeftContent.position.x, newValueLeft, currentLeftContent.position.z);
        }

        // Contents that are repositioned are organized.
        currentRightContent.SetSiblingIndex(nextRightIndex);
        currentLeftContent.SetSiblingIndex(nextLeftIndex);
    }

    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// CheckThresholds
    /// -------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary> This method checks the scrolling position exceeds the threshold value or not. </summary>
    /// <param name="contentPosY"> Scrolling position 'y' parameter. </param>
    /// <param name="scale"> Scale factor parameter </param>
    /// <returns> Returns true if exceeds </returns>
    private bool CheckThresholds(float contentPosY, float scale)
    {

        var upperLimit = (UpperLimit + threshold) * scale;
        var lowerLimit = (LowerLimit - threshold) * scale;

        return isDragPositive ? (contentPosY > upperLimit) : (contentPosY < lowerLimit);
    }

    #endregion
}
