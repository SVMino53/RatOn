using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_scr : MonoBehaviour
{
    public float walkSpeed = 1.0f;
    public float runSpeed = 1.5f;

    public float inputSensitivityOffset = 0.001f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        direction.Normalize();

        if (direction.x < -inputSensitivityOffset ||
            direction.x > inputSensitivityOffset ||
            direction.y < -inputSensitivityOffset ||
            direction.y > inputSensitivityOffset)
        {
            float angle = Mathf.Asin(direction.x);
            angle *= -Mathf.Rad2Deg;
            if (Input.GetAxis("Vertical") < 0.0f)
            {
                angle += 180.0f;
                angle *= -1.0f;
            }
            Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, angle);

            transform.rotation = newRotation;

            if (Input.GetAxis("Stand Still") <= 0.0f)
            {
                if (Input.GetAxis("Run") > 0.0f)
                {
                    transform.Translate(direction * runSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    }
}
