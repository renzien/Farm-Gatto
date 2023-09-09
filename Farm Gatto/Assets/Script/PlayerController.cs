using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D contactFilter;
    private bool isMoving;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Vector2 input;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                int count = rb.Cast(
                    input,
                    contactFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);
                
                if(count == 0)
                {
                    rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
                }

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                
                StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
}
