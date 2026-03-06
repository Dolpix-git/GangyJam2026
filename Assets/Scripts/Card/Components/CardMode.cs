using UnityEngine;

namespace CardGame.Data
{
    /// <summary>
    /// Stores the mode (ability) selected for this card during the current round.
    /// Reset each round before mode selection begins.
    /// </summary>
    public class CardMode : MonoBehaviour
    {
        public string SelectedAbilityId { get; set; }
        public string TargetingData { get; set; }

        public bool HasSelection => !string.IsNullOrEmpty(SelectedAbilityId);

        public void Clear()
        {
            SelectedAbilityId = null;
            TargetingData = null;
        }
    }
}
