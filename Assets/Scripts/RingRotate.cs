using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotate : MonoBehaviour
{

    public float turnSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var r = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(r.x, r.y, r.z + turnSpeed*Time.deltaTime);
    }
}
