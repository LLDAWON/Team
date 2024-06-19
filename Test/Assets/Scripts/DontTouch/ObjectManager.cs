using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance;
    public static ObjectManager Instance
    { get { return _instance; } }

    public GameObject _bookCase;
   
    private float _speed = 30.0f;
    public float _maxRotationX = 45f;
    private Coroutine _bookCaseRotationCoroutine;

    private void Awake()
    {
        _instance = this;
    }


    public void BookCase()
    {
        if (_bookCaseRotationCoroutine == null)
        {
            _bookCaseRotationCoroutine = StartCoroutine(RotateBookCase()); 
        }
    }

    private IEnumerator RotateBookCase()
    {
        while (!IsBookCaseRotate())
        {
            Vector3 currentRotation = _bookCase.transform.rotation.eulerAngles;
            float newRotationX = Mathf.Clamp(currentRotation.x + _speed * Time.deltaTime, 0f, _maxRotationX);
            _bookCase.transform.rotation = Quaternion.Euler(newRotationX, currentRotation.y, currentRotation.z);
            yield return null; 
        }
        _bookCaseRotationCoroutine = null; 
    }

    private bool IsBookCaseRotate()
    {
        return Mathf.Approximately(_bookCase.transform.rotation.eulerAngles.x, _maxRotationX);
    }


}
