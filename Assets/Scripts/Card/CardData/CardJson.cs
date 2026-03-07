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
        public System.Collections.Generic.List<CardGame.Buffs.CardTag> Tags = new();
    }
}