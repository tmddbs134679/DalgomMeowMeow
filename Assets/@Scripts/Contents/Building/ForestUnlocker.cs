using System.Collections;
using UnityEngine;

public class ForestUnlocker : MonoBehaviour
{
    private IEnumerator Start()
    {
        
        if (!ForestBattleContext.IsVictory) yield break;

        int unlockId = ForestBattleContext.PendingUnlockForestId;
        if (unlockId < 0) yield break;
        
        // 지역이 등록될 때까지(최대 5초) 대기
        float t = 0f;
        while (ForestRegion.All.Count == 0 && t < 5f)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (ForestRegion.All.Count == 0) yield break;

        var target = ForestRegion.All.Find(r => r.Id == unlockId);
        if (target != null)
        {
            target.Unlock();
            ForestBattleContext.PendingUnlockForestId = -1;
            ForestBattleContext.IsVictory = false;
        }
    }
}