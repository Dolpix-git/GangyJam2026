using System;
using UnityEngine;

namespace CardGame.UI.ModelViewPattern
{
    public abstract class ModelViewBase<TModel> : MonoBehaviour
    {
        public TModel Model { get; private set; }

        public event Action<TModel> OnModelChanged;

        public virtual void SetModel(TModel model)
        {
            Model = model;
            OnModelChanged?.Invoke(Model);
        }

        public virtual void ClearModel()
        {
            Model = default;
            OnModelChanged?.Invoke(Model);
        }
    }
}