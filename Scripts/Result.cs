using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField] private ModificatorValue _modificatorValue; //строка вывода результата должна знать о значении модификатора
    [SerializeField] private DifficultyValue _difficultyValue; //должна знать о необходимой сумме для определения успеха или проигрыша
    [SerializeField] private DiceRoller _diceRoller; //должна прочитать инфу с броска о том, какую сторону приземлился кубик
    [SerializeField] private TMP_Text _resultView;

    private Coroutine _resultPrinter;
    private int _result;
    private string _resultText;

    private void OnEnable() //подписка на события о том, завершился или начался бросок
    {
        _diceRoller.RollEnded += RenderResult;
        _diceRoller.RollStarted += ClearResult;
    }

    private void OnDisable() //отписка от событий
    {
        _diceRoller.RollEnded -= RenderResult;
        _diceRoller.RollStarted -= ClearResult;
    }

    private void ClearResult() //метод очистки строки результата
    {
        _resultView.text = "";
    }

    private int ReadDiceResult() //метод получения значения стороны на которую приземлился кубик
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

    private void DefineResult() //определение результата
    {
        _result = ReadDiceResult() + _modificatorValue.ModificatorNumber; //прибавляем к результату броска модификатор
        _resultText = "RESULT: " + _result.ToString();

        if (_result >= _difficultyValue.difficultyValue)
            _resultText += " SUCCESS";
        else
            _resultText += " FAILURE";
    }

    private void RenderResult() //печать результата
    {
        if (_resultPrinter != null)
            StopCoroutine(_resultPrinter);

        _diceRoller.SetResultPrintingStatus(true); //оповещение DiceRoller о том что результат напечатан и можно снова бросать

        DefineResult();
        _resultPrinter = StartCoroutine(ResultPrinter(_resultText));
    }

    IEnumerator ResultPrinter(string text) //корутина печати результата
    {
        var delay = new WaitForSeconds(0.1f); //сохранение переменной ожидания чтобы каждый раз не создавать новый объект WaitForSeconds

        foreach (char symbol in text)
        {
            _resultView.text += symbol;
            yield return delay;
        }

        _diceRoller.SetResultPrintingStatus(false); //оповещение DiceRoller о том что бросать пока нельзя
    }
}
