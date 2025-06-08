using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Patrol")]
public class PatrolState : EnemyState
{
    [SerializeField] private EnemyState chaseState;
    private float detectionRange = 15f;

    public override void Enter(EnemyStateMachine stateMachine)
    {
        stateMachine.Agent.SetDestination(stateMachine.WayPoints[stateMachine.PatrolIndex].position);
        stateMachine.Anim.SetBool("Running", true);
    }

    public override void UpdateState(EnemyStateMachine stateMachine)
    {
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.Player.position) < detectionRange) 
        {
            stateMachine.ChangeState(chaseState);
            return;
        }
        if(!stateMachine.Agent.pathPending && stateMachine.Agent.remainingDistance < 0.5f)
        {
            stateMachine.PatrolIndex = (stateMachine.PatrolIndex +1) % stateMachine.WayPoints.Length;
            stateMachine.Agent.SetDestination(stateMachine.WayPoints[stateMachine.PatrolIndex].position);
        }
    }
    
    public override void Exit(EnemyStateMachine stateMachine) { }
}
