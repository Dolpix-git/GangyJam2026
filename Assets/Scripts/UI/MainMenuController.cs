using CardGame.Run;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;

        private void Start()
        {
            _continueButton.SetActive(SaveSystem.HasSave());
        }

        public void OnNewRunPressed()
        {
            RunContext.Instance.StartNewRun();
            SceneManager.LoadScene(SceneNames.Explore);
        }

        public void OnContinuePressed()
        {
            var data = SaveSystem.Load();
            if (data == null)
            {
                Debug.LogWarning("[MainMenu] No save found.");
                return;
            }

            RunContext.Instance.LoadRun(data);
            SceneManager.LoadScene(SceneNames.Explore);
        }
    }
}