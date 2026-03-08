using System;
using System.Collections;
using UnityEngine;

namespace CardGame.UI.Battle
{
    public interface ICardAnimation
    {
        IEnumerator Play(Action onDone);
    }
}
