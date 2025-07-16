using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolDown : MonoBehaviour
{
    private Image _image;
    public float Cooldown;
    private bool usable = true; // 버튼 사용 가능 여부
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (usable == false)
            return;
        

        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= Time.deltaTime / Cooldown; // 5초 동안 쿨타임
        }
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 0; // 쿨타임이 끝나면 이미지가 0으로 설정
            this.gameObject.SetActive(false); // 버튼 비활성화
        }
    }

    public void SkillActive(float cooldown, bool dead)
    {
        Cooldown = cooldown; // 쿨타임 시간 설정 (5초)
        this.gameObject.SetActive(true); // 버튼 활성화
        _image.fillAmount = 1; // 쿨타임 이미지 초기화
    }

    public void ButtonLock()
    {
        if (!usable)
            return;
        this.gameObject.SetActive(true); // 버튼 활성화
        _image.fillAmount = 1; // 쿨타임 이미지 초기화
        usable = false;
    }
}
