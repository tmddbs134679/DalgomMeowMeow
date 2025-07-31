using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterView : MonoBehaviour
{
    AICharacter AICharacter;
    SkinnedMeshRenderer skinnedMeshRenderer;
    public CharacterEmoSet emo;
    Material currentEmo;

    private Animator animator;
    private NavMeshAgent nav;
    public int CurrentAnimation { get; set; }

    public Animator Animator => animator;
    public NavMeshAgent Nav => nav;


    public void Init(AICharacter _aiCharacter)
    {
        AICharacter = _aiCharacter;

        skinnedMeshRenderer = skinnedMeshRenderer = AICharacter.GetComponentInChildren<SkinnedMeshRenderer>();
        currentEmo = skinnedMeshRenderer.materials[1];
        animator = AICharacter.GetComponent<Animator>();
        nav = AICharacter.GetComponent<NavMeshAgent>();
    }


    public void SetAnimation(int animNum)
    {
        Animator.SetInteger("animation", animNum);
        CurrentAnimation = animNum;
    }

    public void SetEmotion(int index)
    {
        if (emo == null) return;
        currentEmo = emo.EmotionMaterials[index];
        var mats = skinnedMeshRenderer.materials;
        mats[1] = emo.EmotionMaterials[index];
        skinnedMeshRenderer.materials = mats;
    }

    public void SetSpeed(float speed)
    {
        nav.speed = speed;
    }


}
