using UnityEngine;

namespace CardGame.Data
{
    public class CardMode : MonoBehaviour
    {
        public int SelectedAbilityIndex { get; set; } = -1;
        public string TargetingData { get; set; }

        public bool HasSelection => SelectedAbilityIndex >= 0;

        public void Clear()
        {
            SelectedAbilityIndex = -1;
            TargetingData = null;
        }
    }
}