using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlexGhost.Models
{
    public class GameData : MonoBehaviour
    {
        public static GameData instance;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        /*Dictionary<string, BlockInfo> blockInfoDatas;
        Dictionary<string, PlantInfo> plantInfoDatas;
        Dictionary<string, AreaInfo> areaInfoDatas;
        public void InitBlockInfo(List<BlockInfo> blockInfos)
        {
            blockInfoDatas = new Dictionary<string, BlockInfo>();
            for (int i = 0; i < blockInfos.Count; i++)
            {
                blockInfoDatas.Add(blockInfos[i].blockID, blockInfos[i]);
            }
        }

        public BlockInfo GetBlockInfoByBlockID(string blockID)
        {
            return blockInfoDatas[blockID];
        }

        public void InitPlantInfo(List<PlantInfo> plantInfos)
        {
            plantInfoDatas = new Dictionary<string, PlantInfo>();
            for (int i = 0; i < plantInfos.Count; i++)
            {
                plantInfoDatas.Add(plantInfos[i].plantID, plantInfos[i]);
            }
        }

        public PlantInfo GetPlantInfoByPlantID(string plantID)
        {
            return plantInfoDatas[plantID];
        }

        public void InitAreaInfo(List<AreaInfo> areaInfos)
        {
            areaInfoDatas = new Dictionary<string, AreaInfo>();
            for (int i = 0; i < areaInfos.Count; i++)
            {
                areaInfoDatas.Add(areaInfos[i].areaID, areaInfos[i]);
            }
        }

        public AreaInfo GetAreaInfoByAreaID(string areaID)
        {
            return areaInfoDatas[areaID];
        }*/

    }
}
