using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _noise;//카메라 흔들림효과


    public Volume _volume;
    private Vignette _vignette;
    private DepthOfField _depthOfField;

    private float _initialAmplitude;
    private float _initialFrequency;
    private float _intensity = 0f;
    private float _targetIntensity = 0.5f;
    private bool _increasing = true;
    private bool _isChased = false;
    public float _speed = 1.0f; // 속도 조절
    public float _vignetteSpeed = 0.5f;



    private Vector3 initialPosition;

    private Quaternion initialRotation;

    private void Awake()
    {
        Instance = this;
        _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _depthOfField);
    }

    private void Start()
    {

        initialPosition = _virtualCamera.transform.position;
        initialRotation = _virtualCamera.transform.rotation;
    }

    public void Shake(float magnitude, float duration)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float magnitude, float duration)
    {
        GameManager.Instance.GetPlayer().GetComponent<PlayerController>().SetIsEventCamera(true);
        float elapsed = 0f;

        while (elapsed < duration)
        {

            _virtualCamera.transform.position = initialPosition + Random.insideUnitSphere * magnitude;
            _virtualCamera.transform.rotation = Quaternion.Euler(initialRotation.eulerAngles + Random.insideUnitSphere * magnitude);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _virtualCamera.transform.position = initialPosition;
        _virtualCamera.transform.rotation = initialRotation;
        GameManager.Instance.GetPlayer().GetComponent<PlayerController>().SetIsEventCamera(false);

    }
    public void ShakeCamera(float intensity, float duration)
    {


        _initialAmplitude = _noise.m_AmplitudeGain;
        _initialFrequency = _noise.m_FrequencyGain;

        _noise.m_AmplitudeGain = intensity;
        _noise.m_FrequencyGain = intensity;
        Invoke("StopShake", duration);
    }
    private void StopShake()
    {
        _noise.m_AmplitudeGain = _initialAmplitude;
        _noise.m_FrequencyGain = _initialFrequency;
    }
    public void StartFuzziness()
    {
        _vignette.color.value = Color.black;
        _vignette.intensity.value = 1.0f;
        _vignette.smoothness.value = 1.0f;
        _depthOfField.focalLength.value = 300.0f;

        StartCoroutine(FuzzinessEffect());

    }

    private IEnumerator FuzzinessEffect()
    {
        int repetitions = 0;
        bool decreasing = true;
        _intensity = 1.0f;
        float focalLength = 300.0f;

        while (repetitions < 3)
        {
            if (decreasing)
            {
                _intensity = Mathf.MoveTowards(_intensity, 0f, _speed * Time.deltaTime);
                focalLength = Mathf.MoveTowards(focalLength, 1f, _speed * Time.deltaTime);

                if (_intensity <= 0f)
                {
                    decreasing = false;
                }
            }
            else
            {
                _intensity = Mathf.MoveTowards(_intensity, 1.0f, _speed * Time.deltaTime);
                focalLength = Mathf.MoveTowards(focalLength, 300f, _speed * Time.deltaTime);
                if (_intensity >= 1.0f)
                {
                    decreasing = true;
                    repetitions++;
                }
            }
            _depthOfField.focalLength.value = focalLength;
            _vignette.intensity.value = _intensity;
            yield return null;
        }

        _vignette.intensity.value = 0f;
        _depthOfField.focalLength.value = 1f;

    }

    public void StartVignette()
    {
        if (_isChased)
            return;
        _isChased = true;
        _vignette.color.value = Color.red;
        _vignette.smoothness.value = 0.5f;
        StartCoroutine(RedVignette());
    }
    public void StopVignette()
    {
        _isChased = false;
    }
    private IEnumerator RedVignette()
    {
        while (_isChased)
        {
            if (_increasing)
            {
                _intensity = Mathf.MoveTowards(_intensity, _targetIntensity, _vignetteSpeed * Time.deltaTime);
                if (_intensity >= _targetIntensity)
                {
                    _increasing = false;
                }
            }
            else
            {
                _intensity = Mathf.MoveTowards(_intensity, 0f, _vignetteSpeed * Time.deltaTime);
                if (_intensity <= 0f)
                {
                    _increasing = true;
                }
            }
            _vignette.intensity.value = _intensity;
            yield return null;
        }

        _vignette.intensity.value = 0f;
    }

}
