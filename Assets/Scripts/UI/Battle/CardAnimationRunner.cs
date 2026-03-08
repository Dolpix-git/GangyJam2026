using System;
using CardGame.Patterns;

namespace CardGame.UI.Battle
{
    public class CardAnimationRunner : MonoSingleton<CardAnimationRunner>
    {
        public void Play(ICardAnimation animation, Action onDone)
        {
            StartCoroutine(animation.Play(onDone));
        }
    }
}
