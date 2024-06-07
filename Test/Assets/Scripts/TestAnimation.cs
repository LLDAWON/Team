using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animation[_animation.clip.name].speed = 1.0f;
            _animation[_animation.clip.name].time = 0;
            _animation.Play();

            SoundManager.Instance.Play2D("Door");
        }

        if (Input.GetMouseButtonDown(1))
        {
            _animation[_animation.clip.name].speed = -1.0f;
            _animation[_animation.clip.name].time = _animation[_animation.clip.name].length;
            _animation.Play();

            SoundManager.Instance.Play2D("Door");
        }
    }
}
