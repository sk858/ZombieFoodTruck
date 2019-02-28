using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trash : TaskManager
{	
    public override bool OnWorkingChanged(bool isWorking, Worker worker)
    {
        if (isWorking && (worker.HoldingFood || worker.HoldingPotatoes || worker.HoldingIce))
        {
            if (worker.HoldingFood)
            {
                worker.DiscardFood();
            }
            else if (worker.HoldingPotatoes)
            {
                worker.RemovePotatoes();
            }
            else if(worker.HoldingIce)
            {
            }
			TutorialManager.Instance.OnTaskStarted (TaskType.Trash);
			TutorialManager.Instance.OnTaskFinished (TaskType.Trash);
			TruckLogger.Instance.LogUseTrash ();
            return true;
        }
        return false;
    }

    public override bool CanDock(Worker worker)
    {
        return worker.HoldingFood || worker.HoldingPotatoes;
    }
}
