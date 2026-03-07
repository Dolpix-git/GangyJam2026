namespace CardGame.Abilities
{
    public static class ActionEnumNames
    {
        public static string Of(TargetSlot slot)
        {
            return slot switch
            {
                TargetSlot.Self => "self",
                TargetSlot.FriendlyLeft => "the friendly to the left",
                TargetSlot.FriendlyRight => "the friendly to the right",
                TargetSlot.FriendlyBoth => "both friendly flanks",
                TargetSlot.EnemyOpposing => "the opposing enemy",
                TargetSlot.EnemyLeft => "the enemy to the left",
                TargetSlot.EnemyRight => "the enemy to the right",
                TargetSlot.EnemyBoth => "both enemy flanks",
                TargetSlot.AllFriendly => "all friendlies",
                TargetSlot.AllEnemy => "all enemies",
                TargetSlot.All => "all units",
                _ => slot.ToString()
            };
        }

        public static string Of(ShiftTarget target)
        {
            return target switch
            {
                ShiftTarget.EnemyLeft => "enemy board left",
                ShiftTarget.EnemyRight => "enemy board right",
                ShiftTarget.FriendlyLeft => "friendly board left",
                ShiftTarget.FriendlyRight => "friendly board right",
                _ => target.ToString()
            };
        }
    }
}