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

    public string obstacleTag = "Obstacle";

    public GameObject lineSightObj;

    Rigidbody2D rb;

    EdgeCollider2D lineCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lineCollider = lineSightObj.GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 selfPos = new Vector2(transform.position.x, transform.position.y);
        //Vector2 playerPos = new Vector2(playerObj.transform.position.x, playerObj.transform.position.y);

        if (curState == State.IDLE)
        {

        }
        else if (curState == State.STROLLING)
        {

        }
        else if (curState == State.CHASING)
        {
            Vector2 toPlayerDirection = playerObj.transform.position - transform.position;
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
            Vector2 enemyDirection = new Vector2(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

            Vector2 toPlayer = playerObj.transform.position - transform.position;

            float playerAngle = Vector2.SignedAngle(enemyDirection, toPlayer.normalized);

            if (playerAngle <= viewAngleDegrees && playerAngle >= -viewAngleDegrees)
            {
                bool isBlocked = false;

                lineSightObj.transform.localScale = new Vector3(1.0f, toPlayer.magnitude, 1.0f);
                
                Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + playerAngle);

                lineSightObj.transform.rotation = newRotation;

                ContactFilter2D cf = new ContactFilter2D();
                List<Collider2D> overlappingColliders = new List<Collider2D>();
                int n = lineCollider.OverlapCollider(cf.NoFilter(), overlappingColliders);

                for (int i = 0; i < n; i++)
                {
                    if (overlappingColliders[i].CompareTag(obstacleTag))
                    {
                        isBlocked = true;
                        break;
                    }
                }

                if (!isBlocked && n != 0)
                {
                    curState = State.CHASING;

                    // FOR TESTING!!!
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
                }
                else
                {
                    // FOR TESTING!!!
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
                }
            }
            else
            {
                // FOR TESTING!!!
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            // FOR TESTING!!!
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    }
}
