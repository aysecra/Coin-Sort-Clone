using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoinSortClone.SO
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        private static GameSettings _instance;

        public static GameSettings Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<GameSettings>().FirstOrDefault();
#if UNITY_EDITOR
                if (!_instance)
                    InitializeFromDefault(
                        UnityEditor.AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/DefaultGameSettings.asset"));
#endif
                return _instance;
            }
        }

        private static string SavedSettingsPath => Path.Combine(Application.dataPath, "Data/GameSettings.json");

        private static string SavedSettingsLocalPath =>
            Path.Combine(Application.persistentDataPath, "Data/GameSettings.json");


        public async Task<(T, bool)> LoadFromJSON<T>(bool isLocal)
        {
            string dirPath = !isLocal ? $"{Application.dataPath}/Data/" : $"{Application.persistentDataPath}/Data/";

            if (Directory.Exists(dirPath))
            {
                try
                {
                    string loadPlayerData =
                        await File.ReadAllTextAsync(!isLocal ? SavedSettingsPath : SavedSettingsLocalPath);
                    (T, bool) result = (JsonUtility.FromJson<T>(loadPlayerData), true);
                    return result;
                }
                catch (Exception e)
                {
                    Directory.CreateDirectory(dirPath);
                    return (default, false);
                }
            }

            Directory.CreateDirectory(dirPath);
            return (default, false);
        }

        public async void SaveToJSON<T>(T saveData, bool isLocal)
        {
            string dirPath = !isLocal ? $"{Application.dataPath}/Data/" : $"{Application.persistentDataPath}/Data/";

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string saveContent = JsonUtility.ToJson(saveData);
            await File.WriteAllTextAsync(!isLocal ? SavedSettingsPath : SavedSettingsLocalPath, saveContent);
        }

        private static void InitializeFromDefault(GameSettings settings)
        {
            if (_instance) DestroyImmediate(_instance);
            _instance = Instantiate(settings);
            _instance.hideFlags = HideFlags.HideAndDontSave;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Window/Game Settings")]
        public static void ShowGameSettings()
        {
            UnityEditor.Selection.activeObject = Instance;
        }
#endif
    }
}