using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public float arm;
    public float maxHul;
    public float curHul;
    public bool ENEMY;
    public bool STRIKECRAFT;
    public GameObject parent;
    public int index;
    public Transform destroyedModel;
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
        if (STRIKECRAFT) {
            if (ENEMY) {
                gameObject.layer = 23;
            } else {
                gameObject.layer = 13;
            }
            gameObject.GetComponent<TargetAcquisition>().addSCList(ENEMY);
        } else {
            if (ENEMY) {
                gameObject.layer = 20;
            } else {
                gameObject.layer = 10;
            }
            gameObject.GetComponent<TargetAcquisition>().addList(ENEMY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curHul <= 0) destroyShip();
    }

    void destroyShip() {
        if (STRIKECRAFT) {
            if (parent) {
                parent.GetComponent<CarrierControl>().craftDestroyed(index);
            }
            gameObject.GetComponent<TargetAcquisition>().removeSCList(ENEMY);
        } else {
            gameObject.GetComponent<TargetAcquisition>().removeList(ENEMY);
        }
        Transform b = Instantiate(destroyedModel, rb.position, Quaternion.identity); 
        b.transform.rotation = transform.rotation;
        b.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        Destroy(gameObject);
    }
}
