using System.Collections.Generic;
using CardGame.Abilities;

namespace CardGame.Card.CardData
{
    public class AbilityJson
    {
        public string Name;
        public int MaxPp;
        public List<ActionStepJson> Steps = new();
    }

    public class ActionStepJson
    {
        public IAction Action;
    }
}
