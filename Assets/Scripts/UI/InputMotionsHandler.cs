using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMotionsHandler : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool tapAllowed = false;
    [SerializeField] private float tapDuration = 0.4f;
    [SerializeField] private float tapDeadZone = 100f;

    public static event Action PlayerTapInputEvent;
    public static event Action PlayerSwipeRightInputEvent;
    public static event Action PlayerSwipeLeftInputEvent;
    public static event Action PlayerSwipeUpInputEvent;
    public static event Action PlayerSwipeDownInputEvent;

    public void RecordDragMotion()
    {
        StopAllCoroutines();
        tapAllowed = true;
        startPoint = Input.mousePosition;
        StartCoroutine(TapAllowance());
    }

    public void ExecuteDesiredInput()
    {
        endPoint = Input.mousePosition;
        Vector2 swipeVector = endPoint - startPoint;
        if (tapAllowed && swipeVector.magnitude < tapDeadZone)
        {
            PlayerTapInputEvent?.Invoke();
            //Debug.Log("Detected tap input");
        }
        else if (swipeVector.magnitude > tapDeadZone)
        {
            if (swipeVector.normalized.x > 0.7f) // 0.7f is just quick hillbilly shorthand to indicate passing the 45% normalized mark
            {
                PlayerSwipeRightInputEvent?.Invoke();
                //Debug.Log("Detected swipe right input");
            }
            else if (swipeVector.normalized.x < -0.7f)
            {
                PlayerSwipeLeftInputEvent?.Invoke();
                //Debug.Log("Detected swipe left input");
            }
            else if (swipeVector.normalized.y < -0.7f)
            {
                PlayerSwipeDownInputEvent?.Invoke();
                //Debug.Log("Detected swipe down input");
            }
            else if (swipeVector.normalized.y > 0.7f)
            {
                PlayerSwipeUpInputEvent?.Invoke();
                //Debug.Log("Detected swipe up input");
            }
        }
    }

    private IEnumerator TapAllowance()
    {
        yield return new WaitForSecondsRealtime(tapDuration);
        tapAllowed = false;
    }
}
