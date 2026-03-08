using System.Collections;
using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModelView.Views
{
    public class ViewCardHealth : ViewBase<ModelViewCard, GameObject>
    {
        [Header("Health Bar")]
        [SerializeField] private Image _healthBar;

        [SerializeField] private Image _ghostBar;
        [SerializeField] private float _healthAnimDuration = 0.25f;
        [SerializeField] private float _ghostDelay = 0.3f;
        [SerializeField] private float _ghostAnimDuration = 0.4f;

        [Header("Health Text")]
        [SerializeField] private TMP_Text _healthText;

        [Header("Shield Bar")]
        [SerializeField] private Image _shieldBar;

        [SerializeField] private GameObject _shieldRoot;
        [SerializeField] private float _shieldAnimDuration = 0.2f;
        private Coroutine _ghostCoroutine;

        private BuffData _buffs;
        private HealthData _health;

        private Coroutine _healthCoroutine;
        private Coroutine _shieldCoroutine;

        protected override void OnDisable()
        {
            base.OnDisable();
            Unsubscribe();
        }

        protected override void HandleModelChanged(GameObject model)
        {
            Unsubscribe();

            if (model == null) return;

            _health = model.GetComponent<HealthData>();
            _buffs  = model.GetComponent<BuffData>();

            if (_health != null) _health.OnHealthChanged += OnHealthChanged;
            if (_buffs  != null) _buffs.OnBuffsChanged   += OnBuffsChanged;

            Refresh();
        }

        private void Unsubscribe()
        {
            if (_health != null)
            {
                _health.OnHealthChanged -= OnHealthChanged;
            }

            if (_buffs != null)
            {
                _buffs.OnBuffsChanged -= OnBuffsChanged;
            }
        }

        private void OnHealthChanged(int current, int max)
        {
            var targetFill = max > 0 ? (float)current / max : 0f;
            AnimateHealth(targetFill);
            AnimateGhost(targetFill);
            RefreshShield();
            UpdateHealthText(current, max);
        }

        private void OnBuffsChanged()
        {
            RefreshShield();
        }

        private void Refresh()
        {
            if (_health == null)
            {
                return;
            }

            var fill = _health.MaxHealth > 0
                ? (float)_health.CurrentHealth / _health.MaxHealth
                : 0f;

            if (_healthBar != null)
            {
                _healthBar.fillAmount = fill;
            }

            if (_ghostBar != null)
            {
                _ghostBar.fillAmount = fill;
            }

            UpdateHealthText(_health.CurrentHealth, _health.MaxHealth);
            RefreshShield();
        }

        private void RefreshShield()
        {
            if (_shieldBar == null)
            {
                return;
            }

            var shield = _buffs?.TotalShield ?? 0;
            var maxHealth = _health?.MaxHealth ?? 1;

            var shieldFill = maxHealth > 0 ? Mathf.Clamp01((float)shield / maxHealth) : 0f;

            if (_shieldRoot != null)
            {
                _shieldRoot.SetActive(shield > 0);
            }

            AnimateShield(shieldFill);
        }

        private void UpdateHealthText(int current, int max)
        {
            if (_healthText != null)
                _healthText.text = $"{current}/{max}";
        }

        private void AnimateHealth(float targetFill)
        {
            if (_healthBar == null) return;
            if (_healthCoroutine != null) StopCoroutine(_healthCoroutine);
            _healthCoroutine = StartCoroutine(AnimateFill(_healthBar, _healthBar.fillAmount, targetFill, _healthAnimDuration));
        }

        private void AnimateGhost(float targetFill)
        {
            if (_ghostBar == null) return;
            if (_ghostCoroutine != null) StopCoroutine(_ghostCoroutine);
            _ghostCoroutine = StartCoroutine(GhostRoutine(_ghostBar.fillAmount, targetFill));
        }

        private void AnimateShield(float targetFill)
        {
            if (_shieldBar == null) return;
            if (_shieldCoroutine != null) StopCoroutine(_shieldCoroutine);
            _shieldCoroutine = StartCoroutine(AnimateFill(_shieldBar, _shieldBar.fillAmount, targetFill, _shieldAnimDuration));
        }

        private static IEnumerator AnimateFill(Image bar, float start, float target, float duration)
        {
            var elapsed = 0f;

            while (elapsed < duration)
            {
                bar.fillAmount = Mathf.Lerp(start, target, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            bar.fillAmount = target;
        }

        private IEnumerator GhostRoutine(float start, float targetFill)
        {
            if (targetFill >= start)
            {
                _ghostBar.fillAmount = targetFill;
                yield break;
            }

            yield return new WaitForSeconds(_ghostDelay);

            var elapsed = 0f;
            while (elapsed < _ghostAnimDuration)
            {
                _ghostBar.fillAmount = Mathf.Lerp(start, targetFill, elapsed / _ghostAnimDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _ghostBar.fillAmount = targetFill;
        }
    }
}