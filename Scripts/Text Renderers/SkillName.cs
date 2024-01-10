using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillName : MonoBehaviour
{
    [SerializeField] private string _skillName;
    [SerializeField] private TMP_Text _skillView;

    void Start()
    {
        _skillView.text = _skillName;
    }
}

