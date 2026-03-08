using System;
using System.Collections;
using UnityEngine;

namespace CardGame.UI.Battle
{
    public class CardAnimator : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private float _lungeDuration = 0.12f;

        [SerializeField] private float _returnDuration = 0.18f;
        [SerializeField] private float _lungeDistance = 60f;
        [SerializeField] private float _hitDuration = 0.18f;
        [SerializeField] private float _shakeStrength = 14f;
        [SerializeField] private int _shakeCount = 6;

        [Header("Pacing")]
        [SerializeField] private float _betweenAttackPause = 0.45f;

        public void PlayAttack(GameObject caster, GameObject[] targets, Action onDone)
        {
            var casterRect = GetRect(caster);
            var targetRects = ToRects(targets);

            var animation = new AttackAnimation(
                casterRect,
                targetRects,
                _lungeDuration,
                _returnDuration,
                _lungeDistance,
                _hitDuration,
                _shakeStrength,
                _shakeCount);

            CardAnimationRunner.Instance.Play(animation, onDone);
        }

        public void PlayBetweenAttackPause(Action onDone)
        {
            StartCoroutine(PauseRoutine(onDone));
        }

        private static RectTransform GetRect(GameObject cardData)
        {
            if (cardData == null)
            {
                return null;
            }

            var ui = CardUIManager.Instance?.GetCardUI(cardData);
            return ui != null ? ui.GetComponent<RectTransform>() : null;
        }

        private static RectTransform[] ToRects(GameObject[] cards)
        {
            var rects = new RectTransform[cards.Length];
            for (var i = 0; i < cards.Length; i++)
            {
                rects[i] = GetRect(cards[i]);
            }

            return rects;
        }

        private IEnumerator PauseRoutine(Action onDone)
        {
            yield return new WaitForSeconds(_betweenAttackPause);
            onDone();
        }
    }
}