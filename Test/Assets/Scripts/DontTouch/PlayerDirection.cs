using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    public RectTransform miniMapRectTransform;
    public RectTransform directionIconRectTransform;
    public Camera miniMapCamera; 
    private Transform playerTransform; 
    
    [SerializeField]
    private Vector3 _offset;
    private float movementRatioX = 11f;
    private float movementRatioY = -14.5f;


    void Start()
    {
        directionIconRectTransform = GetComponent<RectTransform>();
        playerTransform = GameManager.Instance.GetPlayer().transform;

        Vector3 miniMapPos = miniMapCamera.WorldToViewportPoint(playerTransform.position);

        _offset= new Vector3(-96.6f, 215f, 0f);


        //directionIconRectTransform.localPosition = _offset;

    }

    void Update()
    {

        //Vector2 worldPos = new Vector2(playerTransform.position.z * 30f, playerTransform.position.x * 7f);
       
        float playerMovementZ = playerTransform.position.z * movementRatioX;
        float PlayerMovementX = playerTransform.position.x * movementRatioY;

        Vector3 currentPosition = new Vector3(playerMovementZ, PlayerMovementX, 0f);
        
        directionIconRectTransform.localPosition = currentPosition +_offset; 

        if (directionIconRectTransform != null)
        {
            directionIconRectTransform.localRotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.y - 90f);
        }
    }
}
