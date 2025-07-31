using DG.Tweening;
using Scripts.Contents.AI.FSM.State;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class AICharacterInteraction : MonoBehaviour
{
    AICharacter AICharacter;
    [HideInInspector]
    public float clickStartTime = 0f;
    
    private float longPressThreshold = 0.2f;

    [HideInInspector]
    private LayerMask groundLayer;

    private bool isTweening;
    [HideInInspector]
    public bool isClicked = false;
    [HideInInspector]
    public bool isFollowing = false;


    [HideInInspector]
    public Camera _camera;

    private float tempCameraSize = 0;

    private Vector3 tempCameraPos = new Vector3(21.2f, 26.35f, 9.3f); // 초기 카메라 위치

    private GameObject infoButton;
    private Transform head;

    private TextMeshProUGUI nameText;
    public void Init(AICharacter _aiCharacter)
    {
        AICharacter = _aiCharacter;
        groundLayer = LayerMask.GetMask("Ground");
        head = AICharacter.transform.Find("root/pelvis/spine_01/spine_02/spine_03/neck_01");
        infoButton = AICharacter.transform.Find("Canvas").gameObject;
        nameText = AICharacter.transform.Find("Canvas/Name").GetComponent<TextMeshProUGUI>();
        nameText.text = AICharacter.Stat.data.Name;
        _camera = Camera.main;
    }


    public void LongPressClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == hit.collider.transform.IsChildOf(AICharacter.transform) &&
                    (
                    AICharacter.Controller.CurrentState() is CharacterIdleState ||
                    AICharacter.Controller.CurrentState() is CharacterMoveToState ||
                    AICharacter.Controller.CurrentState() is CharacterDeliverState)
                    )

                {
                    clickStartTime += Time.deltaTime;
                    if (clickStartTime > longPressThreshold)
                    {
                        AICharacter.View.Nav.enabled = false;
                        isClicked = false;
                        isFollowing = true;
                    }

                }
            }
        }

        if (isFollowing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
            {
                infoButton.SetActive(false);
                Vector3 mouspot = hit.point;
                AICharacter.View.Animator.SetInteger("animation", 49);
                //SetSpeed(0);
                AICharacter.transform.position = new Vector3(mouspot.x, hit.point.y + 2f, mouspot.z);

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (clickStartTime <= 0.2f && hit.collider.transform.IsChildOf(AICharacter.transform) &&
                    AICharacter.Controller.CurrentState() is not CharacterHelloState &&
                    Managers.Scene.CurrentScene is GameScene &&
                    !isFollowing)
                {
                    if (isClicked)
                    {
                        // 확대 전 상태 저장
                        tempCameraSize = _camera.orthographicSize;
                        tempCameraPos = _camera.transform.position;

                        // 부드럽게 줌인
                        Vector3 targetPos = new Vector3(transform.position.x - 20.3f, 30.5f, transform.position.z - 20.6f);
                        _camera.DOOrthoSize(2f, 0.7f);
                        _camera.transform.DOMove(targetPos, 0.7f).OnComplete(() => isTweening = false);
                    }
                    else if (!isClicked && !isTweening)
                    {
                        isTweening = true;

                        Vector3 targetPos = new Vector3(tempCameraPos.x, 26.35f, tempCameraPos.z);
                        _camera.DOOrthoSize(7, 0.7f);
                        _camera.transform.DOMove(targetPos, 0.7f).OnComplete(() => isTweening = false);
                    }

                }
            }

            if (isFollowing)
            {
                AICharacter.View.Nav.enabled = true;
                AICharacter.transform.position = new Vector3(AICharacter.transform.position.x, AICharacter.transform.position.y, AICharacter.transform.position.z);
                Managers.AI.ValidateNavMeshPosition(AICharacter);
            }
            isFollowing = false;
            AICharacter.View.SetAnimation(AICharacter.View.CurrentAnimation);
            clickStartTime = 0f;
        }
    }
    public void ClickToSet()
    {
        if (isClicked)
        {
            AICharacter.View.SetSpeed(0);
            AICharacter.gameObject.transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y + 180, 0);
            head.transform.localRotation = quaternion.Euler(0, 0, -12);
            infoButton.SetActive(true);
            AICharacter.View.Animator.SetInteger("animation", 36);
        }

        else if (!isClicked && !isFollowing)
        {
            if (AICharacter.Controller.CurrentState() is CharacterIdleState)
                AICharacter.View.SetSpeed(AICharacter.Stat.data.WalkSpeed);
            else if (AICharacter.Controller.CurrentState() is CharacterDeliverState)
                AICharacter.View.SetSpeed(AICharacter.Stat.data.MoveSpeed / 2);
            else
                AICharacter.View.SetSpeed(AICharacter.Stat.data.MoveSpeed);

            AICharacter.View.SetAnimation(AICharacter.View.CurrentAnimation);
            infoButton.SetActive(false);
        }
    }


}
