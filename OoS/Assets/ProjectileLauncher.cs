using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform LaunchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        Vector3 oriScale = projectile.transform.localScale;

        projectile.transform.localScale = new Vector3(oriScale.x * transform.localScale.x > 0 ? 1 : -1, oriScale.y, oriScale.z);
    }
}
