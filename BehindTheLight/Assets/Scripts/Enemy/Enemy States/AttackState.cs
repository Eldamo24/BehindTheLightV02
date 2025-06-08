using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/States/Attack")]
public class AttackState : EnemyState
{
    [SerializeField] private EnemyState chaseState;
    private float attackCooldown = 2f;
    private float attackRange = 1f;
    private float timer;

    public override void Enter(EnemyStateMachine stateMachine)
    {
        timer = 0f;
        stateMachine.Agent.ResetPath();
        stateMachine.Anim.SetTrigger("Attack");
    }

    public override void UpdateState(EnemyStateMachine stateMachine)
    {
        float dist = Vector3.Distance(stateMachine.transform.position, stateMachine.Player.position);
        if (dist > attackRange) 
        {
            stateMachine.ChangeState(chaseState);
            return;
        }
        stateMachine.transform.LookAt(stateMachine.Player);
        timer += Time.deltaTime;
        if(timer >= attackCooldown)
        {
            timer = 0f;
            Debug.Log("Attack");
        }
    }

    public override void Exit(EnemyStateMachine stateMachine) { }
}
