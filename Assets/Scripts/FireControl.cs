using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public TargetAcquisition target;
    public Transform Bullet;
    public Rigidbody2D rb;
    public ParticleSystem ps;
    public bool ENEMY;
    public float turnSpeed;
    public float targetRange;
    private float _targetRange;
    public float fireVel;
    private float _fireVel;
    public float fireRate;
    public float fireSpread;
    private float _fireSpread;
    private float fireCD;
    public float dmgHul;

    public bool MISSILE;
    public float missileHp;
    public float missileArm;
    public float missileTurnVel;

    public bool CIWS;

    public float rotationOffset;
    public float firingArc;

    public float minAngle;
    public float maxAngle;
    public float currAngle;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        ENEMY = transform.parent.GetComponent<ShipStats>().ENEMY;
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        _targetRange = targetRange * 0.1f;
        minAngle = absoluteAngle(transform.parent.rotation.eulerAngles.z + rotationOffset - firingArc / 2);
        maxAngle = absoluteAngle(transform.parent.rotation.eulerAngles.z + rotationOffset + firingArc / 2);
        currAngle = absoluteAngle(transform.rotation.eulerAngles.z);
        findTarget(ENEMY);
        fireCD = (fireCD < 0) ? 0 : fireCD - Time.deltaTime;
        if (target != null) {
            Vector2 dir = (Vector2)target.transform.position - (Vector2)transform.position;
            dir.Normalize();
            angle = absoluteAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            // Case where the firing arc is unbroken
            bool isWithinArcCaseOne = (minAngle < maxAngle && angle > minAngle && angle < maxAngle);
            // Case where the firing arc is broken, ie the 0 degree point cuts the arc somewhere
            bool isWithinArcCaseTwo = (minAngle > maxAngle && (angle < maxAngle || angle > minAngle));

            var r = transform.eulerAngles;
            if (isWithinArcCaseOne) {
                // Simple turn towards the direction
                if (currAngle < angle) {
                    transform.rotation = Quaternion.Euler(r.x, r.y, (r.z + turnSpeed*Time.deltaTime > maxAngle) ? maxAngle : r.z + turnSpeed*Time.deltaTime);
                } else {
                    transform.rotation = Quaternion.Euler(r.x, r.y, (r.z - turnSpeed*Time.deltaTime < minAngle) ? minAngle : r.z - turnSpeed*Time.deltaTime);
                }
            } else if (isWithinArcCaseTwo) {
                // if (angle > maxAngle) {
                //     if (currAngle > minAngle) {
                //         // Current angle is in the same half as the target angle, can turn towards it
                //         if (currAngle < angle) {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, r.z + turnSpeed*Time.deltaTime);
                //         } else {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, (absoluteAngle(r.z - turnSpeed*Time.deltaTime) < minAngle) ? minAngle : r.z - turnSpeed*Time.deltaTime);
                //         }
                //         // Current angle is in the other half, so turn towards the half with the target
                //     } else {
                //         if (currAngle < angle) {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, r.z + turnSpeed*Time.deltaTime);
                //         } else {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, r.z - turnSpeed*Time.deltaTime);
                //         }
                //         // transform.rotation = Quaternion.Euler(r.x, r.y, r.z - turnSpeed*Time.deltaTime);
                //     }
                // } else {
                //     if (currAngle < maxAngle) {
                //         if (currAngle < angle) {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, (absoluteAngle(r.z + turnSpeed*Time.deltaTime) > maxAngle) ? maxAngle : r.z + turnSpeed*Time.deltaTime);
                //         } else {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, r.z - turnSpeed*Time.deltaTime);
                //         }
                //     } else {
                //         if (currAngle < angle) {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, r.z + turnSpeed*Time.deltaTime);
                //         } else {
                //             transform.rotation = Quaternion.Euler(r.x, r.y, r.z - turnSpeed*Time.deltaTime);
                //         }
                //         // transform.rotation = Quaternion.Euler(r.x, r.y, r.z - turnSpeed*Time.deltaTime);
                //     }
                // }
                if (currAngle < angle) {
                    if ((minAngle < angle && minAngle > currAngle) && (maxAngle < angle && maxAngle > currAngle)) {
                        transform.rotation = Quaternion.Euler(r.x, r.y, (absoluteAngle(r.z - turnSpeed*Time.deltaTime) < minAngle && absoluteAngle(r.z - turnSpeed*Time.deltaTime) > maxAngle) ? minAngle : r.z - turnSpeed*Time.deltaTime);
                    } else {
                        transform.rotation = Quaternion.Euler(r.x, r.y, (absoluteAngle(r.z + turnSpeed*Time.deltaTime) > maxAngle && absoluteAngle(r.z + turnSpeed*Time.deltaTime) < minAngle) ? maxAngle : r.z + turnSpeed*Time.deltaTime);
                    }
                } else {
                    if ((minAngle < currAngle && minAngle > angle) && (maxAngle < currAngle && maxAngle > angle)) {
                        transform.rotation = Quaternion.Euler(r.x, r.y, (absoluteAngle(r.z + turnSpeed*Time.deltaTime) > maxAngle && absoluteAngle(r.z + turnSpeed*Time.deltaTime) < minAngle) ? maxAngle : r.z + turnSpeed*Time.deltaTime);
                    } else {
                        transform.rotation = Quaternion.Euler(r.x, r.y, (absoluteAngle(r.z - turnSpeed*Time.deltaTime) < minAngle && absoluteAngle(r.z - turnSpeed*Time.deltaTime) > maxAngle) ? minAngle : r.z - turnSpeed*Time.deltaTime);
                    }
                }
            } 
            float angleToTarget = Mathf.Abs(transform.rotation.eulerAngles.z - angle) % 360;
            if (fireCD == 0 && (angleToTarget < 20 || angleToTarget > 340)) Shoot();
        }
    }
    
    float absoluteAngle(float angle) {
        if (angle < 0) {
            return 360 + angle;
        } else {
            return angle % 360;
        }
    }

    void Shoot() {
        _fireVel = fireVel * 0.1f;
        _fireSpread = fireSpread * 0.1f;
        Transform b = Instantiate(Bullet, rb.position, Quaternion.identity); 
        if (MISSILE) {
            b.GetComponent<Missile>().dmgHul = dmgHul;
            b.GetComponent<Missile>().ENEMY = ENEMY;
            b.GetComponent<MissileStats>().ENEMY = ENEMY;
            b.GetComponent<MissileStats>().maxHul = missileHp;
            b.GetComponent<MissileStats>().arm = missileArm;
            b.GetComponent<Missile>().time = 1.1f * _targetRange / _fireVel;
            b.GetComponent<Missile>().target = target;
            b.GetComponent<Missile>().turnVel = missileTurnVel;
            b.GetComponent<Missile>().velocity = _fireVel;
        } else {
            b.GetComponent<Bullet>().dmgHul = dmgHul;
            b.GetComponent<Bullet>().ENEMY = ENEMY;
            b.GetComponent<Bullet>().time = 1.1f * _targetRange / _fireVel;
        }
        b.transform.rotation = transform.rotation;
        b.transform.Rotate(0, 0, Random.Range(-_fireSpread, _fireSpread));
        b.GetComponent<Rigidbody2D>().velocity = b.transform.up * _fireVel + new Vector3(rb.velocity.x, rb.velocity.y, 0);
        ps.Play();
        ps.Stop();
        fireCD = 60/fireRate;
    }

    void findTarget(bool ENEMY) {
        if (CIWS) {
            target = (ENEMY) ? TargetAcquisition.findAllySC(transform.position, _targetRange, currAngle, minAngle, maxAngle) : TargetAcquisition.findEnemySC(transform.position, _targetRange, currAngle, minAngle, maxAngle);
            if (target == null) {
                target = (ENEMY) ? TargetAcquisition.findAlly(transform.position, _targetRange, currAngle, minAngle, maxAngle) : TargetAcquisition.findEnemy(transform.position, _targetRange, currAngle, minAngle, maxAngle);
            }
        } else {
            target = (ENEMY) ? TargetAcquisition.findAlly(transform.position, _targetRange, currAngle, minAngle, maxAngle) : TargetAcquisition.findEnemy(transform.position, _targetRange, currAngle, minAngle, maxAngle);
        }
    }
}
