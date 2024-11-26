using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Watermelon.SquadShooter;

namespace Watermelon
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] UIController uiController;
        [SerializeField] MusicSource musicSource;

        [Space]
        [SerializeField] Button world1Button;
        [SerializeField] Button world2Button;

        private void Awake()
        {
            uiController.Init();
            uiController.InitPages();

            // Activate menu music
            musicSource.Init();
            musicSource.Activate();

            // Add click events
            world1Button.onClick.AddListener(() => { LoadLevel(0, 0); });
            world2Button.onClick.AddListener(() => { LoadLevel(1, 0); });
        }

        public void LoadLevel(int worldIndex, int levelIndex)
        {
            LevelSave levelSave = SaveController.GetSaveObject<LevelSave>("level");
            levelSave.WorldIndex = worldIndex;
            levelSave.LevelIndex = levelIndex;

            Overlay.Show(0.3f, () =>
            {
                SceneManager.LoadScene("Game");

                Overlay.Hide(0.3f);
            });
        }
    }
}