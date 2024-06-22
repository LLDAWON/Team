using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask; // 레이어 마스크 설정 (Inspector에서 설정 가능)

    private Sprite _hitCursorSprite;
    private Sprite _defaultCursorSprite;
    private List<int> _preEvents = new();
    private EventData _eventData; // 이벤트 데이터 저장
    private RaycastHit hit; // 레이캐스트 히트 정보 저장

    public int CurKey
    {
        get { return _eventData.Key; } // 현재 이벤트 키 반환
    }

    public void SetKey(int key)
    {
        _eventData.Key = key; // 이벤트 키 설정
    }


    private void Awake()
    {
        // 커서 스프라이트 미리 로드
        _hitCursorSprite = Resources.Load<Sprite>("Textures/UI/HitCursor");
        _defaultCursorSprite = Resources.Load<Sprite>("Textures/UI/Cursur");
    }



    private void Update()
    {
        CheckCollision();

    }

    private void CheckCollision()
    {
        if (Camera.main == null) return;
        Transform childTransform = transform.GetChild(0).transform.GetChild(0).transform;
        Vector3 pos = childTransform.position; // 자식 객체의 위치 가져오기
        Vector3 origin = Camera.main.transform.position; // 레이의 시작점: 카메라의 위치
        Vector3 dir = pos - childTransform.parent.position; // 레이의 방향 계산

        Debug.DrawRay(origin, dir * 2.0f, Color.red, 2.0f); // 디버그 레이 그리기

        if (Physics.Raycast(origin, dir, out hit, 2.0f, _layerMask))
        {
            _eventData = DataManager.Instance.GetEventData(hit.collider.tag); // 히트된 객체의 태그로 이벤트 데이터 가져오기

            if (_eventData.EventTag == "None")
                return; // 이벤트 태그가 "None"이면 반환

            if (CheckPreEvent())
            {
                UIManager.Instance.GetCursor.GetComponent<Image>().sprite = _hitCursorSprite; // 커서 이미지 변경

                if (_eventData.Type == 2)
                {
                    UIManager.Instance.SetText(17); // UI 텍스트 설정
                    UIManager.Instance.KeySlider.gameObject.SetActive(true); // 키 슬라이더 활성화
                    GameObject obj = hit.collider.gameObject;

                    UIManager.Instance.KeySlider.OnPressKey(obj); // 키 슬라이더에 객체 전달

                    return;
                }

                UIManager.Instance.SetText(5); // UI 텍스트 설정

                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnKeyDown(); // F 키가 눌리면 OnKeyDown 호출
                    return;
                }
            }
        }
        else
        {
            UIManager.Instance.KeySlider.gameObject.SetActive(false); // 키 슬라이더 비활성화
            UIManager.Instance.GetCursor.GetComponent<Image>().sprite = _defaultCursorSprite; // 기본 커서 이미지로 변경
        }
    }

    private void OnKeyDown()
    {
        SendText();

        if (_eventData.Type == 1)
        {
            hit.collider.GetComponent<TestAnimation>().PlayAnimation();
            if (hit.collider.CompareTag("ClassDoor1"))
            {
                SoundManager.Instance.Play3D("Door", hit.transform.position, false);
                return;
            }

            SoundManager.Instance.Play3D(hit.collider.tag, hit.transform.position, false);
        }
        else if (_eventData.Type == 0)
        {
            if (_eventData.GetItemKey == 0)
                return;
            hit.collider.gameObject.SetActive(false);
        }
    }

    private bool CheckPreEvent()
    {
        if (_eventData.Condition > 0)
        {
            return _preEvents.Contains(_eventData.Condition);
        }
        else
        {
            return true;
        }
    }

    private void SendText()
    {
        if (_eventData.GetItemKey > 0)
        {
            ItemManager.Instance.GetItem(_eventData.GetItemKey); // 아이템 획득
            ConditionText(_eventData.GetItemKey); // 조건에 따른 텍스트 설정
        }

        UIManager.Instance.SetText(_eventData.TextDataKey); // UI 텍스트 설정

        _preEvents.Add(_eventData.Key); // 이전 이벤트 키 목록에 추가
    }

    private void ConditionText(int key)
    {
        ItemData data = DataManager.Instance.GetItemData(key); // 아이템 데이터 가져오기

        if (data.Type == 4)
        {
            UIManager.Instance.SetText(_eventData.TextDataKey - 1); // 특정 조건에 따른 텍스트 설정
        }
    }

    public void CallEvent(string key)
    {
        _eventData = DataManager.Instance.GetEventData(key); // 이벤트 데이터 가져오기

        SendText(); // 텍스트 전송
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("4ChangeScene"))
        {
            SceneManager.LoadScene(1); // 씬 변경
            UIManager.Instance.CandleUI.gameObject.SetActive(true); // UI 활성화
            UIManager.Instance.StartCoroutine("Candle"); // 코루틴 시작
        }
        if (collision.collider.CompareTag("3ChangeScene"))
        {
            SceneManager.LoadScene(2); // 씬 변경
            UIManager.Instance.SetText(1); // UI 텍스트 설정
        }
        if(collision.collider.CompareTag("EndingScene"))
        {
            SceneManager.LoadScene(6);
            SoundManager.Instance.Stop3D("BallerinaBG");
        }
    }
}
