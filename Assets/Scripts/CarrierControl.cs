using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierControl : MonoBehaviour
{
    public GameObject strikecraft;
    public float respawnMaxTimer;
    private float respawnTimer;
    public int curSize;
    public int maxSize; 

    // Start is called before the first frame update
    void Start()
    {
        respawnTimer = 0;
        curSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        respawnTimer = (respawnTimer < 0) ? 0 : respawnTimer - Time.deltaTime;
        if (respawnTimer == 0 && curSize != maxSize) {
            launchCraft();
        }
    }

    void launchCraft() {
        GameObject sc = Instantiate(strikecraft, transform.position, Quaternion.identity); 
        sc.GetComponent<ShipStats>().parent = gameObject;
        sc.GetComponent<ShipStats>().STRIKECRAFT = true;
        sc.GetComponent<ShipStats>().ENEMY = transform.parent.GetComponent<ShipStats>().ENEMY;
        sc.transform.rotation = transform.rotation;
        curSize++;
        respawnTimer = respawnMaxTimer;
    }

    public void craftDestroyed(int i) {
        curSize--;
    }
}
