using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float dmgHul;
    public float time;
    public bool ENEMY;
    public Rigidbody2D rb;
    public Transform impact;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = (ENEMY) ? 21 : 11;
    }

    // Update is called once per frame
    void Update()
    {
        time = (time < 0) ? 0 : time - Time.deltaTime;
        if (time == 0) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == 12 || col.gameObject.layer == 22) {
            MissileStats missileStats = col.gameObject.GetComponent<MissileStats>();
            if ((ENEMY && !missileStats.ENEMY) || !ENEMY && missileStats.ENEMY) {
                missileStats.curHul -= (dmgHul - missileStats.arm < 0) ? 0 : dmgHul - missileStats.arm;
                Transform b = Instantiate(impact, rb.position, Quaternion.identity); 
                b.transform.rotation = transform.rotation;
                Destroy(gameObject);
            } 
        } else {
            ShipStats shipStats = col.gameObject.GetComponent<ShipStats>();
            if ((ENEMY && !shipStats.ENEMY) || !ENEMY && shipStats.ENEMY) {
                shipStats.curHul -= (dmgHul - shipStats.arm < 0) ? 0 : dmgHul - shipStats.arm;
                Transform b = Instantiate(impact, rb.position, Quaternion.identity); 
                b.transform.rotation = transform.rotation;
                Destroy(gameObject);
            } 
        }
    }
}
