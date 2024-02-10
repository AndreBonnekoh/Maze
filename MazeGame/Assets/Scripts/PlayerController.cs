using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    private Coroutine moveCoroutine;

    public LayerMask solidObjectsLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input != Vector2.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 4;
            }
            else
            {
                moveSpeed = 2;
            }

            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            var targetPosition = transform.position + new Vector3(input.x, input.y, 0f);

            if (IsWalkable(targetPosition))
            {
                moveCoroutine = StartCoroutine(Move(targetPosition));
            }
        }
        else if (isMoving)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            isMoving = false;
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

    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
