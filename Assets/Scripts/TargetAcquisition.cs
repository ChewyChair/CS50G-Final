using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAcquisition : MonoBehaviour
{
    private static List<TargetAcquisition> enemyList;
    private static List<TargetAcquisition> enemySCList;
    private static List<TargetAcquisition> allyList;
    private static List<TargetAcquisition> allySCList;
    private static bool noEnemiesLeft;
    private static bool noAlliesLeft;

    // Start is called before the first frame update 
    void Start()
    {
        noEnemiesLeft = false;
        noAlliesLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        enemyList = new List<TargetAcquisition>(); 
        enemySCList = new List<TargetAcquisition>(); 
        allyList = new List<TargetAcquisition>(); 
        allySCList = new List<TargetAcquisition>(); 
    }

    public Vector3 getPosition() {
        return transform.position;
    }

    private static bool isInArc(Vector3 targetPos, Vector3 pos, float minAngle, float maxAngle) {
        Vector2 dir = (Vector2)targetPos - (Vector2)pos;
        dir.Normalize();
        float angle = absoluteAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
        // Case where the firing arc is unbroken
        bool isWithinArcCaseOne = (minAngle < maxAngle && angle > minAngle && angle < maxAngle);
        // Case where the firing arc is broken, ie the 0 degree point cuts the arc somewhere
        bool isWithinArcCaseTwo = (minAngle > maxAngle && (angle < maxAngle || angle > minAngle));
        if (isWithinArcCaseOne || isWithinArcCaseTwo) {
            return true;
        }
        return false;
    }

    private static float angleToTurn(float currAngle, Vector3 pos, Vector3 targetPos) {
        Vector2 dir = (Vector2)targetPos - (Vector2)pos;
        dir.Normalize();
        float angle = absoluteAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
        return Mathf.Abs(angle - currAngle) % 360;
    }

    public static TargetAcquisition findEnemy(Vector3 pos, float range, float currAngle, float minAngle, float maxAngle) {
        TargetAcquisition closest = null;
        TargetAcquisition closestInArc = null;
        if (enemyList == null) return null;
        for (int i = 0; i < enemyList.Count; i++) {
            TargetAcquisition curr = enemyList[i];
            // ignore targets not in range
            if (Vector3.Distance(pos, curr.getPosition()) > range) {
                continue;
            }
            // check if in arc, handles it separately
            if (isInArc(curr.getPosition(), pos, minAngle, maxAngle)) {
                if (closestInArc == null) {
                    closestInArc = curr;
                }
                if (angleToTurn(currAngle, pos, curr.getPosition()) < angleToTurn(currAngle, pos, closestInArc.getPosition())) {
                    closestInArc = curr;
                }
            }
            if (closest == null) {
                closest = curr;
            }
            if (Vector3.Distance(pos, curr.getPosition()) < Vector3.Distance(pos, closest.getPosition())) {
                closest = curr;
            }
        }
        // prioritise closest in arc and in range, otherwise we return the closest target
        return (closestInArc) ? closestInArc : closest;
    }

    public static TargetAcquisition findEnemySC(Vector3 pos, float range, float currAngle, float minAngle, float maxAngle) {
        TargetAcquisition closest = null;
        TargetAcquisition closestInArc = null;
        if (enemySCList == null) return null;
        for (int i = 0; i < enemySCList.Count; i++) {
            TargetAcquisition curr = enemySCList[i];
            if (Vector3.Distance(pos, curr.getPosition()) > range) {
                continue;
            }
            if (isInArc(curr.getPosition(), pos, minAngle, maxAngle)) {
                if (closestInArc == null) {
                    closestInArc = curr;
                }
                if (angleToTurn(currAngle, pos, curr.getPosition()) < angleToTurn(currAngle, pos, closestInArc.getPosition())) {
                    closestInArc = curr;
                }
            }
            if (closest == null) {
                closest = curr;
            }
            if (Vector3.Distance(pos, curr.getPosition()) < Vector3.Distance(pos, closest.getPosition())) {
                closest = curr;
            }
        }
        return (closestInArc) ? closestInArc : closest;
    }

    public static TargetAcquisition findAlly(Vector3 pos, float range, float currAngle, float minAngle, float maxAngle) {
        TargetAcquisition closest = null;
        TargetAcquisition closestInArc = null;
        if (allyList == null) return null;
        for (int i = 0; i < allyList.Count; i++) {
            TargetAcquisition curr = allyList[i];
            if (Vector3.Distance(pos, curr.getPosition()) > range) {
                continue;
            }
            if (isInArc(curr.getPosition(), pos, minAngle, maxAngle)) {
                if (closestInArc == null) {
                    closestInArc = curr;
                }
                if (angleToTurn(currAngle, pos, curr.getPosition()) < angleToTurn(currAngle, pos, closestInArc.getPosition())) {
                    closestInArc = curr;
                }
            }
            if (closest == null) {
                closest = curr;
            }
            if (Vector3.Distance(pos, curr.getPosition()) < Vector3.Distance(pos, closest.getPosition())) {
                closest = curr;
            }
        }
        return (closestInArc) ? closestInArc : closest;
    }

    public static TargetAcquisition findAllySC(Vector3 pos, float range, float currAngle, float minAngle, float maxAngle) {
        TargetAcquisition closest = null;
        TargetAcquisition closestInArc = null;
        if (allySCList == null) return null;
        for (int i = 0; i < allySCList.Count; i++) {
            TargetAcquisition curr = allySCList[i];
            if (Vector3.Distance(pos, curr.getPosition()) > range) {
                continue;
            }
            if (isInArc(curr.getPosition(), pos, minAngle, maxAngle)) {
                if (closestInArc == null) {
                    closestInArc = curr;
                }
                if (angleToTurn(currAngle, pos, curr.getPosition()) < angleToTurn(currAngle, pos, closestInArc.getPosition())) {
                    closestInArc = curr;
                }
            }
            if (closest == null) {
                closest = curr;
            }
            if (Vector3.Distance(pos, curr.getPosition()) < Vector3.Distance(pos, closest.getPosition())) {
                closest = curr;
            }
        }
        return (closestInArc) ? closestInArc : closest;
    }

    public void addList(bool ENEMY) {
        if (ENEMY) {
            TargetAcquisition enemy = gameObject.transform.GetComponent<TargetAcquisition>();
            if (TargetAcquisition.enemyList == null) TargetAcquisition.enemyList = new List<TargetAcquisition>();
            TargetAcquisition.enemyList.Add(enemy);
        }
        else {
            TargetAcquisition ally = gameObject.transform.GetComponent<TargetAcquisition>();
            if (TargetAcquisition.allyList == null) TargetAcquisition.allyList = new List<TargetAcquisition>();     
            TargetAcquisition.allyList.Add(ally);       
        }
    }

    public void addSCList(bool ENEMY) {
        if (ENEMY) {
            TargetAcquisition enemy = gameObject.transform.GetComponent<TargetAcquisition>();
            if (TargetAcquisition.enemySCList == null) TargetAcquisition.enemySCList = new List<TargetAcquisition>();
            TargetAcquisition.enemySCList.Add(enemy);
        }
        else {
            TargetAcquisition ally = gameObject.transform.GetComponent<TargetAcquisition>();
            if (TargetAcquisition.allySCList == null) TargetAcquisition.allySCList = new List<TargetAcquisition>();     
            TargetAcquisition.allySCList.Add(ally);       
        }
    }

    public void removeList(bool ENEMY) {
        if (ENEMY) {
            enemyList.Remove(this);
            if (enemyList.Count == 0) {
                noEnemiesLeft = true;
            }
        }
        else {
            allyList.Remove(this);
            if (allyList.Count == 0) {
                noAlliesLeft = true;
            }
        }
    }

    public void removeSCList(bool ENEMY) {
        if (ENEMY) {
            enemySCList.Remove(this);
        }
        else {
            allySCList.Remove(this);
        }
    }

    private static float absoluteAngle(float angle) {
        if (angle < 0) {
            return 360 + angle;
        } else {
            return angle % 360;
        }
    }

    public static bool isEnemyVictory() {
        return noAlliesLeft;
    }

    public static bool isAlliedVictory() {
        return noEnemiesLeft;
    }
}
