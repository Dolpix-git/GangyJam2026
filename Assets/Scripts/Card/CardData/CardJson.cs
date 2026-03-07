using System.Collections.Generic;
using CardGame.Buffs;

namespace CardGame.Card.CardData
{
    [System.Serializable]
    public class CardJson
    {
        public string CardId;
        public string CardName;
        public string Description;
        public int MaxHealth;
        public int Speed;
        public int StruggleDamage;
        public List<CardTag> Tags = new();
    }
}