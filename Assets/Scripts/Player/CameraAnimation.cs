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

    private void Start()
    {
        animator.Play("CameraLevelStart");
    }

    private void OnEnable()
    {
        RoomLevelManager.RoomLevelChanging += AnimateNextLevel;
    }

    private void OnDisable()
    {
        RoomLevelManager.RoomLevelChanging -= AnimateNextLevel;
    }

    private void AnimateNextLevel()
    {
        animator.Play("CameraNextLevel");
    }
}
