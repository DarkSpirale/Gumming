using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public Gumming CurrentGumming { get; private set; }
    public Ability CurrentAbility { get; private set; }

    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private Transform abilitySlotsParents;
    [SerializeField] private GameObject abilitySlotPrefab;
    [SerializeField] private List<Ability> abilities = new List<Ability>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one instance of AbilityManager existing.");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        RefreshContent();
    }

    public void UseAbility()
    {
        Ability abilityToUse = abilities.Find(_ability => _ability.data.abilityName == CurrentAbility.data.abilityName);

        if (abilityToUse.numberOfUses == 0)
        {
            Debug.LogError("Remaining uses of ability is zero !");
            return;
        }

        abilityToUse.numberOfUses--;
        
        if (abilityToUse.numberOfUses == 0)
        {
            DeselectAbility();
        }

        RefreshContent();
    }

    public void AddAbility(Ability abilityToAdd)
    {
        abilities.Add(abilityToAdd);
        RefreshContent();
    }

    public void AddAbility(SO_AbilityData data, int amount)
    {
        Ability abilityToAdd = new Ability(data, amount);
        abilities.Add(abilityToAdd);
        RefreshContent();
    }

    public void SelectAbility(Ability abilityToSelect)
    {
        CurrentAbility = abilityToSelect;
    }

    public void DeselectAbility()
    {
        CurrentAbility = null;
    }

    public void SelectGumming(Gumming gummingToSelect)
    {
        CurrentGumming = gummingToSelect;
    }

    public void DeselectGumming()
    {
        CurrentGumming = null;
    }

    private void CreateAbilitySlots()
    {
        int currentNumberOfSlots = abilitySlotsParents.childCount;
        int numberOfSlotsToCreate = abilities.Count - currentNumberOfSlots;

        for (int i = currentNumberOfSlots; i < currentNumberOfSlots + numberOfSlotsToCreate; i++)
        {
            GameObject slot = Instantiate(abilitySlotPrefab, abilitySlotsParents.transform);
            slot.name = $"AbilitySlot{i}";
        }
    }

    private void RefreshContent()
    {
        if(abilitySlotsParents.childCount < abilities.Count)
        {
            CreateAbilitySlots();
        }

        for (int i = 0; i < abilities.Count; i++)
        {
            AbilitySlot currentSlot = abilitySlotsParents.GetChild(i).GetComponent<AbilitySlot>();

            if(currentSlot == null)
            {
                Debug.LogError("Mission ability slot");
                return;
            }

            currentSlot.ability = abilities[i];
            currentSlot.abilityIcon.sprite = abilities[i].data.icon;
            currentSlot.numbersOfUses.text = abilities[i].numberOfUses.ToString();
            currentSlot.button.interactable = abilities[i].numberOfUses > 0;
        }
    }

    
}
