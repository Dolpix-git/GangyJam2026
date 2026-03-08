using System.Collections.Generic;
using CardGame.Patterns;

namespace CardGame.Run
{
    public class RunContext : MonoSingleton<RunContext>
    {
        public const int StartingCoins = 10;
        public const int CoinsPerWin = 5;
        public const int MaxDeckSize = 7;

        public bool IsDeckFull => PlayerCardIds.Count >= MaxDeckSize;

        public List<string> PlayerCardIds { get; private set; } = new();
        public List<string> EnemyCardIds { get; private set; } = new();
        public int RunCount { get; private set; }
        public int Coins { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void StartNewRun()
        {
            PlayerCardIds = new List<string>();
            EnemyCardIds = new List<string>();
            RunCount = 0;
            Coins = StartingCoins;
            Save();
        }

        public void LoadRun(RunSaveData data)
        {
            PlayerCardIds = new List<string>(data.PlayerCardIds);
            EnemyCardIds = new List<string>();
            RunCount = data.RunCount;
            Coins = data.Coins;
        }

        public void IncrementRunCount()
        {
            RunCount++;
        }

        public void AwardWinCoins()
        {
            Coins += CoinsPerWin;
        }

        public bool TrySpendCoins(int amount)
        {
            if (Coins < amount)
            {
                return false;
            }

            Coins -= amount;
            return true;
        }

        public void Save()
        {
            SaveSystem.Save(new RunSaveData
            {
                PlayerCardIds = new List<string>(PlayerCardIds),
                RunCount = RunCount,
                Coins = Coins
            });
        }
    }
}