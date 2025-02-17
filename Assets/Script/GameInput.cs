using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
   
     private GameInputAcion action;

    private void Start()
    {
        action = new GameInputAcion();
        action.Player.Enable();

        action.Player.Interact.performed += Interacr_Perfomed;
    }

    public void Interacr_Perfomed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }

    public Vector3 GetMovementDirectionNormalized()
    {
        Vector2 inputVector2 = action.Player.Move.ReadValue<Vector2>();

        Vector3 direction = new Vector3(inputVector2.x, 0, inputVector2.y);

        direction = direction.normalized; 

        return direction;
    }

}
