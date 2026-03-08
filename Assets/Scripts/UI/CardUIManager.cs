using System.Collections.Generic;
using CardGame.Patterns;
using UI.ModelView.Models;
using UnityEngine;

public class CardUIManager : MonoSingleton<CardUIManager>
{
    [SerializeField] private ModelViewCard _cardPrefab;
    [SerializeField] private float _scale;

    private readonly Dictionary<GameObject, ModelViewCard> _allCards = new();

    public void CreateCardUI(GameObject cardModel, Transform parent)
    {
        var modelViewCard = Instantiate(_cardPrefab, parent);
        modelViewCard.transform.localPosition = Vector3.zero;
        modelViewCard.transform.localScale = Vector3.one * _scale;
        modelViewCard.SetModel(cardModel);

        _allCards.Add(cardModel, modelViewCard);
    }

    public void ReParentCard(GameObject cardModel, Transform parent)
    {
        if (!_allCards.TryGetValue(cardModel, out var model))
        {
            CreateCardUI(cardModel, parent);
            return;
        }

        model.transform.SetParent(parent);
        model.transform.localPosition = Vector3.zero;
    }

    public ModelViewCard GetCardUI(GameObject cardModel)
    {
        _allCards.TryGetValue(cardModel, out var ui);
        return ui;
    }

    public void DestroyCard(GameObject cardModel)
    {
        if (cardModel == null)
        {
            return;
        }

        if (!_allCards.TryGetValue(cardModel, out var model))
        {
            return;
        }

        Destroy(model.gameObject);
    }
}