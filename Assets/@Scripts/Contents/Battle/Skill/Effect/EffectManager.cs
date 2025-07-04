using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    public GameObject FireHandPrefab;
    public GameObject YellowShieldPrefab;
    public GameObject RainAreaPrefab;

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
        FireHandPrefab.transform.position = pos + Vector3.up*5;
        FireHandPrefab.SetActive(true);
        yield return StartCoroutine(MoveDown(FireHandPrefab));
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
        FireHandPrefab.SetActive(false); // 효과 비활성화
    }
    #endregion

    #region FireHand
    public IEnumerator FireHand(Transform left, Transform right)
    {
        GameObject LfireEffect;
        GameObject RfireEffect;

        if (left.childCount == 0 && right.childCount == 0)
        {
            LfireEffect = Instantiate(FireHandPrefab, left);
            RfireEffect = Instantiate(FireHandPrefab, right);
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
            LfireEffect = Instantiate(FireHandPrefab, left);
            RfireEffect = Instantiate(FireHandPrefab, right);
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

    #region Invincibility

    public IEnumerator Invincibility(Transform parent)
    {
        // 이미 자식에 YellowShield 오브젝트가 있는지 검사
        Transform existingShield = parent.Find("Invincible");

        GameObject shield;

        if (existingShield == null)
        {
            // 없으면 새로 생성
            shield = Instantiate(YellowShieldPrefab, Vector3.zero, Quaternion.identity);
            shield.name = "Invincible"; // 나중에 찾기 쉽도록 이름 지정
            shield.transform.SetParent(parent);
            shield.transform.localPosition = Vector3.zero;
        }
        else
        {
            shield = existingShield.gameObject;
        }
        shield.GetComponent<ParticleSystem>().Play(); // 파티클 효과 재생
        yield return new WaitForSeconds(3f);
    }

    #endregion

    #region Rain

    public IEnumerator Rain(Vector3 pos)
    {
        RainAreaPrefab.transform.position = pos;
        RainAreaPrefab.SetActive(true);
        RainAreaPrefab.GetComponent<ParticleSystem>().Play(); // 비 효과 재생
        yield return new WaitForSeconds(5f); // 5초 동안 비 효과 유지
        RainAreaPrefab.SetActive(false); // 비 효과 비활성화
    }

    #endregion
}