using System.Collections.Generic;
using CardGame.Buffs;
using UnityEngine;

namespace CardGame.Data
{
    public class TagData : MonoBehaviour
    {
        private readonly HashSet<CardTag> _tags = new();

        public void Initialize(List<CardTag> tags)
        {
            _tags.Clear();
            foreach (var buffTag in tags)
            {
                _tags.Add(buffTag);
            }
        }

        public bool HasTag(CardTag buffTag)
        {
            return _tags.Contains(buffTag);
        }
    }
}