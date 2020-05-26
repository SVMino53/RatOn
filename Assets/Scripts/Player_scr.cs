using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_scr : MonoBehaviour
{
    ControllerInput controls;

    public KeyCode moveUpK = KeyCode.UpArrow;
    public KeyCode moveDownK = KeyCode.DownArrow;
    public KeyCode moveLeftK = KeyCode.LeftArrow;
    public KeyCode moveRightK = KeyCode.RightArrow;
    public KeyCode runK = KeyCode.LeftShift;
    public KeyCode standStillK = KeyCode.LeftControl;
    public KeyCode shrinkK = KeyCode.Z;
    public KeyCode recordK = KeyCode.X;

    Vector2 faceDirection;
    bool run;
    bool standStill;
    bool shrink;
    bool inputShrinkImpulse = false;
    bool record;


    public enum State
    {
        STANDING,
        WALKING,
        RUNNING
    }
    public State curState = State.STANDING;

    public bool isTiny = false;
    public bool isRecording = false;
    public bool isChased = false;

    public float walkSpeed = 1.0f;
    public float runSpeed = 1.5f;

    public float inputSensitivityOffset = 0.001f;

    public string enemyTag = "Enemy";

    public float recordingScoreIncrement = 1.0f;
    public float maxSecretValue = 100.0f;

    public Image secretBarFill;

    public Sprite normalSprite;
    public Sprite tinySprite;

    public SpriteRenderer recordAreaSpriteRenderer;

    [HideInInspector]
    public float secretValue = 0.0f;
    float totalSecretValue = 0.0f;
    public float minTotalSecretValue = 200.0f;

    public GameObject bossObj;
    public float maxDistanceFromBoss = 0.4f;

    public int moneyPerSecret = 100;

    public Text moneyText;

    public string exitTag = "LevelExit";

    // For testing
    public GameObject goodJob;


    SpriteRenderer spriteRenderer;

    CircleCollider2D[] colliders;


    private void Awake()
    {
        controls = new ControllerInput();

        controls.Gameplay.Move.performed += ctx => faceDirection = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => faceDirection = Vector2.zero;

        controls.Gameplay.Run.performed += ctx => run = true;
        controls.Gameplay.Run.canceled += ctx => run = false;

        controls.Gameplay.StandStill.performed += ctx => standStill = true;
        controls.Gameplay.StandStill.canceled += ctx => standStill = false;

        controls.Gameplay.Shrink.performed += ctx => shrink = true;
        controls.Gameplay.Shrink.canceled += ctx => shrink = false;

        controls.Gameplay.Record.performed += ctx => record = true;
        controls.Gameplay.Record.canceled += ctx => record = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        secretBarFill.fillAmount = 0.0f;

        spriteRenderer = GetComponent<SpriteRenderer>();

        colliders = GetComponents<CircleCollider2D>();

        colliders[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shrink && !inputShrinkImpulse || Input.GetKeyDown(shrinkK))
        {
            if (!isTiny)
            {
                isTiny = true;

                spriteRenderer.sprite = tinySprite;

                colliders[0].enabled = false;
                colliders[1].enabled = true;
            }
            else
            {
                isTiny = false;

                spriteRenderer.sprite = normalSprite;

                colliders[1].enabled = false;
                colliders[0].enabled = true;
            }
            inputShrinkImpulse = true;
        }
        else if (!shrink && inputShrinkImpulse || Input.GetKeyDown(shrinkK))
        {
            inputShrinkImpulse = false;
        }

        if (faceDirection.x < -inputSensitivityOffset ||
            faceDirection.x > inputSensitivityOffset ||
            faceDirection.y < -inputSensitivityOffset ||
            faceDirection.y > inputSensitivityOffset)
        {
            float angle = Mathf.Asin(faceDirection.normalized.x);
            angle *= -Mathf.Rad2Deg;
            if (faceDirection.y < 0.0f)
            {
                angle += 180.0f;
                angle *= -1.0f;
            }
            Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, angle);

            transform.rotation = newRotation;

            if (!standStill)
            {
                if (run)
                {
                    transform.Translate(faceDirection * runSpeed * Time.deltaTime, Space.World);
                    curState = State.RUNNING;
                }
                else
                {
                    transform.Translate(faceDirection * walkSpeed * Time.deltaTime, Space.World);
                    curState = State.WALKING;
                }
            }
            else
            {
                curState = State.STANDING;
            }
        }
        else
        {
            Vector2 moveDirection = Vector2.zero;

            if (Input.GetKey(moveUpK))
            {
                moveDirection.y = 1.0f;
            }
            if (Input.GetKey(moveDownK))
            {
                moveDirection.y = -1.0f;
            }
            if (Input.GetKey(moveLeftK))
            {
                moveDirection.x = -1.0f;
            }
            if (Input.GetKey(moveRightK))
            {
                moveDirection.x = 1.0f;
            }

            if (moveDirection != Vector2.zero)
            {
                moveDirection.Normalize();

                float angle = Mathf.Asin(moveDirection.normalized.x);
                angle *= -Mathf.Rad2Deg;
                if (moveDirection.y < 0.0f)
                {
                    angle += 180.0f;
                    angle *= -1.0f;
                }
                Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, angle);

                transform.rotation = newRotation;

                if (!Input.GetKey(standStillK))
                {
                    if (Input.GetKey(runK))
                    {
                        transform.Translate(moveDirection * runSpeed * Time.deltaTime, Space.World);
                        curState = State.RUNNING;
                    }
                    else
                    {
                        transform.Translate(moveDirection * walkSpeed * Time.deltaTime, Space.World);
                        curState = State.WALKING;
                    }
                }
                else
                {
                    curState = State.STANDING;
                }
            }
            else
            {
                curState = State.STANDING;
            }
        }

        if (record || Input.GetKey(recordK))
        {
            if (Vector3.Distance(transform.position, bossObj.transform.position) <= maxDistanceFromBoss)
            {
                if (secretValue > 0.0f)
                {
                    GameStatic_scr.money += (uint)(moneyPerSecret * secretValue);
                    moneyText.text = "$" + GameStatic_scr.money.ToString();
                    totalSecretValue += secretValue;

                    secretValue = 0.0f;
                    secretBarFill.fillAmount = 0.0f;
                }
            }
            else
            {
                isRecording = true;
            }
        }
        else
        {
            isRecording = false;
        }

        if (isRecording && secretValue < maxSecretValue && !isChased && !isTiny)
        {
            recordAreaSpriteRenderer.enabled = true;
        }
        else
        {
            recordAreaSpriteRenderer.enabled = false;
        }
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            if (isRecording && collision.GetComponent<Enemy_scr>().isTalking && secretValue < maxSecretValue && !isChased && !isTiny)
            {
                secretValue += Time.deltaTime * recordingScoreIncrement;
                secretBarFill.fillAmount = secretValue / maxSecretValue;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(exitTag))
        {
            if (totalSecretValue >= minTotalSecretValue)
            {
                GameStatic_scr.level++;

                goodJob.SetActive(true);
            }
        }
    }
}
