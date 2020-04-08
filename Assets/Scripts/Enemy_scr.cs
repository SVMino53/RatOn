using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_scr : MonoBehaviour
{
    public enum State
    {
        IDLE,
        STROLLING,
        CHASING,
        LOSING
    }

    public State curState = State.IDLE;

    public float walkSpeed = 1.0f;
    public float runSpeed = 1.5f;
    public float rotationSpeed = 10.0f;

    public GameObject playerObj;

    public float viewDistance = 2.0f;
    [Range(0.0f, 180.0f)]
    public float viewAngleDegrees = 45.0f;

    public bool isTalking;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curState == State.IDLE)
        {

        }
        else if (curState == State.STROLLING)
        {

        }
        else if (curState == State.CHASING)
        {
            Vector3 toPlayerDirection = playerObj.transform.position - transform.position;
            toPlayerDirection.Normalize();

            float angle = Mathf.Asin(toPlayerDirection.normalized.x);
            angle *= -Mathf.Rad2Deg;
            if (toPlayerDirection.y < 0.0f)
            {
                angle += 180.0f;
                angle *= -1.0f;
            }
            Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, angle);

            transform.rotation = newRotation;

            transform.Translate(toPlayerDirection * runSpeed * Time.deltaTime, Space.World);
        }
        else if (curState == State.LOSING)
        {

        }

        float playerDistance = Vector3.Distance(transform.position, playerObj.transform.position);

        if (playerDistance <= viewDistance)
        {
            Vector3 enemyDirection = new Vector3(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

            Vector3 toPlayerDirection = playerObj.transform.position - transform.position;
            toPlayerDirection.Normalize();

            float playerAngle = Vector3.Angle(enemyDirection, toPlayerDirection);

            if (playerAngle <= viewAngleDegrees)
            {
                curState = State.CHASING;

                // FOR TESTING!!!
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            }
            else
            {
                // FOR TESTING!!!
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.0f);
            }
        }
        else
        {
            // FOR TESTING!!!
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.0f);
        }
    }
}
