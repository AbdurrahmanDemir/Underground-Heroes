using System.Collections;
using UnityEngine;

public class HeroHealthPassage : Passage
{    
    protected override void ApplyBoost()
    {
        upgradeSelectManager.HeroHealthItem(amount);
    }
}
