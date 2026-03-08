using System;
using System.Collections;
using UnityEngine;

namespace CardGame.UI.Battle
{
    public class AttackAnimation : ICardAnimation
    {
        private readonly RectTransform _caster;
        private readonly float _hitDuration;
        private readonly float _lungeDistance;
        private readonly float _lungeDuration;
        private readonly float _returnDuration;
        private readonly int _shakeCount;
        private readonly float _shakeStrength;
        private readonly RectTransform[] _targets;

        public AttackAnimation(
            RectTransform caster,
            RectTransform[] targets,
            float lungeDuration,
            float returnDuration,
            float lungeDistance,
            float hitDuration,
            float shakeStrength,
            int shakeCount)
        {
            _caster = caster;
            _targets = targets;
            _lungeDuration = lungeDuration;
            _returnDuration = returnDuration;
            _lungeDistance = lungeDistance;
            _hitDuration = hitDuration;
            _shakeStrength = shakeStrength;
            _shakeCount = shakeCount;
        }

        public IEnumerator Play(Action onDone)
        {
            var isSelfHit = IsSelfHit();
            var restPos = _caster != null ? _caster.anchoredPosition : Vector2.zero;

            if (!isSelfHit && _caster != null && _targets.Length > 0)
            {
                var lungeDir = ComputeLungeDirection();
                var lungedPos = restPos + lungeDir * _lungeDistance;
                yield return MoveTo(_caster, restPos, lungedPos, _lungeDuration);
            }

            var done = 0;
            var total = _targets.Length + (_caster != null && !isSelfHit ? 1 : 0);

            foreach (var target in _targets)
            {
                var anim = new HitFlashAnimation(target, _hitDuration, _shakeStrength, _shakeCount);
                CardAnimationRunner.Instance.StartCoroutine(anim.Play(() => done++));
            }

            if (_caster != null && !isSelfHit)
            {
                CardAnimationRunner.Instance.StartCoroutine(
                    MoveTo(_caster, _caster.anchoredPosition, restPos, _returnDuration, () => done++)
                );
            }

            yield return new WaitUntil(() => done >= total);
            onDone();
        }

        private bool IsSelfHit()
        {
            if (_caster == null || _targets.Length != 1)
            {
                return false;
            }

            return _targets[0] == _caster;
        }

        private Vector2 ComputeLungeDirection()
        {
            var sum = Vector2.zero;
            foreach (var t in _targets)
            {
                sum += (Vector2)t.position;
            }

            var avg = sum / _targets.Length;
            var dir = avg - (Vector2)_caster.position;
            return dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up;
        }

        private static IEnumerator MoveTo(
            RectTransform rect,
            Vector2 from,
            Vector2 to,
            float duration,
            Action onDone = null)
        {
            var elapsed = 0f;

            while (elapsed < duration)
            {
                rect.anchoredPosition = Vector2.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rect.anchoredPosition = to;
            onDone?.Invoke();
        }
    }
}