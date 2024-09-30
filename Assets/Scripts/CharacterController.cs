using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    public abstract void HandleInput(CharacterController character);
    public abstract void Update(CharacterController character);
}

public class IdleState : CharacterState
{
    public override void HandleInput(CharacterController character)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.SetState(new JumpingState());
        }
    }

    public override void Update(CharacterController character)
    {
        // Idle behavior
    }
}

public class JumpingState : CharacterState
{
    public override void HandleInput(CharacterController character)
    {
        // Handle jumping input
    }

    public override void Update(CharacterController character)
    {
        // Jumping behavior
        if (character.IsGrounded)
        {
            character.SetState(new IdleState());
        }
    }
}

public class CharacterController : MonoBehaviour
{
    private CharacterState currentState;
    public bool IsGrounded;

    void Start()
    {
        SetState(new IdleState());
    }

    public void SetState(CharacterState state)
    {
        currentState = state;
    }

    void Update()
    {
        currentState.HandleInput(this);
        currentState.Update(this);
    }
}

