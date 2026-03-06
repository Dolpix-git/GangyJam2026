using System.Collections.Generic;

namespace CardGame.Card.AbilityConfig
{
    [System.Serializable]
    public class AbilityConfig
    {
        public string targetingClass;
        public List<AbilityActionConfig> actions;
    }
}