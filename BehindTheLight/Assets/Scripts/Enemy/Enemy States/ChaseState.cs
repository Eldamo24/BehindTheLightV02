using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Chase")]
public class ChaseState : EnemyState
{
    [SerializeField] private EnemyState attackState;
    [SerializeField] private EnemyState patrolState;
    private float attackRange = 3f;
    private float detectionRange = 15f;

    public override void Enter(EnemyStateMachine stateMachine) 
    {
        stateMachine.Anim.SetBool("Running", true);
    }

    public override void UpdateState(EnemyStateMachine stateMachine)
    {
        float dist = Vector3.Distance(stateMachine.transform.position, stateMachine.Player.position);
        if (dist > detectionRange)
        {
            stateMachine.ChangeState(patrolState);
            return;
        }
        if (dist <= attackRange) 
        {
            stateMachine.ChangeState(attackState);
            return;
        }
        stateMachine.Agent.SetDestination(stateMachine.Player.position);
    }

    public override void Exit(EnemyStateMachine stateMachine)
    {
        stateMachine.Agent.ResetPath();
    }


}
