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
    public string windowTag = "Window";

    public GameObject lineSightObj;

    public float getPathDelay = 0.5f;
    float curGetPathDelay = 0.0f;

    public float nextPointDistence = 0.2f;
    int pointIndex = 0;

    Rigidbody2D rb;

    EdgeCollider2D lineCollider;

    List<Vector2> path;

    // For testing
    public LineRenderer pathLine;
    LineRenderer prevPathLine = null;

    List<Vector2> GetPath(Vector2 target)
    {
        if (prevPathLine != null)
        {
            Destroy(prevPathLine.gameObject);
        }
        Vector2 curPoint = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        List<Vector2> path = new List<Vector2>();
        Vector2 newPoint;

        // For testing ->
        List<Vector3> pointsList = new List<Vector3>();
        pointsList.Add(curPoint);
        // <-

        while(Vector2.Distance(curPoint, target) > Mathf.Sqrt(2.0f) && Time.deltaTime < 10.0f)
        {

            Vector2 pointToPlayer = playerObj.transform.position;
            pointToPlayer -= curPoint;

            if (pointToPlayer.x <= -1.0f)
            {
                curPoint.x -= 1.0f;
            }
            else if (pointToPlayer.x >= 1.0f)
            {
                curPoint.x += 1.0f;
            }
            if (pointToPlayer.y <= -1.0f)
            {
                curPoint.y -= 1.0f;
            }
            else if (pointToPlayer.y >= 1.0f)
            {
                curPoint.y += 1.0f;
            }

            path.Add(curPoint);
            pointsList.Add(curPoint);
        }

        path.Add(playerObj.transform.position);
        pointsList.Add(playerObj.transform.position);

        pathLine.positionCount = pointsList.Count;
        pathLine.SetPositions(pointsList.ToArray());

        prevPathLine = Instantiate(pathLine);

        return path;
    }

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
            if (curGetPathDelay >= getPathDelay)
            {
                path = GetPath(playerObj.transform.position);

                pointIndex = 0;

                curGetPathDelay = 0.0f;
            }
            else
            {
                curGetPathDelay += Time.deltaTime;
            }

            if (Vector2.Distance(transform.position, path[pointIndex]) <= nextPointDistence && pointIndex < path.Count - 1)
            {
                pointIndex++;
            }

            Vector2 goToPoint = path[pointIndex];

            Vector2 direction = -transform.position;
            direction += goToPoint;

            transform.Translate(direction.normalized * runSpeed * Time.deltaTime, Space.World);

            float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

            //Vector2 toPlayerDirection = playerObj.transform.position - transform.position;
            //toPlayerDirection.Normalize();

            //float angle = Mathf.Asin(toPlayerDirection.normalized.x);
            //angle *= -Mathf.Rad2Deg;
            //if (toPlayerDirection.y < 0.0f)
            //{
            //    angle += 180.0f;
            //    angle *= -1.0f;
            //}
            //Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, angle);

            //transform.rotation = newRotation;

            //transform.Translate(toPlayerDirection * runSpeed * Time.deltaTime, Space.World);
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

                if (!isBlocked && n > 1)
                {
                    curState = State.CHASING;

                    curGetPathDelay = getPathDelay;

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
