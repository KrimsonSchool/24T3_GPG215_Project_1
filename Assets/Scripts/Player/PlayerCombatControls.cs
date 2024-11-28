using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatInputs { Tap, Release, SwipeUp, SwipeDown, SwipeLeft, SwipeRight }

public class PlayerCombatControls : Singleton<PlayerCombatControls>
{
    [SerializeField] private float tapDuration = 0.2f;
    [HideInInspector] public static readonly float defaultDeadZone = 0.05f;
    private float deadZone = 0.05f;
    private float adjustedDeadZone;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 swipeVector;
    private bool detectingInput = false;
    private bool tapAllowed = false;
    private bool swipeExecuted = false;

    public float DeadZone {  get { return deadZone; } set { deadZone = value; } }

    public static event Action<CombatInputs> PlayerControlInput;

    protected override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey("DeadZone"))
        {
            deadZone = PlayerPrefs.GetFloat("DeadZone");
        }
        else
        {
            deadZone = defaultDeadZone;
        }
    }

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
                if (swipeVector.normalized.x > 0.7f) // 0.7f is just quick hillbilly shorthand to indicate passing the 45 degree normalized mark
                {
                    PlayerControlInput?.Invoke(CombatInputs.SwipeRight);
                    //Debug.Log("Detected swipe right input");
                }
                else if (swipeVector.normalized.x < -0.7f)
                {
                    PlayerControlInput?.Invoke(CombatInputs.SwipeLeft);
                    //Debug.Log("Detected swipe left input");
                }
                else if (swipeVector.normalized.y < -0.7f)
                {
                    PlayerControlInput?.Invoke(CombatInputs.SwipeDown);
                    //Debug.Log("Detected swipe down input");
                }
                else if (swipeVector.normalized.y > 0.7f)
                {
                    PlayerControlInput?.Invoke(CombatInputs.SwipeUp);
                    //Debug.Log("Detected swipe up input");
                }
            }
        }
    }

    public void InputRelease()
    {
        if (tapAllowed && !swipeExecuted && swipeVector.magnitude < adjustedDeadZone)
        {
            PlayerControlInput?.Invoke(CombatInputs.Tap);
            //Debug.Log("Detected tap input");
        }
        PlayerControlInput?.Invoke(CombatInputs.Release);
        swipeExecuted = false;
        detectingInput = false;
    }
}
