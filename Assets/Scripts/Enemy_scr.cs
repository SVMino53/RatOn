﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_scr : MonoBehaviour
{
    public bool testing = false;

    public enum State
    {
        IDLE,
        STROLLING,
        CHASING,
        LOSING
    }

    public State curState = State.IDLE;

    public List<Vector2> strollPath;

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

    public float nextPointDistence = 0.2f;
    int pointIndex = 0;
    int strollPointIndex = 0;

    public Collider2D obstacleMapCollider;
    public Collider2D windowMapCollider;

    public EdgeCollider2D generalLineCollider;
    public CircleCollider2D generalPointCollider;

    public float losingTime = 5.0f;
    float curLosingTime = 0.0f;

    public float minTalkingTime = 8.0f;
    public float maxTalkingTime = 20.0f;
    float talkingTime = 0.0f;
    [Range(0.0f, 1.0f)]
    public float chanceOfTalking = 0.2f;
    public float chanceOfTalkingInterval = 0.5f;
    float chanceOfTalkingIntervalTime = 0.0f;
    public float minTimeToWannaTalk = 30.0f;
    float timeToWannaTalk = 0.0f;

    public Sprite mainSprite;
    public Sprite talkSprite;

    EdgeCollider2D lineCollider;

    List<Vector2> path;

    ContactFilter2D cf = new ContactFilter2D();

    bool getPathOnce = true;

    // TEMPORARY
    bool isSecondPath = false;

    // For testing
    public LineRenderer pathLine;
    LineRenderer prevPathLine = null;
    public LineRenderer longPathLine;
    LineRenderer prevLongPathLine = null;

    bool GetLineIsCollidingWith(Vector2 start, Vector2 end, Collider2D collider, float pointDistance)
    {
        Vector2 direction = (end - start).normalized;
        Vector2 curPoint = start;

        int limit = 0;
        while ((end - curPoint).magnitude > pointDistance)
        {
            if (limit == 1000)
            {
                break;
            }
            limit++;

            if (collider.OverlapPoint(curPoint))
            {
                return true;
            }

            curPoint += direction * pointDistance;
        }

        return false;
    }

    List<Vector2> GetPath(Vector2 target)
    {
        Vector2 curTarget = target;

        List<Vector2> branchPoints = new List<Vector2>();

        if (prevPathLine != null)
        {
            Destroy(prevPathLine.gameObject);
        }

        if (prevLongPathLine != null)
        {
            Destroy(prevLongPathLine.gameObject);
        }

        Vector2 startPoint = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 curPoint = startPoint;

        List <Vector2> path = new List<Vector2>();

        // For testing ->
        List<Vector3> pointsList = new List<Vector3>
        {
            curPoint
        };

        List<Vector3> longPointsList = new List<Vector3>
        {
            curPoint
        };
        // <-

        List<Vector2> reachedPoints = new List<Vector2>();
        List<Vector2> allPathPoints = new List<Vector2>();
        int limit0 = 0;
        while (Vector2.Distance(curPoint, target) > Mathf.Sqrt(2.0f))
        {
            if (limit0 >= 1000)
            {
                break;
            }

            limit0++;

            Vector2 nextPoint = curPoint;
            List<Vector2> potentialPoints = new List<Vector2>();

            reachedPoints.Add(curPoint);

            nextPoint.y += 1.0f;
            if (!obstacleMapCollider.OverlapPoint(nextPoint) && !windowMapCollider.OverlapPoint(nextPoint) && !reachedPoints.Contains(nextPoint))
            {
                potentialPoints.Add(nextPoint);
            }
            nextPoint.y -= 1.0f;
            nextPoint.x += 1.0f;
            if (!obstacleMapCollider.OverlapPoint(nextPoint) && !windowMapCollider.OverlapPoint(nextPoint) && !reachedPoints.Contains(nextPoint))
            {
                potentialPoints.Add(nextPoint);
            }
            nextPoint.y -= 1.0f;
            nextPoint.x -= 1.0f;
            if (!obstacleMapCollider.OverlapPoint(nextPoint) && !windowMapCollider.OverlapPoint(nextPoint) && !reachedPoints.Contains(nextPoint))
            {
                potentialPoints.Add(nextPoint);
            }
            nextPoint.y += 1.0f;
            nextPoint.x -= 1.0f;
            if (!obstacleMapCollider.OverlapPoint(nextPoint) && !windowMapCollider.OverlapPoint(nextPoint) && !reachedPoints.Contains(nextPoint))
            {
                potentialPoints.Add(nextPoint);
            }

            if (potentialPoints.Count > 0)
            {
                curPoint = potentialPoints[0];
            }
            
            for (int i = 1; i < potentialPoints.Count; i++)
            {
                if (Vector2.Distance(potentialPoints[i], target) < Vector2.Distance(curPoint, target))
                {
                    curPoint = potentialPoints[i];
                }
            }

            if (reachedPoints.Contains(curPoint))
            {
                if (allPathPoints.Count > 0)
                {
                    curPoint = allPathPoints[allPathPoints.Count - 1];
                    allPathPoints.RemoveAt(allPathPoints.Count - 1);
                }
            }
            else
            {
                allPathPoints.Add(curPoint);
            }
        }

        Vector2 curReachablePoint = transform.position;

        if (allPathPoints.Count > 0)
        {
            int limit1 = 0;
            while (curReachablePoint != allPathPoints[allPathPoints.Count - 1])
            {
                if (limit1 >= 1000)
                {
                    break;
                }
                limit1++;

                for (int i = allPathPoints.Count - 1; i >= 0; i--)
                {
                    if (!GetLineIsCollidingWith(curReachablePoint, allPathPoints[i], obstacleMapCollider, 0.1f) &&
                        !GetLineIsCollidingWith(curReachablePoint, allPathPoints[i], windowMapCollider, 0.1f))
                    {
                        curReachablePoint = allPathPoints[i];
                        path.Add(curReachablePoint);
                        break;
                    }
                }
            }
        }
        path.Add(target);

        if (testing)
        {
            for (int i = 0; i < path.Count; i++)
            {
                pointsList.Add(path[i]);
            }

            for (int i = 0; i < allPathPoints.Count; i++)
            {
                longPointsList.Add(allPathPoints[i]);
            }

            pathLine.positionCount = pointsList.Count;
            pathLine.SetPositions(pointsList.ToArray());

            longPathLine.positionCount = longPointsList.Count;
            longPathLine.SetPositions(longPointsList.ToArray());

            prevPathLine = Instantiate(pathLine);

            prevLongPathLine = Instantiate(longPathLine);
        }

        return path;
    }

    void DoIWannaTalk()
    {
        if (timeToWannaTalk >= minTimeToWannaTalk)
        {
            if (chanceOfTalkingIntervalTime >= chanceOfTalkingInterval)
            {
                if (Random.value <= chanceOfTalking)
                {
                    isTalking = true;
                    GetComponent<SpriteRenderer>().sprite = talkSprite;
                    timeToWannaTalk = 0.0f;
                }

                chanceOfTalkingIntervalTime = 0.0f;
            }
            else
            {
                chanceOfTalkingIntervalTime += Time.deltaTime;
            }
        }
        else
        {
            timeToWannaTalk += Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineCollider = lineSightObj.GetComponent<EdgeCollider2D>();

        if (strollPath.Count == 0)
        {
            curState = State.IDLE;
        }
        else if (strollPath.Count == 1)
        {
            curState = State.IDLE;
            transform.position = strollPath[0];
        }
        else
        {
            curState = State.STROLLING;
            transform.position = strollPath[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position2d = new Vector2(transform.position.x, transform.position.y);

        if (isTalking)
        {
            curState = State.IDLE;
        }

        if (curState == State.IDLE)
        {
            if (isTalking)
            {
                if (talkingTime >= maxTalkingTime)
                {
                    isTalking = false;
                    talkingTime = 0.0f;
                    GetComponent<SpriteRenderer>().sprite = mainSprite;

                    if (strollPath.Count > 1)
                    {
                        curState = State.STROLLING;
                    }
                }
                else if (talkingTime >= minTalkingTime && Random.Range(minTalkingTime, maxTalkingTime) < talkingTime)
                {
                    
                    isTalking = false;
                    talkingTime = 0.0f;
                    GetComponent<SpriteRenderer>().sprite = mainSprite;

                    if (strollPath.Count > 1)
                    {
                        curState = State.STROLLING;
                    }
                }
                else
                {
                    talkingTime += Time.deltaTime;
                }
            }
            else
            {
                DoIWannaTalk();
            }
        }
        else if (curState == State.STROLLING)
        {
            float enemyToStrollPathPointDistance = (strollPath[strollPointIndex] - position2d).magnitude;

            if (enemyToStrollPathPointDistance <= nextPointDistence)
            {
                if (strollPointIndex < strollPath.Count - 1)
                {
                    strollPointIndex++;
                }
                else
                {
                    strollPointIndex = 0;
                }
            }

            Vector2 goToPoint = strollPath[strollPointIndex];

            Vector2 direction = -transform.position;
            direction += goToPoint;

            transform.Translate(direction.normalized * walkSpeed * Time.deltaTime, Space.World);

            float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

            DoIWannaTalk();
        }
        else if (curState == State.CHASING)
        {
            if (path.Count > 0)
            {
                float enemyToPathPointDistance = (path[pointIndex] - position2d).magnitude;

                if (enemyToPathPointDistance <= nextPointDistence)
                {
                    if (pointIndex < path.Count - 1)
                    {
                        pointIndex++;
                    }
                    else
                    {
                        if (!isSecondPath)
                        {
                            path = GetPath(playerObj.transform.position);

                            pointIndex = 0;

                            isSecondPath = true;
                        }
                        else
                        {
                            curState = State.LOSING;

                            curLosingTime = 0.0f;

                            if (strollPath.Count == 1)
                            {
                                path = GetPath(strollPath[0]);

                                pointIndex = 0;
                            }
                        }
                    }
                }

                Vector2 goToPoint = path[pointIndex];

                Vector2 direction = -transform.position;
                direction += goToPoint;

                transform.Translate(direction.normalized * runSpeed * Time.deltaTime, Space.World);

                float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

                transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }

            playerObj.GetComponent<Player_scr>().isChased = true;
        }
        else if (curState == State.LOSING)
        {
            if (curLosingTime < losingTime)
            {
                curLosingTime += Time.deltaTime;
            }
            else
            {
                if (strollPath.Count == 0)
                {
                    curState = State.IDLE;
                }
                else if (strollPath.Count == 1)
                {
                    float enemyToPathPointDistance = (path[pointIndex] - position2d).magnitude;

                    if (enemyToPathPointDistance <= nextPointDistence)
                    {
                        if (pointIndex < path.Count - 1)
                        {
                            pointIndex++;
                        }
                        else
                        {
                            curState = State.IDLE;
                        }
                    }

                    Vector2 goToPoint = path[pointIndex];

                    Vector2 direction = -transform.position;
                    direction += goToPoint;

                    transform.Translate(direction.normalized * walkSpeed * Time.deltaTime, Space.World);

                    float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                }
                else
                {
                    Vector2 closestPoint = strollPath[0];

                    if (getPathOnce)
                    {
                        for (int i = 1; i < strollPath.Count; i++)
                        {
                            if (Vector2.Distance(position2d, strollPath[i]) < Vector2.Distance(position2d, closestPoint))
                            {
                                closestPoint = strollPath[i];

                                strollPointIndex = i;
                            }
                        }

                        path = GetPath(closestPoint);

                        pointIndex = 0;

                        getPathOnce = false;
                    }
                    else
                    {
                        float enemyToPathPointDistance = (path[pointIndex] - position2d).magnitude;

                        if (enemyToPathPointDistance <= nextPointDistence)
                        {
                            if (pointIndex < path.Count - 1)
                            {
                                pointIndex++;
                            }
                            else
                            {
                                curState = State.STROLLING;
                            }
                        }

                        Vector2 goToPoint = path[pointIndex];

                        Vector2 direction = -transform.position;
                        direction += goToPoint;

                        transform.Translate(direction.normalized * walkSpeed * Time.deltaTime, Space.World);

                        float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

                        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                    }
                }

                playerObj.GetComponent<Player_scr>().isChased = false;
            }
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

                    path = GetPath(playerObj.transform.position);

                    pointIndex = 0;

                    isSecondPath = false;

                    isTalking = false;

                    GetComponent<SpriteRenderer>().sprite = mainSprite;

                    //curGetPathDelay = getPathDelay;

                    // FOR TESTING!!!
                    if (testing)
                    {
                        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
                    }
                }
                else
                {
                    // FOR TESTING!!!
                    if (testing)
                    {
                        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
                    }
                }
            }
            else
            {
                // FOR TESTING!!!
                if (testing)
                {
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }
        else
        {
            // FOR TESTING!!!
            if (testing)
            {
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
