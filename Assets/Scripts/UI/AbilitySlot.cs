using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilitySlot : MonoBehaviour
{
    public Ability ability;
    public Image abilityIcon;
    public Text numbersOfUses;
    [HideInInspector] public Toggle button;
    [SerializeField] private Color colorWhenSelected;
    [SerializeField] private Color colorWhenDeselected;
    private AbilityManager abilityManager;

    private void Awake()
    {
        button = transform.GetComponent<Toggle>();
    }

    private void Start()
    {
        abilityManager = AbilityManager.instance;
    }

    private void Update()
    {
        if (abilityManager.CurrentAbility == ability)
        {
            SetButtonColors(colorWhenSelected, colorWhenSelected);
        }
        else
        {
            SetButtonColors(colorWhenDeselected, colorWhenDeselected);
        }
    }

    public void ClickOnSlot()
    {
        abilityManager.SelectAbility(ability);
    }

    private void SetButtonColors(Color normalColor, Color selectedColor)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = normalColor;
        cb.selectedColor = selectedColor;
        button.colors = cb;
    }
}
