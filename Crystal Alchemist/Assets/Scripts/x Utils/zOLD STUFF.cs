using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zOLDSTUFF
{
    /*
    // Start is called before the first frame update
    private IEnumerator fireSkillToMultipleTargets(TargetingSystem targetingSystem, StandardSkill skill)
    {
        float damageReduce = targetingSystem.sortedTargets.Count;

        if (targetingSystem.sortedTargets.Count > 0 && targetingSystem.lastID == 0)
        {
            targetingSystem.lastID = targetingSystem.sortedTargets[targetingSystem.sortedTargets.Count - 1].gameObject.GetInstanceID();

            int ID = 1;

            for (int i = 0; ID != targetingSystem.lastID;)
            {
                if (targetingSystem.sortedTargets[i] == null) i++; //Springe weiter, wenn das Ziel nicht mehr existiert
                else
                {
                    Character target = targetingSystem.sortedTargets[i];
                    ID = target.gameObject.GetInstanceID();

                    if (targetingSystem.hittedIDs.Contains(ID))
                    {
                        i++; //Springe weiter, wenn das Ziel bereits getroffen wurde und noch existiert
                    }
                    else
                    {
                        bool playSoundEffect = false;
                        if (i == 0 || skill.multiHitDelay > 0.3f) playSoundEffect = true;

                        fireSkillToSingleTarget(target, damageReduce, playSoundEffect, skill);
                        targetingSystem.hittedIDs.Add(target.gameObject.GetInstanceID());
                        yield return new WaitForSeconds(skill.multiHitDelay);
                    }
                }
            }
        }

        Destroy(this.activeLockOnTarget);
        this.activeLockOnTarget = null;
    }*/
}
