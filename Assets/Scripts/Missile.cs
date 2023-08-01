using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float dmgHul;
    public float time;
    public float turnVel;
    public float velocity;
    public bool ENEMY;
    public Rigidbody2D rb;
    public Transform impact;
    public TargetAcquisition target;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = (ENEMY) ? 22 : 12;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            time = (time < 0) ? 0 : time - Time.deltaTime;
            if (time == 0) {
                gameObject.GetComponent<TargetAcquisition>().removeSCList(ENEMY);
                Destroy(gameObject);
            }

            Vector2 dir = (Vector2)target.transform.position - (Vector2)transform.position;
            dir.Normalize();
            float angle = absoluteAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), turnVel *Time.deltaTime);
            rb.velocity = transform.up * velocity;
        } 
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == 11 || col.gameObject.layer == 21) {
            // do nothing, handled by bullet.cs
        } else {
            ShipStats shipStats = col.gameObject.GetComponent<ShipStats>();
            if ((ENEMY && !shipStats.ENEMY) || !ENEMY && shipStats.ENEMY) {
                shipStats.curHul -= (dmgHul - shipStats.arm < 0) ? 0 : dmgHul - shipStats.arm;
                Transform b = Instantiate(impact, rb.position, Quaternion.identity); 
                b.transform.rotation = transform.rotation;
                gameObject.GetComponent<TargetAcquisition>().removeSCList(ENEMY);
                Destroy(gameObject);
            } 
        }
    }

    float absoluteAngle(float angle) {
        if (angle < 0) {
            return 360 + angle;
        } else {
            return angle % 360;
        }
    }
}
