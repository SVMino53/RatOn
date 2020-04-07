using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_scr : MonoBehaviour
{
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = playerObj.transform.position;
        newPosition.z = -10.0f;

        transform.position = newPosition;
    }
}
