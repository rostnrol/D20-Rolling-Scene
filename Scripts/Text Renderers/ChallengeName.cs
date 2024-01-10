using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeName : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private TMP_Text _challengeName;

    void Start()
    {
        _challengeName.text = _name;
    }
}
