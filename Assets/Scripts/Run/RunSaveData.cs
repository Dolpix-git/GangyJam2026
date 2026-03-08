using System;
using System.Collections.Generic;

namespace CardGame.Run
{
    [Serializable]
    public class RunSaveData
    {
        public List<string> PlayerCardIds = new();
        public int RunCount;
    }
}