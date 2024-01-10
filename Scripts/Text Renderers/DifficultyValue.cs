using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyValue : MonoBehaviour
{
    [SerializeField] private int _difficultyValue;
    [SerializeField] private TMP_Text _valueView;

    public int difficultyValue => _difficultyValue;

    void Start()
    {
        _valueView.text = _difficultyValue.ToString();
    }
}
