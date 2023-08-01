using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public float turnVel;
    public float mainAccel;
    public float mainMax;
    public float reverseAccel;
    public float reverseMax;
    private float _turnVel;
    private float _mainVel;

    // Start is called before the first frame update
    void Start()
    {
        _mainVel = 0;
        turnVel = GetComponent<ShipStats>().turnVel;
        mainAccel = 0.00005f * GetComponent<ShipStats>().mainAccel;
        reverseAccel = 0.005f * GetComponent<ShipStats>().reverseAccel;
        mainMax = 0.005f * GetComponent<ShipStats>().mainMax;
        reverseMax = 0.005f * GetComponent<ShipStats>().reverseMax;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * _mainVel;

        Vector2 dir = (Vector2)target.position - (Vector2)transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        float angleToTarget = Mathf.Abs(transform.rotation.eulerAngles.z - angle) % 360;
        // Debug.Log(angleToTarget);
        if (angleToTarget > 90 && angleToTarget < 270) {
            _mainVel = (_mainVel < -reverseMax) ? -reverseMax : _mainVel - reverseAccel;
        } else {
            _mainVel = (_mainVel > mainMax) ? mainMax : _mainVel + mainAccel;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), turnVel *Time.deltaTime);
    }
}
