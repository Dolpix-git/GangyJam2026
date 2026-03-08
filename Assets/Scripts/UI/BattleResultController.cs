using CardGame.Run;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame.UI
{
    public class BattleResultController : MonoBehaviour
    {
        public void OnReturnToExplorePressed()
        {
            RunContext.Instance.Save();
            SceneManager.LoadScene(SceneNames.Explore);
        }

        public void OnReturnToMainMenuPressed()
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
        }
    }
}