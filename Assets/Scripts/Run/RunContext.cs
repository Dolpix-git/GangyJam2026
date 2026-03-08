using System.Collections.Generic;
using CardGame.Patterns;

namespace CardGame.Run
{
    public class RunContext : MonoSingleton<RunContext>
    {
        public List<string> PlayerCardIds { get; private set; } = new();
        public List<string> EnemyCardIds { get; private set; } = new();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void StartNewRun()
        {
            PlayerCardIds = new List<string> { "001_Concept", "002_Concept", "003_Concept" };
            EnemyCardIds = new List<string> { "001_Concept", "002_Concept", "003_Concept" };
            Save();
        }

        public void LoadRun(RunSaveData data)
        {
            PlayerCardIds = new List<string>(data.PlayerCardIds);
            EnemyCardIds = new List<string> { "001_Concept", "002_Concept", "003_Concept" };
        }

        public void Save()
        {
            SaveSystem.Save(new RunSaveData { PlayerCardIds = new List<string>(PlayerCardIds) });
        }
    }
}