using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileStats : MonoBehaviour
{
    public float arm;
    public float maxHul;
    public float curHul;
    public bool ENEMY;
    public Rigidbody2D rb;
    public Transform impact;

    // Start is called before the first frame update
    void Start()
    {
        if (ENEMY) {
            gameObject.layer = 22;
        } else {
            gameObject.layer = 12;
        }
        curHul = maxHul;
        gameObject.GetComponent<TargetAcquisition>().addSCList(ENEMY);
    }

    // Update is called once per frame
    void Update()
    {
        if (curHul <= 0) destroyShip();
    }

    public void destroyShip() {
        gameObject.GetComponent<TargetAcquisition>().removeSCList(ENEMY);
        Transform b = Instantiate(impact, rb.position, Quaternion.identity); 
        b.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }
}
