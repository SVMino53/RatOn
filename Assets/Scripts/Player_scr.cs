using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_scr : MonoBehaviour
{
    ControllerInput controls;

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


    public float secretValue = 0.0f;


    //Rigidbody2D rb;

    SpriteRenderer spriteRenderer;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (shrink && !inputShrinkImpulse)
        {
            if (!isTiny)
            {
                isTiny = true;

                spriteRenderer.sprite = tinySprite;
            }
            else
            {
                isTiny = false;

                spriteRenderer.sprite = normalSprite;
            }
            inputShrinkImpulse = true;
        }
        else if (!shrink && inputShrinkImpulse)
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

            isRecording = record;
        }
        else
        {
            curState = State.STANDING;
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
            if (record && collision.GetComponent<Enemy_scr>().isTalking && secretValue < maxSecretValue && !isChased && !isTiny)
            {
                secretValue += Time.deltaTime * recordingScoreIncrement;
                secretBarFill.fillAmount = secretValue / maxSecretValue;
            }
        }
    }
}
