﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overmind : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> enemMelee;
    private GameObject MeleeL;
    private GameObject MeleeR;
    [HideInInspector]
    public List<GameObject> enemRanged;
    private GameObject RangedL;
    private GameObject RangedR;
    [HideInInspector]
    public GameObject enemMiniboss;

    private float aggroTimerMelee;
    private float aggroTimerRanged;
    private float aggroTimerMiniboss;

    // Start is called before the first frame update
    void Start()
    {
        enemMelee = new List<GameObject>();
        enemRanged = new List<GameObject>();
        enemMiniboss = null;
        aggroTimerMelee = Random.Range(1f, 4f);
        aggroTimerRanged = Random.Range(2f, 5f);
        aggroTimerMiniboss = Random.Range(4f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        // Update melee
        aggroTimerMelee -= Time.deltaTime;
        if(aggroTimerMelee <= 0)
        {
            aggroTimerMelee = Random.Range(1f, 4f);
            if(enemMelee.Count > 0)
            {
                CallMeleeAttack();
            }
        }

        // Update ranged
        aggroTimerRanged -= Time.deltaTime;
        if(aggroTimerRanged <= 0)
        {
            aggroTimerRanged = Random.Range(2f, 5f);
            if(enemRanged.Count > 0)
            {
                CallRangedAttack();
            }
        }

        // Update miniboss if one exists
        if(enemMiniboss != null && enemMiniboss.GetComponent<EnemyMovement>().state != "attacking" && enemMiniboss.GetComponent<EnemyMovement>().state != "doingattack" && enemMiniboss.GetComponent<EnemyMovement>().state != "stunned")
        {
            aggroTimerMiniboss -= Time.deltaTime;
            if (aggroTimerMiniboss <= 0)
            {
                aggroTimerMiniboss = Random.Range(4f, 8f);
                CallMinibossAttack();
            }
        }

        UpdateAttackers();
    }

    // Calls a melee attack from the closest enemy to the player
    private void CallMeleeAttack()
    {
        GameObject closestL = null;
        GameObject closestR = null;
        float distL = 9999;
        float distR = 9999;

        // Get the closest enemies on the left and right
        foreach(GameObject e in enemMelee)
        {
            if(e.GetComponent<EnemyMovement>().direction == 1) // enemy is on the LEFT
            {
                if(e.GetComponent<EnemyMovement>().pDist < distL)
                {
                    closestL = e;
                    distL = e.GetComponent<EnemyMovement>().pDist;
                }
            }
            else // enemy is on the RIGHT
            {
                if (e.GetComponent<EnemyMovement>().pDist < distR)
                {
                    closestR = e;
                    distR = e.GetComponent<EnemyMovement>().pDist;
                }
            }
        }

        // No enemies exist, or all enemy slots are taken
        if(closestR == null && closestL == null || (MeleeR != null && MeleeL != null))
        {
            return;
        }

        // Special cases if enemies are all on one side
        if(closestL == null)
        {
            if (closestR != null && MeleeR == null)
            {
                MeleeR = closestR;
                SetToAttack(MeleeR);
            }
            return;
        }
        if (closestR == null)
        {
            if (closestL != null && MeleeL == null)
            {
                MeleeL = closestL;
                SetToAttack(MeleeL);
            }
            return;
        }

        // Attack a side with no current attackers. If both sides have attackers, do nothing.
        if (MeleeR == null && MeleeL == null)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                MeleeR = closestR;
                SetToAttack(MeleeR);
                return;
            }
            else if (rand == 1)
            {
                MeleeL = closestL;
                SetToAttack(MeleeL);
                return;
            }
        }
        else if (MeleeR == null && MeleeL != null)
        {
            MeleeR = closestR;
            SetToAttack(MeleeR);
            return;
        }
        else if (MeleeL == null && MeleeR != null)
        {
            MeleeL = closestL;
            SetToAttack(MeleeL);
            return;
        }
    }
    
    // Calls a ranged attack from the closest ranged enemy to the player
    private void CallRangedAttack()
    {
        GameObject closestL = null;
        GameObject closestR = null;
        float distL = 9999;
        float distR = 9999;

        // Get the closest enemies on the left and right
        foreach (GameObject e in enemRanged)
        {
            if (e.GetComponent<EnemyMovement>().direction == 1) // enemy is on the LEFT
            {
                if (e.GetComponent<EnemyMovement>().pDist < distL)
                {
                    closestL = e;
                    distL = e.GetComponent<EnemyMovement>().pDist;
                }
            }
            else // enemy is on the RIGHT
            {
                if (e.GetComponent<EnemyMovement>().pDist < distR)
                {
                    closestR = e;
                    distR = e.GetComponent<EnemyMovement>().pDist;
                }
            }
        }

        // No enemies exist, or all enemy slots are taken
        if (closestR == null && closestL == null || (RangedR != null && RangedL != null))
        {
            return;
        }

        // Special cases if enemies are all on one side
        if (closestL == null)
        {
            if (closestR != null && RangedR == null)
            {
                RangedR = closestR;
                SetToAttack(RangedR);
            }
            return;
        }
        if (closestR == null)
        {
            if (closestL != null && RangedL == null)
            {
                RangedL = closestL;
                SetToAttack(RangedL);
            }
            return;
        }

        // Attack a side with no current attackers. If both sides have attackers, do nothing.
        if (RangedR == null && RangedL == null)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                RangedR = closestR;
                SetToAttack(RangedR);
                return;
            }
            else if (rand == 1)
            {
                RangedL = closestL;
                SetToAttack(RangedL);
                return;
            }
        }
        else if (RangedR == null && RangedL != null)
        {
            RangedR = closestR;
            SetToAttack(RangedR);
            return;
        }
        else if (RangedL == null && RangedR != null)
        {
            RangedL = closestL;
            SetToAttack(RangedL);
            return;
        }
    }

    // Tells the miniboss to attack
    private void CallMinibossAttack()
    {
        if(enemMiniboss != null)
        {
            SetToAttack(enemMiniboss);
        }
    }

    // Tells the enemy that they want to attack when available.
    private void SetToAttack(GameObject enem)
    {
        if(enem.GetComponent<EnemyMovement>().state != "attacking" && enem.GetComponent<EnemyMovement>().state != "doingattack")
        {
            enem.GetComponent<EnemyMovement>().wantsToAttack = true;
        }
    }

    private void UpdateAttackers()
    {
        // Check all attacking enemies. If they are not currently moving to attack or attacking, free those enemy slots.
        if(MeleeL != null)
        {
            if(MeleeL.GetComponent<EnemyMovement>().state != "attacking" && MeleeL.GetComponent<EnemyMovement>().state != "doingattack")
            {
                MeleeL = null;
            }
        }
        if (MeleeR != null)
        {
            if (MeleeR.GetComponent<EnemyMovement>().state != "attacking" && MeleeR.GetComponent<EnemyMovement>().state != "doingattack")
            {
                MeleeR = null;
            }
        }
        if (RangedL != null)
        {
            if (RangedL.GetComponent<EnemyMovement>().state != "attacking" && RangedL.GetComponent<EnemyMovement>().state != "doingattack")
            {
                RangedL = null;
            }
        }
        if (RangedR != null)
        {
            if (RangedR.GetComponent<EnemyMovement>().state != "attacking" && RangedR.GetComponent<EnemyMovement>().state != "doingattack")
            {
                RangedR = null;
            }
        }
    }

    public void AddMelee(GameObject obj)
    {
        enemMelee.Add(obj);
    }

    public void RemoveMelee(GameObject obj)
    {
        enemMelee.Remove(obj);
    }

    public void AddRanged(GameObject obj)
    {
        enemRanged.Add(obj);

    }

    public void RemoveRanged(GameObject obj)
    {
        enemRanged.Remove(obj);
    }

    public void AddMiniboss(GameObject obj)
    {
        enemMiniboss = obj;
    }

    public void RemoveMiniboss()
    {
        enemMiniboss = null;
    }

    public bool areThereEnemies()
    {
        if(enemMelee.Count == 0 && enemRanged.Count == 0 && enemMiniboss == null)
        {
            return true;
        }
        return false;
    }

    public void ClearLists()
    {
        enemMelee.Clear();
        enemRanged.Clear();
        MeleeL = null;
        MeleeR = null;
        RangedL = null;
        RangedR = null;
        enemMiniboss = null;
        aggroTimerMelee = Random.Range(1f, 4f);
        aggroTimerRanged = Random.Range(2f, 5f);
        aggroTimerMiniboss = Random.Range(4f, 8f);
    }
}
