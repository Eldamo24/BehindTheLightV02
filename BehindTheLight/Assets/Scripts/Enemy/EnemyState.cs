using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
    public abstract void Enter(EnemyStateMachine stateMachine);
    public abstract void UpdateState(EnemyStateMachine stateMachine);
    public abstract void Exit(EnemyStateMachine stateMachine);
}
