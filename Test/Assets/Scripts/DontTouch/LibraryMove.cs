using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryMove : MonoBehaviour
{
    private float moveInterval;
    private float moveDistance = 1.0f;
    private float rotateAngle = 90.0f; 
    public LayerMask obstacleMask; 

   

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
               
                newRotation *= Quaternion.Euler(0, rotateAngle, 0);
            }
            else
            {
                
                newRotation *= Quaternion.Euler(0, -rotateAngle, 0);
            }

           
            Vector3 moveDirection = newRotation * Vector3.forward;
            Vector3 targetPosition = transform.position + moveDirection;

           
            bool collisionDetected = false;
            yield return StartCoroutine(CheckCollision(targetPosition, newRotation, (result) => collisionDetected = result));

            if (!collisionDetected)
            {
               
                yield return StartCoroutine(MoveTo(targetPosition, newRotation));
               
            }
              moveInterval = Random.Range(5, 10.0f);
           // moveInterval = 3f;
            yield return new WaitForSeconds(moveInterval);
        }
    }

    private IEnumerator MoveTo(Vector3 targetPosition, Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        float moveTime = 1.0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;
           
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private IEnumerator CheckCollision(Vector3 targetPosition, Quaternion targetRotation, System.Action<bool> onComplete)
    {
       
        GameObject tempObject = new GameObject("TempCollider");
        tempObject.transform.position = transform.position;
        tempObject.transform.rotation = transform.rotation;
        BoxCollider tempCollider = tempObject.AddComponent<BoxCollider>();
        tempCollider.size = GetComponent<BoxCollider>().size;

        yield return StartCoroutine(MoveTempObject(tempObject, targetPosition, targetRotation, () =>
        {
            Collider[] hitColliders = Physics.OverlapBox(tempObject.transform.position, tempCollider.size / 2, tempObject.transform.rotation, obstacleMask);

            bool collisionDetected = false;
            foreach (Collider collider in hitColliders)
            {
                if (collider != GetComponent<Collider>())
                {
                    collisionDetected = true; 
                    break;
                }
            }

            onComplete(collisionDetected);
            Destroy(tempObject);
        }));
    }

    private IEnumerator MoveTempObject(GameObject tempObject, Vector3 targetPosition, Quaternion targetRotation, System.Action onComplete)
    {
        float elapsedTime = 0f;
        float moveTime = 1.0f; 

        Quaternion startRotation = tempObject.transform.rotation;

        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;
         
            tempObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempObject.transform.rotation = targetRotation;

        onComplete();
    }
  
}
