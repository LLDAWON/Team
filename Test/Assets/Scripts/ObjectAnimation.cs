using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimation : MonoBehaviour
{
    private Animation _animation;
    private bool _isOpen;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _isOpen = false;
    }

    public void PlayAnimation()
    {
        if(!_isOpen )
        {
            _animation[_animation.clip.name].speed = 1.0f;
            _animation[_animation.clip.name].time = 0;
            _animation.Play();
            _isOpen= true;
        }
        else
        {
            _animation[_animation.clip.name].speed = -1.0f;
            _animation[_animation.clip.name].time = _animation[_animation.clip.name].length;
            _animation.Play();
            _isOpen = false;
        }
    }
}
