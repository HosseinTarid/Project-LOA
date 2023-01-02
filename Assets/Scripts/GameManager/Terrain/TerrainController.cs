using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOATerrain
{
    public enum TerrainType { Jungle, Beach, Desert, Mountain }
    public class TerrainController
    {
        const string PrefabsAddressPrefix = "Lands/";
        Dictionary<TerrainType, TerrainObject> terrainDict = new Dictionary<TerrainType, TerrainObject>();
        TerrainType? curretnTerrainType = null;
        Action onDone;
        public void ActivateTerrain(TerrainType type, Action onDOne)
        {
            this.onDone = onDOne;
            Action Activate = () =>
            {
                terrainDict[type].gameObject.SetActive(true);
                curretnTerrainType = type;

                onDone?.Invoke();
            };

            DeactivateCurrentTerrain();

            if (!terrainDict.ContainsKey(type))
                LOAGameManager.Instance.StartCoroutine(LoadAndInstantiateTerrain(type, Activate));
            else
                Activate();
        }

        void DeactivateCurrentTerrain()
        {
            if (!curretnTerrainType.HasValue)
                return;

            terrainDict[curretnTerrainType.Value].gameObject.SetActive(false);
            curretnTerrainType = null;
        }

        IEnumerator LoadAndInstantiateTerrain(TerrainType type, Action onDone)
        {
            string terrainPrefabAddress = PrefabsAddressPrefix + type.ToString();
            ResourceRequest terrainRequest = Resources.LoadAsync<TerrainObject>(terrainPrefabAddress);
            yield return terrainRequest;

            TerrainObject terrain = UnityEngine.Object.Instantiate<TerrainObject>(terrainRequest.asset as TerrainObject);
            terrainDict.Add(type, terrain);
            onDone?.Invoke();
        }
    }
}