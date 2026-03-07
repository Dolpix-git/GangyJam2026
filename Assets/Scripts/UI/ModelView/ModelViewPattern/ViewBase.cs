using UnityEngine;

namespace CardGame.UI.ModelViewPattern
{
    public abstract class ViewBase<TModelView, TModel> : MonoBehaviour
        where TModelView : ModelViewBase<TModel>
    {
        [SerializeField] protected TModelView modelView;

        protected TModel Model => modelView != null ? modelView.Model : default;

        protected virtual void Awake()
        {
            if (modelView == null)
            {
                Debug.LogError("Model is not set");
                return;
            }
        }

        protected virtual void OnEnable()
        {
            if (modelView != null)
            {
                modelView.OnModelChanged += HandleModelChanged;
                HandleModelChanged(modelView.Model);
            }
        }

        protected virtual void OnDisable()
        {
            if (modelView != null)
            {
                modelView.OnModelChanged -= HandleModelChanged;
            }
        }

        protected abstract void HandleModelChanged(TModel model);
    }
}