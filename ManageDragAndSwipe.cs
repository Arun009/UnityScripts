using UnityEngine;
using UnityEngine.EventSystems;

public class ManageDragAndSwipe : MonoBehaviour
{
    private Vector3 objectPositionWRTCam;
    private Vector3 offset;
    private Vector3 objectInitialPosition;

    private Vector2 startTouch;
    private Vector2 swipeDelta;

    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;

    private bool moveChoiceA;
    private bool moveChoiceB;
    private bool moveChoiceC;
    private bool moveChoiceD;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        
        beginDrag.eventID = EventTriggerType.BeginDrag;
        onDrag.eventID = EventTriggerType.Drag;
        endDrag.eventID = EventTriggerType.EndDrag;
        
        beginDrag.callback.AddListener((data) => { OnBeginDragDelegate((PointerEventData)data); });
        onDrag.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        endDrag.callback.AddListener((data) => { OnEndDragDelegate((PointerEventData)data); });
        
        trigger.triggers.Add(beginDrag);
        trigger.triggers.Add(onDrag);
        trigger.triggers.Add(endDrag);

        objectInitialPosition = transform.position;
        swipeDelta = Vector2.zero;
    }

    public void OnBeginDragDelegate(PointerEventData data)
    {
        if (!GameController.Instance.disableInputs)
        {
            objectPositionWRTCam = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(data.position.x, data.position.y, objectPositionWRTCam.z));
            transform.SetAsLastSibling();

            startTouch = data.position;
        }
    }
    public void OnDragDelegate(PointerEventData data)
    {
        swipeDelta = data.position - startTouch;
        if (!GameController.Instance.disableInputs)
        {
            Vector3 screenPoint = new Vector3(data.position.x, data.position.y, objectPositionWRTCam.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(screenPoint) + offset;
            transform.position = currentPosition;
        }
    }
    public void OnEndDragDelegate(PointerEventData data)
    {
        if (!GameController.Instance.disableInputs)
            IdentifySwipeDirection();
    }

    void IdentifySwipeDirection()
    {
        if (swipeDelta.magnitude >= 40)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or Right
                if (x < 0)
                {
                    swipeLeft = true;
                    if (transform.gameObject.tag.Equals("ChoiceB"))
                        moveChoiceB = true;
                    else
                        Reset();
                }
                else
                {
                    swipeRight = true;
                    if (transform.gameObject.tag.Equals("ChoiceD"))
                        moveChoiceD = true;
                    else
                        Reset();
                }
            }
            else
            {
                // Up or Down
                if (y < 0)
                {
                    swipeDown = true;
                    if (transform.gameObject.tag.Equals("ChoiceC"))
                        moveChoiceC = true;
                    else
                        Reset();
                }
                else
                {
                    swipeUp = true;
                    if (transform.gameObject.tag.Equals("ChoiceA"))
                        moveChoiceA = true;
                    else
                        Reset();
                }
            }
        }
        startTouch = swipeDelta = Vector2.zero;
        swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        transform.position = objectInitialPosition;
        swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }

    private void FixedUpdate()
    {
        if (moveChoiceA)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 200.0f);
            if (transform.position.y - objectInitialPosition.y > 30.0f)
            {
                moveChoiceA = false;
                transform.gameObject.SetActive(false);
                GameController.Instance.choiceAisSelected = true;
            }
        }
        else if (moveChoiceB)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 200.0f);
            if (transform.position.x - objectInitialPosition.x < -30.0f)
            {
                moveChoiceB = false;
                transform.gameObject.SetActive(false);
                GameController.Instance.choiceBisSelected = true;
            }
        }
        else if (moveChoiceC)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 200.0f);
            if (transform.position.y - objectInitialPosition.y < -30.0f)
            {
                moveChoiceC = false;
                transform.gameObject.SetActive(false);
                GameController.Instance.choiceCisSelected = true;
            }
        }
        else if (moveChoiceD)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 200.0f);
            if (transform.position.x - objectInitialPosition.x > 30.0f)
            {
                moveChoiceD = false;
                transform.gameObject.SetActive(false);
                GameController.Instance.choiceDisSelected = true;
            }
        }
    }
}
