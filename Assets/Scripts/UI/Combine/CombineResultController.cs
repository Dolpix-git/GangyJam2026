using System.Collections;
using CardGame.Card;
using CardGame.StateMachine.Explore;
using CardGame.StateMachine.Explore.States;
using UI.ModelView.Models;
using UnityEngine;

namespace CardGame.UI.Combine
{
    public class CombineResultController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ExploreStateData _stateData;

        [SerializeField] private ModelViewCard _cardUIPrefab;

        [Header("Anchor Positions (UI RectTransforms)")]
        [SerializeField] private RectTransform _anchorLeft;

        [SerializeField] private RectTransform _anchorRight;
        [SerializeField] private RectTransform _anchorCentre;

        [Header("Animation")]
        [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.EaseInOut(0f, 100f, 1f, 800f);

        [SerializeField] private float _slideDuration = 1.5f;
        [SerializeField] private float _mergeDistance = 10f;

        [Header("UI")]
        [SerializeField] private GameObject _goToBattleButton;

        private GameObject _dataA;
        private GameObject _dataB;
        private GameObject _dataResult;

        private ModelViewCard _uiA;
        private ModelViewCard _uiB;
        private ModelViewCard _uiResult;

        public void Show(GameObject parentA, GameObject parentB, string combinedId)
        {
            _goToBattleButton.SetActive(false);

            _dataA = parentA;
            _dataB = parentB;
            _dataA.SetActive(false);
            _dataB.SetActive(false);

            _uiA = SpawnCardUI(_dataA, _anchorLeft);
            _uiB = SpawnCardUI(_dataB, _anchorRight);

            _dataResult = CardFactory.Instance.CreateCard(combinedId);
            _dataResult.SetActive(false);

            StartCoroutine(RunMergeSequence());
        }

        public void GoToBattle()
        {
            Cleanup();
            _stateData.GoToState(new ExploreFightState());
        }

        private ModelViewCard SpawnCardUI(GameObject dataCard, RectTransform anchor)
        {
            var ui = Instantiate(_cardUIPrefab, transform);
            ui.GetComponent<RectTransform>().anchoredPosition = anchor.anchoredPosition;
            ui.SetModel(dataCard);
            return ui;
        }

        private IEnumerator RunMergeSequence()
        {
            var target = _anchorCentre.anchoredPosition;
            var rectA = _uiA.GetComponent<RectTransform>();
            var rectB = _uiB.GetComponent<RectTransform>();

            var elapsed = 0f;
            while (Vector2.Distance(rectA.anchoredPosition, target) > _mergeDistance)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / _slideDuration);
                var step = _speedCurve.Evaluate(t) * Time.deltaTime;
                rectA.anchoredPosition = Vector2.MoveTowards(rectA.anchoredPosition, target, step);
                rectB.anchoredPosition = Vector2.MoveTowards(rectB.anchoredPosition, target, step);
                yield return null;
            }

            Destroy(_uiA.gameObject);
            Destroy(_uiB.gameObject);
            Destroy(_dataA);
            Destroy(_dataB);
            _uiA = _uiB = null;
            _dataA = _dataB = null;

            _dataResult.SetActive(false);
            _uiResult = SpawnCardUI(_dataResult, _anchorCentre);

            _goToBattleButton.SetActive(true);
        }

        private void Cleanup()
        {
            if (_uiA != null)
            {
                Destroy(_uiA.gameObject);
            }

            if (_uiB != null)
            {
                Destroy(_uiB.gameObject);
            }

            if (_uiResult != null)
            {
                Destroy(_uiResult.gameObject);
            }

            if (_dataA != null)
            {
                Destroy(_dataA);
            }

            if (_dataB != null)
            {
                Destroy(_dataB);
            }

            if (_dataResult != null)
            {
                Destroy(_dataResult);
            }
        }
    }
}