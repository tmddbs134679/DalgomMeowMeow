using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public enum TeamState { Moving, Fighting, Returning }

    [SerializeField] private TeamState _currentState;
    [SerializeField] private float _moveSpeed = 2f;
    private List<PlayerCharacter> _members;

    private void Awake()
    {
        _members = GetComponentsInChildren<PlayerCharacter>().ToList();
    }

    private void Update()
    {
        _members.RemoveAll(m => m.IsDead);

        if (_members.Count == 0)
        {
            GameOver();
            return;  // 더 이상 진행하지 않고 함수 종료
        }
        if(Managers.Battle.Victory)
        {
            _members.ForEach(m => m.Animator.SetInteger("animation", 8));
            return;
        }

        switch (_currentState)
        {
            case TeamState.Moving:
                MoveForward();
                break;

            case TeamState.Returning:

                foreach (var m in _members)
                {
                    if (!m.HasLookedForward &&
                        !m.Agent.pathPending &&
                        m.Agent.remainingDistance <= m.Agent.stoppingDistance &&
                        m.Agent.velocity.sqrMagnitude == 0f)
                    {
                        Vector3 forward = transform.forward;
                        forward.y = 0f;

                        if (forward != Vector3.zero)
                            m.SmoothLookForward(forward);

                        m.HasLookedForward = true;
                    }
                }

                if (AllReturned())
                {
                    _currentState = TeamState.Moving;
                    _members.ForEach(m =>
                    {
                        m.Agent.ResetPath();
                        m.Agent.speed = m.MoveSpeed;
                    });
                }
                break;
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    }


    

    private void EnterBattle()
    {
        _currentState = TeamState.Fighting;
    }

    private void ReturnToFormation()
    {
        _currentState = TeamState.Returning;
        foreach (var m in _members)
            m.ReturnToStartPosition();
    }

    private bool AllReturned()
    {
        return _members.All(m =>
        !m.Agent.pathPending &&
        m.Agent.remainingDistance <= m.Agent.stoppingDistance &&
        m.Agent.velocity.sqrMagnitude == 0f);
    }


    private void OnEnable()
    {
        foreach (var member in _members)
        {
            member.OnBattleStateChanged += HandleMemberBattleStateChanged;
        }
    }

    private void OnDisable()
    {
        foreach (var member in _members)
        {
            member.OnBattleStateChanged -= HandleMemberBattleStateChanged;
        }
    }

    private void HandleMemberBattleStateChanged(BattleCharacter member, bool isInBattle)
    {
        if (_members.All(m => m.IsInBattle))
        {
            EnterBattle();
        }
        else if (_currentState == TeamState.Fighting && _members.All(m => !m.IsInBattle))
        {
            ReturnToFormation();
        }
    }


    public void GameOver()
    {
        Debug.Log("You Lose!");
    }
}
