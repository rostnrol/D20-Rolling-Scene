using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField] private ModificatorValue _modificatorValue; //������ ������ ���������� ������ ����� � �������� ������������
    [SerializeField] private DifficultyValue _difficultyValue; //������ ����� � ����������� ����� ��� ����������� ������ ��� ���������
    [SerializeField] private DiceRoller _diceRoller; //������ ��������� ���� � ������ � ���, ����� ������� ����������� �����
    [SerializeField] private TMP_Text _resultView;

    private Coroutine _resultPrinter;
    private int _result;
    private string _resultText;

    private void OnEnable() //�������� �� ������� � ���, ���������� ��� ������� ������
    {
        _diceRoller.RollEnded += RenderResult;
        _diceRoller.RollStarted += ClearResult;
    }

    private void OnDisable() //������� �� �������
    {
        _diceRoller.RollEnded -= RenderResult;
        _diceRoller.RollStarted -= ClearResult;
    }

    private void ClearResult() //����� ������� ������ ����������
    {
        _resultView.text = "";
    }

    private int ReadDiceResult() //����� ��������� �������� ������� �� ������� ����������� �����
    {
        foreach (var side in _diceRoller.DiceSides)
        {
            if (side.Value == _diceRoller.CurrentDiceSide)
            {
                return side.Key;
            }
        }
        return 0;
    }

    private void DefineResult() //����������� ����������
    {
        _result = ReadDiceResult() + _modificatorValue.ModificatorNumber; //���������� � ���������� ������ �����������
        _resultText = "RESULT: " + _result.ToString();

        if (_result >= _difficultyValue.difficultyValue)
            _resultText += " SUCCESS";
        else
            _resultText += " FAILURE";
    }

    private void RenderResult() //������ ����������
    {
        if (_resultPrinter != null)
            StopCoroutine(_resultPrinter);

        _diceRoller.SetResultPrintingStatus(true); //���������� DiceRoller � ��� ��� ��������� ��������� � ����� ����� �������

        DefineResult();
        _resultPrinter = StartCoroutine(ResultPrinter(_resultText));
    }

    IEnumerator ResultPrinter(string text) //�������� ������ ����������
    {
        var delay = new WaitForSeconds(0.1f); //���������� ���������� �������� ����� ������ ��� �� ��������� ����� ������ WaitForSeconds

        foreach (char symbol in text)
        {
            _resultView.text += symbol;
            yield return delay;
        }

        _diceRoller.SetResultPrintingStatus(false); //���������� DiceRoller � ��� ��� ������� ���� ������
    }
}
