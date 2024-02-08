using System;
using System.Collections.Generic;

namespace CoinSortClone.Data
{
    [Serializable]
    public class PlayerData
    {
        public int LevelIndex;
        public string LevelName;
        public List<CoinData> CoinDataList;
    }
}