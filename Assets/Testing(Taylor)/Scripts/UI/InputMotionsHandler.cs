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

    public static event Action PlayerAttackInputEvent;
    public static event Action PlayerDodgeRightInputEvent;
    public static event Action PlayerDodgeLeftInputEvent;
    public static event Action PlayerBlockInputEvent;

    public void RecordDragMotion()
    {
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
            PlayerAttackInputEvent?.Invoke();
            //Debug.Log("Detected attack input");
        }
        else if (swipeVector.magnitude > tapDeadZone)
        {
            if (swipeVector.normalized.x > 0.7f) // 0.7f is just quick hillbilly shorthand to indicate passing the 45% normalized mark
            {
                PlayerDodgeRightInputEvent?.Invoke();
                //Debug.Log("Detected dodge right input");
            }
            else if (swipeVector.normalized.x < -0.7f)
            {
                PlayerDodgeLeftInputEvent?.Invoke();
                //Debug.Log("Detected dodge left input");
            }
            else if (swipeVector.normalized.y < -0.7f)
            {
                PlayerBlockInputEvent?.Invoke();
                //Debug.Log("Detected block input");
            }
            else if (swipeVector.normalized.y > 0.7f)
            {
                PlayerAttackInputEvent?.Invoke();
                //Debug.Log("Detected attack input"); //idk this might feel intuitive to some people?
            }
        }
    }

    private IEnumerator TapAllowance()
    {
        yield return new WaitForSeconds(tapDuration);
        tapAllowed = false;
    }
}
