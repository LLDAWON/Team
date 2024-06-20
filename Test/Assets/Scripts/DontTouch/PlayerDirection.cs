using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    public RectTransform miniMapRectTransform;
    public RectTransform directionIconRectTransform;
    public Camera miniMapCamera; 
    private Transform playerTransform; 
    private RectTransform playerIconRectTransform; 

    void Start()
    {
        playerIconRectTransform = GetComponent<RectTransform>();
        playerTransform = GameManager.Instance.GetPlayer().transform;
    }

    void Update()
    {
        Vector3 miniMapPos = miniMapCamera.WorldToViewportPoint(playerTransform.position);

        miniMapPos.x *= miniMapRectTransform.rect.width;
        miniMapPos.y *= miniMapRectTransform.rect.height;

        miniMapPos.x -= miniMapRectTransform.rect.width * 0.5f;
        miniMapPos.y -= miniMapRectTransform.rect.height * 0.5f;

        playerIconRectTransform.localPosition = miniMapPos;

        if (directionIconRectTransform != null)
        {
            directionIconRectTransform.localRotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.y - 90f);
        }
    }
}
