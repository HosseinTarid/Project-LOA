using System;
using System.Collections.Generic;
using UnityEngine;

public class DoOnMainThread : Utility.Patterns.MonoBehaviourSingleton<DoOnMainThread>
{
    Action action;

    public void Do(Action action)
    {
        this.action = action;
    }
    
    void Update()
    {
        if (action != null)
        {
            action.Invoke();
            action = null;
        }
    }
}
