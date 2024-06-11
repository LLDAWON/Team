using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CandleScript : MonoBehaviour
{
    public bool isLit = false;
    public float lightRadius = 5.0f; // �к��� ��� ����
    private NavMeshObstacle navMeshObstacle;

    void Awake()
    {
        navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        //navMeshObstacle.shape = NavMeshObstacleShape.Cylinder;
        navMeshObstacle.radius = lightRadius;
        navMeshObstacle.height = 0.5f; // ���� ����
        navMeshObstacle.carving = false; // ���� ��ֹ��� ����
        UpdateObstacle();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isLit ? Color.yellow : Color.gray;
        Gizmos.DrawWireSphere(transform.position, lightRadius);
    }

    void UpdateObstacle()
    {
        navMeshObstacle.enabled = isLit;
    }

    public void SetLit(bool islitOn)
    {
        isLit = islitOn;
        UpdateObstacle();
    }

    public bool IsWithinLight(Vector3 position)
    {
        if (!isLit)
            return false;

        float distance = Vector3.Distance(position, transform.position);
        return distance <= lightRadius;
    }
}
