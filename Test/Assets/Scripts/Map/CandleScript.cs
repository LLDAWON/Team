using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CandleScript : MonoBehaviour
{
    public bool _isLit = false;
    public float _lightRadius = 5.0f; // ÃÐºÒÀÇ ¹à±â ¹üÀ§

    private ParticleSystem _fire;

    GameObject _fireObject;
    GameObject _flashPoint;

    void Awake()
    {
        //ÆÄÆ¼Å¬ on&off
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
    public void SetLit(bool islitOn)
    {
        if (_isLit) return;

        _fireObject.SetActive(islitOn);
        _flashPoint.SetActive(islitOn);
        _isLit = true;
        _fire.Play();

        UIManager.Instance.AddCandle();

       
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
