using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryMove : MonoBehaviour
{
    private float _moveInterval;
    private float _moveDistance = 1.0f;
    private float _rotateAngle = 90.0f; 
    public LayerMask _obstacleMask; 

   

    private void Start()
    {

        StartCoroutine(MoveRoutine());
    }
    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            int randomDirection = Random.Range(0, 2);

            Quaternion newRotation = transform.rotation;

            if (randomDirection == 0)
            {
                newRotation *= Quaternion.Euler(0, _rotateAngle, 0);
            }
            else
            {
                newRotation *= Quaternion.Euler(0, -_rotateAngle, 0);
            }

            bool collisionDetected = false;
            yield return StartCoroutine(CheckCollision(newRotation, (result) => collisionDetected = result));

            if (!collisionDetected)
            {
                yield return StartCoroutine(MoveTo(newRotation));
            }
            _moveInterval = Random.Range(5, 10.0f);
            yield return new WaitForSeconds(_moveInterval);
        }
    }

    private IEnumerator MoveTo(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        float rotateTime = 1.0f;

        Quaternion startRotation = transform.rotation;

        while (elapsedTime < rotateTime)
        {
            float t = elapsedTime / rotateTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private IEnumerator CheckCollision(Quaternion targetRotation, System.Action<bool> onComplete)
    {
        BoxCollider originalCollider = GetComponent<BoxCollider>();

        originalCollider.enabled = false;
        BoxCollider tempCollider = gameObject.AddComponent<BoxCollider>();
        tempCollider.size = originalCollider.size;
        tempCollider.isTrigger = true; 

        bool collisionDetected = false;

        yield return StartCoroutine(MoveTempObject(tempCollider, targetRotation, originalCollider, (collision) =>
        {
            collisionDetected = collision;
        }));

        onComplete(collisionDetected);

        Destroy(tempCollider);
        originalCollider.enabled = true;
    }

    private IEnumerator MoveTempObject(BoxCollider tempCollider, Quaternion targetRotation, BoxCollider originalCollider, System.Action<bool> onComplete)
    {
        float elapsedTime = 0f;
        float rotateTime = 1.0f;

        Quaternion startRotation = tempCollider.transform.rotation;

        bool collisionDetected = false;

        while (elapsedTime < rotateTime)
        {
            float t = elapsedTime / rotateTime;

            tempCollider.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
 
            Collider[] hitColliders = Physics.OverlapBox(tempCollider.bounds.center, tempCollider.size / 2, tempCollider.transform.rotation, _obstacleMask);
            foreach (Collider collider in hitColliders)
            {
                if (collider != tempCollider)
                {
                    collisionDetected = true;
                    break;
                }
            }
            if (collisionDetected)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        onComplete(collisionDetected);
    }

}
