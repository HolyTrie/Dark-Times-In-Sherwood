using DTIS;
using UnityEditor.EditorTools;
using UnityEngine;

public class TaskAttackRaycast : MonoBehaviour
{
    [Tooltip("The Attack Damage enemy can deals")]
    [SerializeField] private int EnemyDMG;

    [Tooltip("The Transform of the hitbox of the hit")]
    [SerializeField] private Transform HitTransform;

    public void DamagePlayer()
    {
        var hit = Physics2D.CircleCast(HitTransform.position, 0.3f, Vector3.forward, 1, 1 << 3); // player layer.
        if (hit)
        {
            hit.transform.gameObject.TryGetComponent(out PlayerController playerC);
            if (playerC != null)
            {
                playerC.HpBar.depleteHp(EnemyDMG);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(HitTransform.position, 1);
    }
}