using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Idle")]
public class IdleState : EnemyState
{
    private float idleDuration = 2f;
    [SerializeField] private EnemyState patrolState;
    [SerializeField] private EnemyState chaseState;
    private float detectionRange = 15f;

    public override void Enter(EnemyStateMachine stateMachine)
    {
        stateMachine.Agent.ResetPath();
        stateMachine.IdleTimer = 0f;
        stateMachine.Anim.SetBool("Running", false);
    }


    public override void UpdateState(EnemyStateMachine stateMachine)
    {
        stateMachine.IdleTimer += Time.deltaTime;
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.Player.position) < detectionRange)
        {
            stateMachine.ChangeState(chaseState);
            return;
        }
        if (stateMachine.IdleTimer >= idleDuration)
        {
            stateMachine.ChangeState(patrolState);
        }
    }

    public override void Exit(EnemyStateMachine stateMachine) { }


}
