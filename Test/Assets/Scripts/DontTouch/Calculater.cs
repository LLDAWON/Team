using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Calculater : MonoBehaviour
{
    public List<Button> _hexBtns;
    public Button _plusButton;
    public Button _equalsButton;
    public TMP_Text _answerText;
    public TMP_Text _resultText;

    private List<string> _inputs = new List<string>();
    private string _answer = "";
    private string _correctAnswer = "AB";
    private Coroutine _resetCoroutine;
    private Canvas canvas;
   

    private void Start()
    {
        foreach (Button button in _hexBtns)
        {
            button.onClick.AddListener(() => OnHexButtonClick(button.GetComponentInChildren<TMP_Text>().text));
        }
        _plusButton.onClick.AddListener(OnPlusButtonClick);
        _equalsButton.onClick.AddListener(OnEqualsButtonClick);
        //DisableRandomButtons(2);
        DisableButtons("A");
        DisableButtons("B");
    }

    public void OnHexButtonClick(string hexValue)
    {
        _inputs.Add(hexValue);
        _answer += hexValue;
        _answerText.text = _answer;
    }

    public void OnPlusButtonClick()
    {
        if (_inputs.Count > 0 && _inputs[_inputs.Count - 1] != "+")
        {
            _inputs.Add("+");
            _answer += " + ";
            _answerText.text = _answer; 
        }
    }

    public void OnEqualsButtonClick()
    {
        string removeSpace = _answer.Replace(" ", "");

        if (_inputs.Count > 0 && _inputs[_inputs.Count - 1] == "+")
        {
            _resultText.text = "try agan";
            _resetCoroutine = StartCoroutine(Reset(3f));
            return;
        }

        string[] hexValues = removeSpace.Split(new string[] { "+" }, System.StringSplitOptions.RemoveEmptyEntries);
        int sum = 0;

        foreach (string hex in hexValues)
        {
            if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int hexValue))
            {
                sum += hexValue;
            }
            else
            {
                _resultText.text = "try agan";
                _resetCoroutine = StartCoroutine(Reset(3f));
                return;
            }
        }

        string sumHex = sum.ToString("X");
        if (sumHex == _correctAnswer)
        {
            _resultText.text = "True";
        }
        else
        {
            _resultText.text = "False";
        }

        _resetCoroutine = StartCoroutine(Reset(3f));
       
    }

    private IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);
        _answer = "";
        _inputs.Clear();
        _answerText.text = "";
        _resultText.text = "";
        ReturnAllButtons();
        // DisableRandomButtons(2);
        DisableButtons("A");
        DisableButtons("B");
    }

    private void DisableRandomButtons(int count)
    {
        List<Button> hexbtn = new List<Button>(_hexBtns);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, hexbtn.Count);
            Button button = hexbtn[randomIndex];
            button.gameObject.SetActive(false);
        }
    }

    private void ReturnAllButtons()
    {
        foreach (Button button in _hexBtns)
        {
            button.gameObject.SetActive(true);
        }
    }
    private void DisableButtons(string buttonName)
    {
        foreach (Button button in _hexBtns)
        {
            if (button.name == buttonName)
            {
                button.gameObject.SetActive(false);
                return; 
            }
        }
    }
}

