using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    public GameObject HealEffectPrefab;
    public GameObject AttackEffectPrefab;
    public GameObject BuffEffectPrefab;
    public GameObject DebuffEffectPrefab;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    #region PunchEffect
    public IEnumerator Punch(Vector3 pos)
    {
        AttackEffectPrefab.transform.position = pos + Vector3.up*5;
        AttackEffectPrefab.SetActive(true);
        yield return StartCoroutine(MoveDown(AttackEffectPrefab));
    }

    private IEnumerator MoveDown(GameObject effect)
    {
        float duration = 1f;
        float elapsed = 0f;
        float speed = 20f; // 1초 동안 1단위 이동 (1 unit/sec)

        while (elapsed < duration && effect.transform.position.y > 0)
        {
            float delta = Time.deltaTime;
            effect.transform.position += Vector3.down * speed * delta;
            elapsed += delta;
            yield return null;
        }
        AttackEffectPrefab.SetActive(false); // 효과 비활성화
    }
    #endregion


}