using System.Collections;
using CoinSortClone.Enums;
using CoinSortClone.Logic;
using CoinSortClone.Structs.Event;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoinSortClone.Components.Manager
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private float _levelCompleteDelay;
        [SerializeField] private float _levelFailDelay;
        
        public void SetLevelPaused()
        {
            // todo: level pause element will added
        }

        public void SetLevelCompleted()
        {
            StartCoroutine(ICompleteLevel());
        }
        
        IEnumerator ICompleteLevel()
        {
            yield return new WaitForSeconds(_levelCompleteDelay);
            DOTween.KillAll();
            LoadNextLevel();
        }
        
        public void SetLevelFailed()
        {
            StartCoroutine(IReloadLevel());
        }
        
        IEnumerator IReloadLevel()
        {
            yield return new WaitForSeconds(_levelFailDelay);
            DOTween.KillAll();
            ReloadLevel();
        }
        
        private void LoadNextLevel()
        {
            string nextLevel = ProgressManager.Instance.GetNextLevelName();
            EventManager.TriggerEvent(new LevelEvent(LevelState.Completed));
            SceneManager.LoadScene(nextLevel);
        }

        private void ReloadLevel()
        {
            string currLevel = ProgressManager.Instance.GetCurrentLevelName();
            EventManager.TriggerEvent(new LevelEvent(LevelState.Failed));
            SceneManager.LoadScene(currLevel);
        }
    }
}
