using System.Collections.Generic;
using CardGame.Patterns;

namespace CardGame.Run
{
    public class RunContext : MonoSingleton<RunContext>
    {
        public List<string> PlayerCardIds { get; private set; } = new();
        public List<string> EnemyCardIds { get; private set; } = new();
        public int RunCount { get; private set; }

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
            Save();
        }

        public void LoadRun(RunSaveData data)
        {
            PlayerCardIds = new List<string>(data.PlayerCardIds);
            EnemyCardIds  = new List<string>();
            RunCount = data.RunCount;
        }

        public void IncrementRunCount()
        {
            RunCount++;
        }

        public void Save()
        {
            SaveSystem.Save(new RunSaveData
            {
                PlayerCardIds = new List<string>(PlayerCardIds),
                RunCount = RunCount
            });
        }
    }
}