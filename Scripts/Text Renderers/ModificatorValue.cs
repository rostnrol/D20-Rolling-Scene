using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModificatorValue : MonoBehaviour
{
    [SerializeField] private int _modificatorValue;
    [SerializeField] private TMP_Text _valueView;

    public int ModificatorNumber => _modificatorValue;

    void Start()
    {
        _valueView.text = "+" + _modificatorValue.ToString();
    }
}
