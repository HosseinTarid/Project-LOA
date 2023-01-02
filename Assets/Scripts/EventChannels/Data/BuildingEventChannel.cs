using System;
using EventChannels.Schema;
using UnityEngine;

namespace EventChannels.Data
{
    [CreateAssetMenu(fileName = "Building Event Channel", menuName = "Event Channels/Building Events", order = 0)]
    public class BuildingEventChannel : EventChannel
    {
        public event Action upgradeStartEvent;
        public event Action upgradeFinishEvent;
        public event Action upgradeCancel;

        public void OnUpgradeStartEvent()
        {
            upgradeStartEvent?.Invoke();
        }

        public void OnUpgradeFinishEvent()
        {
            upgradeFinishEvent?.Invoke();
        }

        public void OnUpgradeCancel()
        {
            upgradeCancel?.Invoke();
        }
    }
}