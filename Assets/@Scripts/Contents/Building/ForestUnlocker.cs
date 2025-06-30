using UnityEngine;

public class ForestUnlocker : MonoBehaviour
{
    private void Start()
    {
        int unlockId = ForestBattleContext.PendingUnlockForestId;
        if (unlockId < 0) return; // 전투 결과가 없는 경우

        // 현재 씬에 있는 모든 ForestRegion을 찾음
        ForestRegion[] regions = FindObjectsOfType<ForestRegion>();
        foreach (var region in regions)
        {
            if (region.Id == unlockId)
            {
                region.Unlock();
                Debug.Log($"[ForestUnlocker] Forest {unlockId} 해금 완료");
                break;
            }
        }

        // ID 초기화 (중복 해금 방지)
        ForestBattleContext.PendingUnlockForestId = -1;
    }
}