using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Watermelon
{
    [InitializeOnLoad]
    public static class AutoInitializerLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadMain()
        {
            if (!CoreEditor.AutoLoadInitializer) return;

            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene != null)
            {
                if (currentScene.name != CoreEditor.InitSceneName)
                {
                    Initializer initializer = Object.FindObjectOfType<Initializer>();
                    if (initializer == null)
                    {
                        GameObject initializerPrefab = EditorUtils.GetAsset<GameObject>("Initializer");
                        if (initializerPrefab != null)
                        {
                            GameObject InitializerObject = Object.Instantiate(initializerPrefab);

                            initializer = InitializerObject.GetComponent<Initializer>();
                            initializer.Awake();
                            initializer.Init(false);
                        }
                        else
                        {
                            Debug.LogError("[Game]: Initializer prefab is missing!");
                        }
                    }
                }
            }
        }
    }
}