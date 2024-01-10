using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModificatorName : MonoBehaviour
{
    [SerializeField] private string _modificatorName;
    [SerializeField] private TMP_Text _modificatorView;

    void Start()
    {
        _modificatorView.text = _modificatorName;
    }
}
