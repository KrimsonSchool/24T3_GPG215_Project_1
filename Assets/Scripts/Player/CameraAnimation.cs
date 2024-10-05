using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        RoomLevelManager.RoomLevelChanging += AnimateCamera;
    }

    private void OnDisable()
    {
        RoomLevelManager.RoomLevelChanging -= AnimateCamera;
    }

    private void AnimateCamera()
    {
        animator.enabled = true;
    }
}
