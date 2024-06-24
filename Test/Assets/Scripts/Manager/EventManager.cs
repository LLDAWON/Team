using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask; // ���̾� ����ũ ���� (Inspector���� ���� ����)

    private Sprite _hitCursorSprite;
    private Sprite _defaultCursorSprite;
    private List<int> _preEvents = new();
    private EventData _eventData; // �̺�Ʈ ������ ����
    private RaycastHit hit; // ����ĳ��Ʈ ��Ʈ ���� ����

    public int CurKey
    {
        get { return _eventData.Key; } // ���� �̺�Ʈ Ű ��ȯ
    }

    public void SetKey(int key)
    {
        _eventData.Key = key; // �̺�Ʈ Ű ����
    }


    private void Awake()
    {
        // Ŀ�� ��������Ʈ �̸� �ε�
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

        Vector3 center = new Vector3(UnityEngine.Screen.width * 0.5f, UnityEngine.Screen.height * 0.5f, 0);
        Ray dir = Camera.main.ScreenPointToRay(center);

        if (Physics.Raycast(dir, out hit, 2.0f, _layerMask))
        {
            _eventData = DataManager.Instance.GetEventData(hit.collider.tag); // ��Ʈ�� ��ü�� �±׷� �̺�Ʈ ������ ��������

            if (_eventData.EventTag == "None")
                return; // �̺�Ʈ �±װ� "None"�̸� ��ȯ

            if (CheckPreEvent())
            {
                UIManager.Instance.GetCursor.GetComponent<Image>().sprite = _hitCursorSprite; // Ŀ�� �̹��� ����

                if (_eventData.Type == 2)
                {
                    UIManager.Instance.SetText(17); // UI �ؽ�Ʈ ����
                    UIManager.Instance.KeySlider.gameObject.SetActive(true); // Ű �����̴� Ȱ��ȭ
                    GameObject obj = hit.collider.gameObject;

                    UIManager.Instance.KeySlider.OnPressKey(obj); // Ű �����̴��� ��ü ����

                    return;
                }

                UIManager.Instance.SetText(5); // UI �ؽ�Ʈ ����

                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnKeyDown(); // F Ű�� ������ OnKeyDown ȣ��
                    return;
                }
            }
        }
        else
        {
            UIManager.Instance.KeySlider.gameObject.SetActive(false); // Ű �����̴� ��Ȱ��ȭ
            UIManager.Instance.GetCursor.GetComponent<Image>().sprite = _defaultCursorSprite; // �⺻ Ŀ�� �̹����� ����
        }

        Debug.DrawRay(dir.origin, dir.direction*2.0f, Color.red, 2.0f); // ����� ���� �׸���

    }

    private void OnKeyDown()
    {
        SendText();

        if (_eventData.Type == 1)
        {
            hit.collider.GetComponent<ObjectAnimation>().PlayAnimation();
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
            ItemManager.Instance.GetItem(_eventData.GetItemKey); // ������ ȹ��
            ConditionText(_eventData.GetItemKey); // ���ǿ� ���� �ؽ�Ʈ ����
        }

        UIManager.Instance.SetText(_eventData.TextDataKey); // UI �ؽ�Ʈ ����

        _preEvents.Add(_eventData.Key); // ���� �̺�Ʈ Ű ��Ͽ� �߰�
    }

    private void ConditionText(int key)
    {
        ItemData data = DataManager.Instance.GetItemData(key); // ������ ������ ��������

        if (data.Type == 4)
        {
            UIManager.Instance.SetText(_eventData.TextDataKey - 1); // Ư�� ���ǿ� ���� �ؽ�Ʈ ����
        }
    }

    public void CallEvent(string key)
    {
        _eventData = DataManager.Instance.GetEventData(key); // �̺�Ʈ ������ ��������

        SendText(); // �ؽ�Ʈ ����
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("4ChangeScene"))
        {
            SoundManager.Instance.Init();
            SceneManager.LoadScene(1); // �� ����
            UIManager.Instance.CandleUI.gameObject.SetActive(true); // UI Ȱ��ȭ
            UIManager.Instance.StartCoroutine("Candle"); // �ڷ�ƾ ����
        }
        if (collision.collider.CompareTag("3ChangeScene"))
        {
            SoundManager.Instance.Init();
            SceneManager.LoadScene(2); // �� ����
            UIManager.Instance.SetText(1); // UI �ؽ�Ʈ ����
        }
        if(collision.collider.CompareTag("EndingScene"))
        {
            SoundManager.Instance.Init();
            SceneManager.LoadScene(6);
            SoundManager.Instance.Stop2D("BallerinaBG");
        }
    }

}
