using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Services.API_Service.Game.User;

namespace BuildingSystem
{
    public class BuildingPool
    {
        const string PrefabsAddressPrefix = "Buildings/";

        Transform parent;
        Transform Parent
        {
            get
            {
                if (parent == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Buildings";
                    parent = go.transform;
                }

                return parent;
            }
        }
        Dictionary<BuildingType, List<BuildingBase>> loadedBuildingsDict = new Dictionary<BuildingType, List<BuildingBase>>();
        Dictionary<BuildingType, List<BuildingBase>> instantiatedBuildingsDict = new Dictionary<BuildingType, List<BuildingBase>>();

        BuildingBase GetPrefab(BuildingType type, int level)
        {
            if (!loadedBuildingsDict.ContainsKey(type) || loadedBuildingsDict[type].Find(x => x.Level == level) == default(BuildingBase))
            {
                string buildingPrefabAddress = PrefabsAddressPrefix + $"{type.ToString()}/{type.ToString()}_LVL{level.ToString()}";
                BuildingBase loadedBuilding = Resources.Load<BuildingBase>(buildingPrefabAddress);
                if (loadedBuilding == null)
                    Debug.LogError($"BuildingPool:: Building with type:{type} and Level:{level} does not exist!");

                if (!loadedBuildingsDict.ContainsKey(type))
                    loadedBuildingsDict.Add(type, new List<BuildingBase>());

                loadedBuildingsDict[type].Add(loadedBuilding);
            }

            return loadedBuildingsDict[type].Find(x => x.Level == level);
        }

        public BuildingBase GetActive(BuildingType type, int level = 1)
        {
            if (!instantiatedBuildingsDict.ContainsKey(type) || instantiatedBuildingsDict[type].Find(x => x.Level == level && !x.gameObject.activeInHierarchy) == default(BuildingBase))
            {
                BuildingBase prefab = GetPrefab(type, level);
                BuildingBase instantiatedBuilding = MonoBehaviour.Instantiate<BuildingBase>(prefab, parent);
                instantiatedBuilding.gameObject.SetActive(false);

                if (!instantiatedBuildingsDict.ContainsKey(type))
                    instantiatedBuildingsDict.Add(type, new List<BuildingBase>());

                instantiatedBuildingsDict[type].Add(instantiatedBuilding);
            }

            BuildingBase building = instantiatedBuildingsDict[type].Find(x => x.Level == level && !x.gameObject.activeInHierarchy);
            building.gameObject.SetActive(true);
            return building;
        }
    }
}