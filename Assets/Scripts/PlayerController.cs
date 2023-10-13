using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    private Vector2 inputValue;
    [SerializeField] private ParticleSystem _particleSystem;

    private void FixedUpdate()
    {
        MoveCharacter(inputValue);
        if (inputValue == Vector2.zero)
        {
            _particleSystem.Stop();
            isMoving = false;
        } else
        {
            _particleSystem.Play();
            isMoving = true;
        }

        HandleUpdate();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputValue = context.ReadValue<Vector2>();
    }
}
