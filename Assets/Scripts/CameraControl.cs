using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && GetComponent<Camera>().orthographicSize > 1f) {
            GetComponent<Camera>().orthographicSize -= 1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && GetComponent<Camera>().orthographicSize < 10f) {
            GetComponent<Camera>().orthographicSize += 1f;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        GetComponent<Camera>().transform.position = GetComponent<Camera>().transform.position + new Vector3(horizontalInput * Time.deltaTime * 2f, verticalInput * Time.deltaTime * 2f, 0);
    }
}
