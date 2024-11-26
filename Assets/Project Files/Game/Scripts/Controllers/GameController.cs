using UnityEngine;
using Watermelon.LevelSystem;
using Watermelon.SquadShooter;

namespace Watermelon
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;

        [Header("Refferences")]
        [SerializeField] UIController uiController;
        [SerializeField] CameraController cameraController;

        [Space]
        [DrawReference]
        [SerializeField] GameSettings settings;

        private static UpgradesController upgradesController;
        private static ParticlesController particlesController;
        private static FloatingTextController floatingTextController;
        private static ExperienceController experienceController;
        private static WeaponsController weaponsController;
        private static CharactersController charactersController;
        private static BalanceController balanceController;
        private static EnemyController enemyController;
        private static TutorialController tutorialController;
        private static CustomMusicController customMusicController;

        public static GameSettings Settings => instance.settings;
        public GameSettings GameSettings => settings;

        private static bool isGameActive;
        public static bool IsGameActive => isGameActive;

        private void Awake()
        {
            instance = this;

            // Cache components
            CacheComponent(out upgradesController);
            CacheComponent(out particlesController);
            CacheComponent(out floatingTextController);
            CacheComponent(out experienceController);
            CacheComponent(out weaponsController);
            CacheComponent(out charactersController);
            CacheComponent(out balanceController);
            CacheComponent(out enemyController);
            CacheComponent(out tutorialController);
            CacheComponent(out customMusicController);

            customMusicController.Init();

            uiController.Init();

            cameraController.Init();

            tutorialController.Initialise();
            upgradesController.Initialise();
            particlesController.Init();
            floatingTextController.Init();

            settings.Initialise();

            LevelController.Initialise();

            experienceController.Initialise();
            weaponsController.Initialise();
            charactersController.Initialise();
            balanceController.Initialise();
            enemyController.Initialise();

            LevelController.SpawnPlayer();

            uiController.InitPages();

            CustomMusicController.PlayMenuMusic();
        }

        private void Start()
        {
            UIController.ShowPage<UIMainMenu>();

            CameraController.SetCameraShiftState(false);

            LevelController.LoadCurrentLevel();
        }

        private void OnDestroy()
        {
            settings.Unload();

            LevelController.Unload();

            Tween.RemoveAll();
        }

        public static void OnGameStarted()
        {
            isGameActive = true;
        }

        public static void LevelComplete()
        {
            if (!isGameActive)
                return;

            LevelData currentLevel = LevelController.CurrentLevelData;

            UIComplete completePage = UIController.GetPage<UIComplete>();
            completePage.SetData(ActiveRoom.CurrentWorldIndex + 1, ActiveRoom.CurrentLevelIndex + 1, currentLevel.GetCoinsReward(), currentLevel.XPAmount, currentLevel.GetCardsReward());

            UIController.PageOpened += OnCompletePageOpened;

            weaponsController.CheckWeaponUpdateState();

            UIController.HidePage<UIGame>();
            UIController.ShowPage<UIComplete>();

            isGameActive = false;
        }

        private static void OnCompletePageOpened(UIPage page, System.Type pageType)
        {
            if(pageType == typeof(UIComplete))
            {
                LevelController.UnloadLevel();

                UIController.PageOpened -= OnCompletePageOpened;
            }
        }

        public static void OnLevelCompleteClosed()
        {
            UIController.HidePage<UIComplete>(() =>
             {
                 if(LevelController.NeedCharacterSugession)
                 {
                     UIController.ShowPage<UICharacterSuggestion>();
                 }
                 else
                 {
                     ShowMainMenuAfterLevelComplete();
                 }
             });
        }

        public static void OnCharacterSugessionClosed()
        {
            ShowMainMenuAfterLevelComplete();
        }

        private static void ShowMainMenuAfterLevelComplete()
        {
            AdsManager.ShowInterstitial(null);

            CustomMusicController.PlayMenuMusic();

            CameraController.SetCameraShiftState(false);
            CameraController.EnableCamera(CameraType.Menu);

            UIController.ShowPage<UIMainMenu>();
            ExperienceController.GainXPPoints(LevelController.CurrentLevelData.XPAmount);

            SaveController.Save(true);

            LevelController.LoadCurrentLevel();
        }

        public static void OnLevelExit()
        {
            isGameActive = false;
        }

        public static void OnLevelFailded()
        {
            if (!isGameActive)
                return;

            UIController.HidePage<UIGame>(() =>
            {
                UIController.ShowPage<UIGameOver>();
                UIController.PageOpened += OnFailedPageOpened;
            });

            LevelController.OnLevelFailed();

            isGameActive = false;
        }

        private static void OnFailedPageOpened(UIPage page, System.Type pageType)
        {
            if (pageType == typeof(UIGameOver))
            {
                AdsManager.ShowInterstitial(null);

                UIController.PageOpened -= OnFailedPageOpened;
            }
        }

        public static void OnReplayLevel()
        {
            isGameActive = true;

            CustomMusicController.PlayMenuMusic();

            CameraController.SetCameraShiftState(false);
            CameraController.EnableCamera(CameraType.Menu);
            LevelController.UnloadLevel();

            UIController.HidePage<UIGameOver>(() =>
            {
                LevelController.LoadCurrentLevel();
                UIController.ShowPage<UIMainMenu>();
            });
        }

        public static void OnRevive()
        {
            isGameActive = true;

            UIController.HidePage<UIGameOver>(() =>
            {
                LevelController.ReviveCharacter();

                UIController.ShowPage<UIGame>();
            });
        }

        #region Extensions
        public bool CacheComponent<T>(out T component) where T : Component
        {
            Component unboxedComponent = gameObject.GetComponent(typeof(T));

            if (unboxedComponent != null)
            {
                component = (T)unboxedComponent;

                return true;
            }

            Debug.LogError(string.Format("Scripts Holder doesn't have {0} script added to it", typeof(T)));

            component = null;

            return false;
        }
        #endregion
    }
}