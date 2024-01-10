using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour, IPointerEnterHandler
{
    public readonly Dictionary<int, Vector3> DiceSides = new Dictionary<int, Vector3>() //инициализация словаря с привязками нумерического значения к координатам каждой из его сторон 
    {
        { 1, new Vector3(5, 0, -180) },
        { 2, new Vector3(35, -168, -52) },
        { 3, new Vector3(-53, 47, 27) },
        { 4, new Vector3(43, -110, -70) },
        { 5, new Vector3(-57, -49, -25) },
        { 6, new Vector3(43, 108, 71) },
        { 7, new Vector3(-50, 5, -2) },
        { 8, new Vector3(30, -189, 55) },
        { 9, new Vector3(-39, -47, -145) },
        { 10, new Vector3(44, 124, -44) },
        { 11, new Vector3(-37, 45, 146) },
        { 12, new Vector3(36, -131, 33) },
        { 13, new Vector3(-30, 12, -126) },
        { 14, new Vector3(45, -180, -180) },
        { 15, new Vector3(-42, 71, -105) },
        { 16, new Vector3(54, -130, 154) },
        { 17, new Vector3(-44, -69, 109) },
        { 18, new Vector3(57, 130, -155) },
        { 19, new Vector3(-31, -10, 125) },
        { 20, new Vector3(-15, -180, 0) }
    };

    protected bool IsRolling = false; //защищенные переменные для использования только в дочернем классе DiceRoller
    protected Vector3 DiceSide;

    private Coroutine _blink;
    private bool _isBlinking = false;
    private Vector3 _originalScale;

    public Vector3 CurrentDiceSide => DiceSide; //свойство чтобы потом в подсчете результата найти значение стороны на которую встал кубик

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    //метод для мигания кубика при наведении курсора путем его кратковременного увеличения
    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (IsRolling && _isBlinking) // ограничение на то чтобы кубик не мигал в тот момент когда вертится или уже мигает.
        {
                StopCoroutine(_blink);
                transform.localScale = _originalScale;
                _isBlinking = false;
        }

        if (!_isBlinking) 
        {
            _blink = StartCoroutine(Blink());
        }
    }

    //корутина регулирующая скорость мигания
    private IEnumerator Blink() 
    {
        float blinkDuration = 0.3f;
        float blinkScale = 1.07f;
        _isBlinking = true;
        Vector3 targetScale = _originalScale * blinkScale;
        float currentTime = 0;

        while (currentTime < blinkDuration)
        {
            transform.localScale = Vector3.Lerp(_originalScale, targetScale, currentTime / blinkDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = _originalScale;
        _isBlinking = false;
    }
}




