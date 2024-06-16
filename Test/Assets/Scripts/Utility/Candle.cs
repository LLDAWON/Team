using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private ParticleSystem _paticle;
    private Light _light;
    private void Awake()
    {
        _paticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        _paticle.Stop();
        _light = transform.GetChild(2).GetComponent<Light>();
        _light.enabled = false;
    }
    public void BurnningCandle()
    {
        _paticle.Play();
        _light.enabled = true;
    }
}
