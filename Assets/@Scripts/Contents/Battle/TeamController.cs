using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public enum TeamState { Moving, Fighting, Returning }

    [SerializeField] private TeamState _currentState;
    [SerializeField] private List<BattleCharacter> _members;
    [SerializeField] private float _moveSpeed = 2f;


    private void Awake()
    {
        _members = GetComponentsInChildren<BattleCharacter>().ToList();
    }

    private void Update()
    {
        _members.RemoveAll(m => m.IsDead);

        if (_members.Count == 0)
        {
            GameOver();
            return;  // 더 이상 진행하지 않고 함수 종료
        }

        switch (_currentState)
        {
            case TeamState.Moving:
                MoveForward();
                break;

            case TeamState.Returning:
                if (AllReturned())
                    _currentState = TeamState.Moving;
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
            Vector3.Distance(m.transform.position, m.GetOriginalWorldPosition()) < 0.2f);
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
