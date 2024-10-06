using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    [SerializeField]private Rigidbody2D rb;
    private PlayerInput playerInput;
    [SerializeField]Animator animator;
    [SerializeField]private Slime playerSlime;

    void Awake(){
        playerInput = new PlayerInput();
    }
    void OnEnable(){
        playerInput.Enable();
        playerInput.Player.Eat.performed += ctx => playerSlime.Eat();
        playerInput.Player.Eat.canceled += ctx => playerSlime.EndEat();
        playerInput.UI.Pause.performed += ctx => GameManager.Instance.CallPauseMenu();
    }

    void OnDisable(){
        playerInput.Disable();
        playerInput.Player.Eat.performed -= ctx => playerSlime.Eat();
        playerInput.Player.Eat.canceled -= ctx => playerSlime.EndEat();
        playerInput.UI.Pause.performed -= ctx => GameManager.Instance.CallPauseMenu();
    }

    void FixedUpdate()
    {
        Vector2 movement = playerInput.Player.Move.ReadValue<Vector2>();
        // Call movement function
        Move(movement);

        if(movement != Vector2.zero){
            playerSlime.PlayMoveAnimation();
        }
    }

    private void Move(Vector2 movement){
        //move with transform
        transform.position += new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.deltaTime;
    }
}
