using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movement Settings")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    private float horizontalMovement;
    public bool isFacingRight = true;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    private int maxJump = 1;
    private int jumpRemaning;

    [Header("GroundCheck Settings")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f,0.05f);
    [SerializeField] private LayerMask grondLayer;

    [Header("Gravity Settings")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed,rb.velocity.y);
        GroundCheck();
        Gravity();
        Flip();
    }
    
    //Inizio di Input
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if(jumpRemaning > 0)
        {
            if(context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpRemaning--;
            }
        }
    }

    //Funzioni di Controllo
    private void Gravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }
    private void GroundCheck()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer))
        {
            jumpRemaning = maxJump;
        }
    }
    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position,groundCheckSize);
    }
}
