using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_scr : MonoBehaviour
{
    ControllerInput controls;

    Vector2 faceDirection;
    bool run;
    bool standStill;


    public float walkSpeed = 1.0f;
    public float runSpeed = 1.5f;

    public float inputSensitivityOffset = 0.001f;

    Rigidbody2D rb;


    private void Awake()
    {
        controls = new ControllerInput();

        controls.Gameplay.Move.performed += ctx => faceDirection = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => faceDirection = Vector2.zero;

        controls.Gameplay.Run.performed += ctx => run = true;
        controls.Gameplay.Run.canceled += ctx => run = false;

        controls.Gameplay.StandStill.performed += ctx => standStill = true;
        controls.Gameplay.StandStill.canceled += ctx => standStill = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
                }
                else
                {
                    transform.Translate(faceDirection * walkSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        
        //Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        //direction.Normalize();

        //if (direction.x < -inputSensitivityOffset ||
        //    direction.x > inputSensitivityOffset ||
        //    direction.y < -inputSensitivityOffset ||
        //    direction.y > inputSensitivityOffset)
        //{
        //    float angle = Mathf.Asin(direction.x);
        //    angle *= -Mathf.Rad2Deg;
        //    if (Input.GetAxis("Vertical") < 0.0f)
        //    {
        //        angle += 180.0f;
        //        angle *= -1.0f;
        //    }
        //    Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, angle);

        //    transform.rotation = newRotation;

        //    if (Input.GetAxis("Stand Still") <= 0.0f)
        //    {
        //        if (Input.GetAxis("Run") > 0.0f)
        //        {
        //            transform.Translate(direction * runSpeed * Time.deltaTime, Space.World);
        //        }
        //        else
        //        {
        //            transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
        //        }
        //    }
        //}
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
