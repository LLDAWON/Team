using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CandleScript : MonoBehaviour
{
    public bool _isLit = false;
    public float _lightRadius = 5.0f; // �к��� ��� ����

    private NavMeshObstacle navMeshObstacle;
    private ParticleSystem _fire;

    GameObject _fireObject;
    GameObject _flashPoint;

    void Awake()
    {
        navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        //navMeshObstacle.shape = NavMeshObstacleShape.Cylinder;
        navMeshObstacle.radius = _lightRadius;
        navMeshObstacle.height = 0.5f; // ���� ����
        navMeshObstacle.carving = false; // ���� ��ֹ��� ����
        UpdateObstacle();
        //��ƼŬ on&off
        _fireObject = transform.GetChild(0).gameObject;
        _flashPoint = transform.GetChild(2).gameObject;
        _fire = _fireObject.GetComponent<ParticleSystem>();

        _fireObject.SetActive(false);
        _flashPoint.SetActive(false);
        _fire.Stop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _isLit ? Color.yellow : Color.gray;
        Gizmos.DrawWireSphere(transform.position, _lightRadius);
    }
    void UpdateObstacle()
    {
        navMeshObstacle.enabled = _isLit;
    }

    public void SetLit(bool islitOn)
    {
        _fireObject.SetActive(islitOn);
        _flashPoint.SetActive(islitOn);
        _isLit = true;
        _fire.Play();
        UpdateObstacle();
    }
    public bool GetLit() { return _isLit;}

    public bool IsWithinLight(Vector3 position)
    {
        if (!_isLit)
            return false;

        float distance = Vector3.Distance(position, transform.position);
        return distance <= _lightRadius;
    }
}
