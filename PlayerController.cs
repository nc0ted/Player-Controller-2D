using System;
using UnityEngine;

namespace Nikton
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Horizontal Movement")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        
        [Header("Vertical Movement")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float grounderDistance;
        [SerializeField] private float groundingForce;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float fallAcceleration;
        [SerializeField] private float jumpPower;
        
        private CapsuleCollider2D _col;
        private Rigidbody2D _rb;
        private Vector3 _velocity;
        private bool _grounded;
        
        private void Awake()
        {
            _col = GetComponent<CapsuleCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            CheckForCollisions();   
            HandleGravity();
            if (_grounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            HorizontalMovement();
        }

        private void FixedUpdate()
        {
            _rb.velocity = _velocity;
        }

        private void HorizontalMovement()
        {
            var xInput = Input.GetAxisRaw("Horizontal");
            if (xInput==0)
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, 
                    deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, xInput * maxSpeed,
                    acceleration * Time.fixedDeltaTime);
            } 
            
            if(xInput==0) return;
            transform.localScale = new Vector3(xInput,1,1);
        }

        private void Jump()
        {
            _velocity.y = jumpPower;
        }
        private void CheckForCollisions()
        { 
            _grounded=Physics2D.CapsuleCast(_col.bounds.center, _col.size,
                _col.direction, 0, Vector2.down, grounderDistance, ~playerLayer);
        }
        private void HandleGravity()
        {
            if (_grounded && _velocity.y <= 0f)
            {
                _velocity.y = groundingForce;
            }
            else
            {
                var inAirGravity = fallAcceleration;
                _velocity.y = Mathf.MoveTowards(_velocity.y, -maxFallSpeed, 
                    inAirGravity * Time.fixedDeltaTime);
            }
        }
    }
}