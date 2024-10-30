using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatControls : MonoBehaviour
{
    [SerializeField] private float tapDuration = 0.2f;
    [SerializeField, Range(0f, 1f), Tooltip("Size of deadzone in relation to percentage of screen width")] private float deadZone = 0.01f;
    private float adjustedDeadZone;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 swipeVector;
    private bool detectingInput = false;
    private bool tapAllowed = false;
    private bool swipeExecuted = false;

    // Prob could refactor into a param of input enums instead
    public static event Action PlayerTapInputEvent;
    public static event Action PlayerSwipeRightInputEvent;
    public static event Action PlayerSwipeLeftInputEvent;
    public static event Action PlayerSwipeUpInputEvent;
    public static event Action PlayerSwipeDownInputEvent;
    public static event Action PlayerReleaseInputEvent;

    private void Update()
    {
        HandleSwipeLogic();
    }

    public void InputStart()
    {
        StopAllCoroutines();
        adjustedDeadZone = Screen.width * deadZone;
        detectingInput = true;
        tapAllowed = true;
        startPoint = Input.mousePosition;
        StartCoroutine(TapAllowance());
    }

    private IEnumerator TapAllowance()
    {
        yield return new WaitForSecondsRealtime(tapDuration);
        tapAllowed = false;
    }

    private void HandleSwipeLogic()
    {
        if (detectingInput && !swipeExecuted)
        {
            endPoint = Input.mousePosition;
            swipeVector = endPoint - startPoint;
            if (swipeVector.magnitude > adjustedDeadZone)
            {
                swipeExecuted = true;
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
    }

    public void InputRelease()
    {
        if (tapAllowed && !swipeExecuted && swipeVector.magnitude < adjustedDeadZone)
        {
            PlayerTapInputEvent?.Invoke();
            //Debug.Log("Detected tap input");
        }
        PlayerReleaseInputEvent?.Invoke();
        swipeExecuted = false;
        detectingInput = false;
    }
}
