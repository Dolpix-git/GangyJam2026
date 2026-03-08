using System;
using UnityEngine;

namespace CardGame.Data
{
    public class CardMode : MonoBehaviour
    {
        public int SelectedAbilityIndex { get; private set; } = -1;
        public string TargetingData { get; set; }

        public bool HasSelection => SelectedAbilityIndex >= 0;

        public event Action OnAbilitySelect;

        public void SelectAbility(int abilityIndex)
        {
            SelectedAbilityIndex = abilityIndex;
            OnAbilitySelect?.Invoke();
        }

        public void Clear()
        {
            SelectedAbilityIndex = -1;
            TargetingData = null;
            OnAbilitySelect?.Invoke();
        }
    }
}