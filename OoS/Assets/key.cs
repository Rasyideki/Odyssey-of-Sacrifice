using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public GameObject Completediconkey;
    public Vector3 spinRotationSpeed = new Vector3(0, 100, 0);

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;

        UpdateCompletedIcon();
    }

    private void UpdateCompletedIcon()
    {
        if (GoalManager.singleton.canEnterCastle)
        {
            Completediconkey.SetActive(true);
        }
        else
        {
            Completediconkey.SetActive(false);
        }
    }
}
