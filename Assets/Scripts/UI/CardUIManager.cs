using System.Collections.Generic;
using CardGame.Patterns;
using UI.ModelView.Models;
using UnityEngine;

public class CardUIManager : MonoSingleton<CardUIManager>
{
    [SerializeField] private ModelViewCard cardPrefab;

    private Dictionary<GameObject, ModelViewCard> AllCards = new();

    public void CreateCardUI(GameObject cardModel, Transform parent)
    {
        var modelViewCard = Instantiate(cardPrefab, parent);
        modelViewCard.transform.localPosition = Vector3.zero;
        modelViewCard.SetModel(cardModel);

        AllCards.Add(cardModel, modelViewCard);
    }

    public void ReParentCard(GameObject cardModel, Transform parent)
    {
        if (!AllCards.TryGetValue(cardModel, out var model))
        {
            CreateCardUI(cardModel, parent);
            return;
        }

        model.transform.SetParent(parent);
        model.transform.localPosition = Vector3.zero;
    }

    public void DestroyCard(GameObject cardModel)
    {
        if (!AllCards.TryGetValue(cardModel, out var model))
        {
            return;
        }

        Destroy(model);
    }
}