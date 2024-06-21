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

    [SerializeField]
    private Vector3 _offset;

    void Start()
    {
        playerIconRectTransform = GetComponent<RectTransform>();
        playerTransform = GameManager.Instance.GetPlayer().transform;

        Vector3 miniMapPos = miniMapCamera.WorldToViewportPoint(playerTransform.position);

        playerIconRectTransform.localPosition = miniMapPos + _offset;
    }

    void Update()
    {
        //Vector3 miniMapPos = miniMapCamera.WorldToViewportPoint(playerTransform.position);

        /*        miniMapPos.x *= miniMapRectTransform.rect.width;
                miniMapPos.y *= miniMapRectTransform.rect.height;*/

        Vector2 worldPos = new Vector2(transform.position.x, transform.position.z) * 4;
        //Vector2 offset = ;
        //
        



        //playerIconRectTransform.localPosition = miniMapPos + _offset;

        if (directionIconRectTransform != null)
        {
            directionIconRectTransform.localRotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.y - 90f);
        }
    }
}
