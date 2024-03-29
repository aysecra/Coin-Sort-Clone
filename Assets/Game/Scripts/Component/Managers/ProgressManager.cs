using System;
using System.Collections.Generic;
using CoinSortClone.Data;
using CoinSortClone.Enums;
using CoinSortClone.Interface;
using CoinSortClone.Logic;
using CoinSortClone.SO;
using CoinSortClone.Structs.Event;
using UnityEngine;

namespace CoinSortClone
{
    public class ProgressManager : PersistentSingleton<ProgressManager>
        , EventListener<LevelEvent>
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private CostSO costSo;
        [SerializeField] private List<LevelProgress> levels;

        private PlayerData _playerData;
        // private const string _playerDataPrefKey = "PlayerData";

        protected override void Awake()
        {
            base.Awake();
            // LoadPlayerData();
            LoadAsyncPlayerData();
        }

        private async void LoadAsyncPlayerData()
        {
            (PlayerData, bool) data = await gameSettings.LoadFromJSON<PlayerData>(true);
            if (data.Item2) _playerData = data.Item1;
            else
            {
                ClearData();
            }
        }

        private void SaveAsyncPlayerData()
        {
            gameSettings.SaveToJSON(_playerData, true);
        }

        // private void LoadPlayerData()
        // {
        //     if (PlayerPrefs.HasKey(_playerDataPrefKey))
        //     {
        //         _playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(_playerDataPrefKey));
        //     }
        //
        //     else if (!PlayerPrefs.HasKey(_playerDataPrefKey) && levels.Count > 0)
        //     {
        //         _playerData = new PlayerData
        //         {
        //             LevelName = levels[0].LevelName,
        //             LevelIndex = 0
        //         };
        //     }
        // }
        //
        // private void SetPlayerData()
        // {
        //     PlayerPrefs.SetString(_playerDataPrefKey, JsonUtility.ToJson(_playerData));
        // }

        private void SetNextLevel()
        {
            int index = _playerData.LevelIndex + 1 < levels.Count ? _playerData.LevelIndex + 1 : 0;
            _playerData.LevelName = levels[index].LevelName;
            _playerData.LevelIndex = index;
        }

        public string GetCurrentLevelName()
        {
            return _playerData.LevelName;
        }

        public string GetNextLevelName()
        {
            int index = _playerData.LevelIndex + 1 < levels.Count ? _playerData.LevelIndex + 1 : 0;
            return levels[index].LevelName;
        }

        public uint GetLastOpenedCoin()
        {
            return _playerData.LastOpenedCoin;
        }

        public void IncreaseOpenedCoin()
        {
            _playerData.LastOpenedCoin++;
        }

        public uint GetGold()
        {
            return _playerData.Gold;
        }

        public void IncreaseGold()
        {
            _playerData.Gold += costSo.Earning;
        }

        public void DecreaseGold(uint value)
        {
            _playerData.Gold -= value;
        }
        
        public void DecreaseGold()
        {
            _playerData.Gold -= costSo.DealCost;
        }

        public void SetSlotIndex(int index)
        {
            _playerData.SlotIndex = index;
        }

        public int GetSlotIndex()
        {
            return _playerData.SlotIndex;
        }

        public int CalculateSlotPrice()
        {
            return (int) (costSo.SlotBeginingCost + _playerData.SlotIndex * costSo.SlotCostIncreasing);
        }

        public void ClearData()
        {
            _playerData = new PlayerData
            {
                LevelName = levels[0].LevelName,
                LevelIndex = 0,
                Gold = 200,
                CoinDataList = new List<CoinData>(),
                LastOpenedCoin = 2,
                SlotIndex = 0
            };
            SaveAsyncPlayerData();
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<LevelEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<LevelEvent>(this);
        }

        private void OnApplicationQuit()
        {
            SaveAsyncPlayerData();
        }

        public void OnEventTrigger(LevelEvent currentEvent)
        {
            if (currentEvent.State == LevelState.Completed)
            {
                SetNextLevel();
            }
        }
    }
}