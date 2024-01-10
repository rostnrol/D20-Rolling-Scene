using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Dice))]
public class DiceRoller : Dice, IPointerClickHandler
{
    public UnityAction RollEnded; //события для передачи состояния броска в результат
    public UnityAction RollStarted;

    private Vector3 _rotationSpeed; //сила и направление броска
    private Vector3 _deceleration = new Vector3(100.0f, 100.0f, 100.0f);  //модификатор остановки вращения
    private float _rollingForce = 620f; //максимальная сила броска
    private float _stopThreshold = 0.1f; //идентификатор скорости вращения при котором следует переходить к поиску ближайшй стороны
    private Quaternion _targetRotation;  // координаты цели докручивания кубика до нужной стороны
    private bool _aligningToSide; //встал ли кубик на сторону
    private Coroutine _rollDice; //корутина броска
    private bool _isRollResultPrinting = false;  //идентификатор того, печатается текст в результате

    // метод для броска кубика кликом по нему
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsRolling && !_isRollResultPrinting) // проверка на то, не вертится ли кубик и не печатается ли результат предыдущего броска
        {
            RollStarted?.Invoke(); //вызов события о том что бросок начался

            if (_rollDice != null) 
                StopCoroutine(_rollDice);

            _rollDice = StartCoroutine(RollDice());
        }
    }

    private IEnumerator RollDice() //корутина броска
    {
        StartRoll();

        while (IsRolling)
        {
            PerformRotation();

            if (_aligningToSide)
            {
                AlignToSide();
            }

            yield return null;
        }
    }

    private void StartRoll() //метод оповещения о начале броска + рандомный выбор силы и направления броска
    {
        IsRolling = true;
        _aligningToSide = false;
        _rotationSpeed = new Vector3(
            Random.Range(-_rollingForce, _rollingForce),
            Random.Range(-_rollingForce, _rollingForce),
            Random.Range(-_rollingForce, _rollingForce));
    }
   
    private void PerformRotation() //метод вращение кубика с последующим замедлением и началом остановки
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime);

        _rotationSpeed = Vector3.MoveTowards(_rotationSpeed, Vector3.zero, _deceleration.magnitude * Time.deltaTime);

        if (_rotationSpeed.magnitude <= _stopThreshold && !_aligningToSide) //переход к подходу к ближайшей стороне кубика к моменту его практически полной остановки
        {
            _targetRotation = GetClosestSide();
            _aligningToSide = true;
        }
    }

    private void AlignToSide() //докручивание кубика до ближайшей стороны от точки где он почти остановился
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 2.0f);

        if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
        {
            transform.rotation = _targetRotation; 
            IsRolling = false; 
            _aligningToSide = false;
            RollEnded?.Invoke(); //вызов оповещения об окончании броска для передачи в результат
        }
    }

    private Quaternion GetClosestSide() //поиск координатов ближайшей стороны от точки где кубик практически остановился
    {
        Quaternion currentRotation = transform.rotation;
        float minDistance = float.MaxValue;
        Quaternion closestSide = Quaternion.identity;

        foreach (Vector3 side in DiceSides.Values) //перебор всех сторон кубика для поиска ближайшей
        {
            Quaternion targetRotation = Quaternion.Euler(side);
            float distance = Quaternion.Angle(currentRotation, targetRotation);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestSide = targetRotation;
                DiceSide = side; //присваивание итоговой стороны полю, чтобы потом считать ее значение в подсчете результата
            }
        }

        return closestSide;
    }

    public void SetResultPrintingStatus(bool isPrinting) //метод для передачи из результата информации о том, закончилась ли его печать 
    {
        _isRollResultPrinting = isPrinting;
    }
}

