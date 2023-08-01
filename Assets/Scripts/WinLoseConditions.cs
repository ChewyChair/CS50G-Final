using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseConditions : MonoBehaviour
{
    public GameObject winCanvas;
    public GameObject loseCanvas;

    // Update is called once per frame
    void Update()
    {
        CheckWin();
        CheckLose();
    }


    void CheckWin()
    {
        if (TargetAcquisition.isAlliedVictory())
        {
            winCanvas.SetActive(true);
        }
    }

    void CheckLose()
    {
        if (TargetAcquisition.isEnemyVictory())
        {
            loseCanvas.SetActive(true);
        }
    }
}
