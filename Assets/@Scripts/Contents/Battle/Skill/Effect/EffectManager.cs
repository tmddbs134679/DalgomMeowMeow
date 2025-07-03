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

    #region FireHand
    public IEnumerator FireHand(Transform left, Transform right)
    {
        GameObject LfireEffect;
        GameObject RfireEffect;

        if (left.childCount == 0 && right.childCount == 0)
        {
            LfireEffect = Instantiate(BuffEffectPrefab, left);
            RfireEffect = Instantiate(BuffEffectPrefab, right);
        }
        else
        {
            LfireEffect = left.GetChild(0).gameObject;
            RfireEffect = right.GetChild(0).gameObject;
        }

        LfireEffect.transform.localPosition = Vector3.zero;
        RfireEffect.transform.localPosition = Vector3.zero;

        LfireEffect.transform.localRotation = Quaternion.identity;
        RfireEffect.transform.localRotation = Quaternion.identity;

        LfireEffect.SetActive(true);
        RfireEffect.SetActive(true);

        yield return new WaitForSeconds(10f);

        LfireEffect.SetActive(false);
        RfireEffect.SetActive(false);
    }




    #endregion




    #region RangedAttack
    public IEnumerator RangedAttack(Transform left, Transform right)
    {
        GameObject LfireEffect;
        GameObject RfireEffect;

        if (left.childCount == 0 && right.childCount == 0)
        {
            LfireEffect = Instantiate(BuffEffectPrefab, left);
            RfireEffect = Instantiate(BuffEffectPrefab, right);
        }
        else
        {
            LfireEffect = left.GetChild(0).gameObject;
            RfireEffect = right.GetChild(0).gameObject;
        }

        LfireEffect.transform.localPosition = Vector3.zero;
        RfireEffect.transform.localPosition = Vector3.zero;

        LfireEffect.transform.localRotation = Quaternion.identity;
        RfireEffect.transform.localRotation = Quaternion.identity;

        LfireEffect.SetActive(true);
        RfireEffect.SetActive(true);

        yield return new WaitForSeconds(10f);

        LfireEffect.SetActive(false);
        RfireEffect.SetActive(false);
    }




    #endregion
}