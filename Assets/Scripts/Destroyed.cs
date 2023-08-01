using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : MonoBehaviour
{
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps.Play();
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
