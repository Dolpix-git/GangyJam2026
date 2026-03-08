using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI.Battle
{
    public class HitFlashAnimation : ICardAnimation
    {
        private readonly float _duration;
        private readonly RectTransform _rect;
        private readonly int _shakeCount;
        private readonly float _shakeStrength;

        public HitFlashAnimation(RectTransform rect, float duration, float shakeStrength, int shakeCount)
        {
            _rect = rect;
            _duration = duration;
            _shakeStrength = shakeStrength;
            _shakeCount = shakeCount;
        }

        public IEnumerator Play(Action onDone)
        {
            if (_rect == null)
            {
                onDone();
                yield break;
            }

            var img = _rect.GetComponent<Image>();
            var origColor = img != null ? img.color : Color.white;
            var origPos = _rect.anchoredPosition;
            var elapsed = 0f;

            while (elapsed < _duration)
            {
                var t = elapsed / _duration;
                var shake = Mathf.Sin(t * Mathf.PI * _shakeCount) * _shakeStrength * (1f - t);
                _rect.anchoredPosition = origPos + new Vector2(shake, 0f);

                if (img != null)
                {
                    img.color = Color.Lerp(Color.red, origColor, t);
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            _rect.anchoredPosition = origPos;
            if (img != null)
            {
                img.color = origColor;
            }

            onDone();
        }
    }
}