using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPersonaje : MonoBehaviour
{
    //VARIABLES
    public float speed;

    private float moveX;
    private float moveY;
    
    //VARIABLES TIPO ESTRUCTURAS
    Rigidbody2D rb2D;     
    Animator animator;
    public InputSystem_Actions acciones;
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        acciones = new InputSystem_Actions();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void OnEnable()
    {
        acciones.Player.Enable();

        acciones.Player.Move.performed += movimientoX;
        acciones.Player.Move.canceled += movimientoX;

        acciones.Player.Move.performed += movimientoY;
        acciones.Player.Move.canceled += movimientoY;
    }
    
    void OnDisable()
    {
        acciones.Player.Move.performed -= movimientoX;
        acciones.Player.Move.canceled -= movimientoX;

        acciones.Player.Move.performed -= movimientoY;
        acciones.Player.Move.canceled -= movimientoY;
        
        acciones.Player.Disable();
    }

    void movimientoX(InputAction.CallbackContext ctx)
    {
        moveX = ctx.ReadValue<Vector2>().x;
    }
    
    void movimientoY(InputAction.CallbackContext ctx)
    {
        moveY = ctx.ReadValue<Vector2>().y;
    }

    void flip()
    {
        if (rb2D.linearVelocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb2D.linearVelocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
    
    private void FixedUpdate()
    {
        rb2D.linearVelocityX = moveX * speed;
        animator.SetFloat("Vx", Mathf.Abs(rb2D.linearVelocityX));
        flip();
        rb2D.linearVelocityY = moveY * speed;
        animator.SetFloat("Vy", rb2D.linearVelocityY);
        flip();
    }
    
}
