using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public TargetAcquisition target;
    public Rigidbody2D rb;
    public bool ENEMY;
    public float targetRange;
    public float engageRange;
    public float turnVel;
    public float mainAccel;
    public float mainMax;
    public float reverseAccel;
    public float reverseMax;
    private float _turnVel;
    private float _mainVel;

    private float minAngle;
    private float maxAngle;
    private float currAngle;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        ENEMY = transform.GetComponent<ShipStats>().ENEMY;
        turnVel = transform.GetComponent<ShipStats>().turnVel;
        mainAccel = 0.00005f * transform.GetComponent<ShipStats>().mainAccel;
        reverseAccel = 0.005f * transform.GetComponent<ShipStats>().reverseAccel;
        mainMax = 0.005f * transform.GetComponent<ShipStats>().mainMax;
        reverseMax = 0.005f * transform.GetComponent<ShipStats>().reverseMax;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        minAngle = absoluteAngle(transform.rotation.eulerAngles.z - 90);
        maxAngle = absoluteAngle(transform.rotation.eulerAngles.z + 90);
        currAngle = absoluteAngle(transform.rotation.eulerAngles.z);

        findTarget(ENEMY);
        
        if (target != null) {
            rb.velocity = transform.up * _mainVel;
            Vector2 dir = (Vector2)target.transform.position - (Vector2)transform.position;
            dir.Normalize();
            float angle = absoluteAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            float angleToTarget = Mathf.Abs(transform.rotation.eulerAngles.z - angle) % 360;

            if (angleToTarget > 90 && angleToTarget < 270) {
                if (Vector3.Distance(transform.position, target.transform.position) > engageRange) {
                    _mainVel = (_mainVel < -reverseMax) ? -reverseMax : _mainVel - reverseAccel;
                }
            } else {
                if (Vector3.Distance(transform.position, target.transform.position) < engageRange) {
                    if (_mainVel > 0) {
                        _mainVel =  _mainVel - reverseAccel;
                    } else if (_mainVel < 0) {
                        _mainVel =  _mainVel + reverseAccel;
                    }
                } else {
                    _mainVel = (_mainVel > mainMax) ? mainMax : _mainVel + reverseAccel;
                }
            }

            if (angleToTarget < 355 && angleToTarget > 5) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), turnVel *Time.deltaTime);
            }
        }
    }

    void findTarget(bool ENEMY) {
        target = (ENEMY) ? TargetAcquisition.findAlly(transform.position, targetRange, currAngle, minAngle, maxAngle) : TargetAcquisition.findEnemy(transform.position, targetRange, currAngle, minAngle, maxAngle);
    }

    float absoluteAngle(float angle) {
        if (angle < 0) {
            return 360 + angle;
        } else {
            return angle % 360;
        }
    }
}
