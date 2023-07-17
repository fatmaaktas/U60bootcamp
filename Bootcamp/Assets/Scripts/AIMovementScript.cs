using System.Collections;
using UnityEngine;

public class AIMovementScript : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        animator.SetBool("IsIdle", true);
    }

    private void Update()
    {
        if (!isWandering)
        {
            StartCoroutine(Wander());
        }

        if (isRotatingRight)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }

        if (isRotatingLeft)
        {
            transform.Rotate(-transform.up * Time.deltaTime * rotationSpeed);
        }

        if (isWalking)
        {
            rb.AddForce(transform.forward * movementSpeed);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private IEnumerator Wander()
    {
        float rotationTime = Random.Range(1.0f, 3.0f);
        float rotateWait = Random.Range(1.0f, 3.0f);
        int rotateDirection = Random.Range(1, 3);
        float walkWait = Random.Range(1.0f, 3.0f);
        float walkTime = Random.Range(1.0f, 15.0f);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);

        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotateWait);

        if (rotateDirection == 1)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }
        else if (rotateDirection == 2)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }

        isWandering = false;
    }
}

