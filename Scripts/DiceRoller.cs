using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Dice))]
public class DiceRoller : Dice, IPointerClickHandler
{
    public UnityAction RollEnded; //������� ��� �������� ��������� ������ � ���������
    public UnityAction RollStarted;

    private Vector3 _rotationSpeed; //���� � ����������� ������
    private Vector3 _deceleration = new Vector3(100.0f, 100.0f, 100.0f);  //����������� ��������� ��������
    private float _rollingForce = 620f; //������������ ���� ������
    private float _stopThreshold = 0.1f; //������������� �������� �������� ��� ������� ������� ���������� � ������ �������� �������
    private Quaternion _targetRotation;  // ���������� ���� ������������ ������ �� ������ �������
    private bool _aligningToSide; //����� �� ����� �� �������
    private Coroutine _rollDice; //�������� ������
    private bool _isRollResultPrinting = false;  //������������� ����, ���������� ����� � ����������

    // ����� ��� ������ ������ ������ �� ����
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsRolling && !_isRollResultPrinting) // �������� �� ��, �� �������� �� ����� � �� ���������� �� ��������� ����������� ������
        {
            RollStarted?.Invoke(); //����� ������� � ��� ��� ������ �������

            if (_rollDice != null) 
                StopCoroutine(_rollDice);

            _rollDice = StartCoroutine(RollDice());
        }
    }

    private IEnumerator RollDice() //�������� ������
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

    private void StartRoll() //����� ���������� � ������ ������ + ��������� ����� ���� � ����������� ������
    {
        IsRolling = true;
        _aligningToSide = false;
        _rotationSpeed = new Vector3(
            Random.Range(-_rollingForce, _rollingForce),
            Random.Range(-_rollingForce, _rollingForce),
            Random.Range(-_rollingForce, _rollingForce));
    }
   
    private void PerformRotation() //����� �������� ������ � ����������� ����������� � ������� ���������
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime);

        _rotationSpeed = Vector3.MoveTowards(_rotationSpeed, Vector3.zero, _deceleration.magnitude * Time.deltaTime);

        if (_rotationSpeed.magnitude <= _stopThreshold && !_aligningToSide) //������� � ������� � ��������� ������� ������ � ������� ��� ����������� ������ ���������
        {
            _targetRotation = GetClosestSide();
            _aligningToSide = true;
        }
    }

    private void AlignToSide() //������������ ������ �� ��������� ������� �� ����� ��� �� ����� �����������
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 2.0f);

        if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
        {
            transform.rotation = _targetRotation; 
            IsRolling = false; 
            _aligningToSide = false;
            RollEnded?.Invoke(); //����� ���������� �� ��������� ������ ��� �������� � ���������
        }
    }

    private Quaternion GetClosestSide() //����� ����������� ��������� ������� �� ����� ��� ����� ����������� �����������
    {
        Quaternion currentRotation = transform.rotation;
        float minDistance = float.MaxValue;
        Quaternion closestSide = Quaternion.identity;

        foreach (Vector3 side in DiceSides.Values) //������� ���� ������ ������ ��� ������ ���������
        {
            Quaternion targetRotation = Quaternion.Euler(side);
            float distance = Quaternion.Angle(currentRotation, targetRotation);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestSide = targetRotation;
                DiceSide = side; //������������ �������� ������� ����, ����� ����� ������� �� �������� � �������� ����������
            }
        }

        return closestSide;
    }

    public void SetResultPrintingStatus(bool isPrinting) //����� ��� �������� �� ���������� ���������� � ���, ����������� �� ��� ������ 
    {
        _isRollResultPrinting = isPrinting;
    }
}

