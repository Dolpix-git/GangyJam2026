using System;
using System.Collections.Generic;
using CardGame.Buffs;
using CardGame.Data;

namespace CardGame.Card.CardData
{
    [Serializable]
    public class CardJson
    {
        public string CardId;
        public string CardName;
        public string Description;
        public int MaxHealth;
        public int Speed;
        public int StruggleDamage;
        public CardRarity Rarity;
        public int Cost;
        public List<CardTag> Tags = new();
    }
}