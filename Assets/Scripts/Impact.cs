using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public ParticleSystem ps;
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        ps.Play();
        timer = 0;
    }

    public void Update()
    {
        timer++;
        if (timer > 120) {
            Destroy(gameObject);
        }
    }
}
