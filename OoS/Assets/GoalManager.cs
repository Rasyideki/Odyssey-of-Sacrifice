using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject Completedicon;
 

    public static GoalManager singleton;

    public intValue _key;


    public int KeyNeeded;
    /*public int KeyCollected;*/
    
    public int FragmentNeeded;
   /* public int FragmentCollected;*/


    public bool canEnterCastle;
    public bool canEnterGate;
    public bool canEnterCave;
    public bool canExitCave;
    public bool canExitCastle;

    private void Awake()
    {
        singleton = this;

    }

    private void Update()
    {
        CollectKey();
        CollectFragment();
    }

    public void CollectKey()
    {
        if (_key.Keyvalue >= KeyNeeded)
        {
            canEnterCastle = true;
        }
    }

    public void CollectFragment()
    {
        if (_key.Fragvalue >= FragmentNeeded)
        {
            Completedicon.SetActive(true);
            canEnterGate = true;
        }
    }
}
