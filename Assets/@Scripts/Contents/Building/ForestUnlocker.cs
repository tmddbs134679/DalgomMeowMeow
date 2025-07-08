using System.Collections;
using UnityEngine;

public class ForestUnlocker : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null; // 한 프레임 대기 (씬 생성 완료 대기)

        if (!ForestBattleContext.IsVictory) yield break;

        int unlockId = ForestBattleContext.PendingUnlockForestId;
        if (unlockId < 0) yield break;

        ForestRegion[] regions = FindObjectsOfType<ForestRegion>(true);
        //Debug.Log($"[ForestUnlocker] 지역 개수: {regions.Length}");

        foreach (var region in regions)
        {
            //Debug.Log($"[ForestUnlocker] 검사 중: ID {region.Id}");
            if (region.Id == unlockId)
            {
                region.Unlock();
                //Debug.Log($"[ForestUnlocker] Forest {unlockId} 해금 완료");
                break;
            }
        }

        ForestBattleContext.PendingUnlockForestId = -1;
        ForestBattleContext.IsVictory = false;
    }
}