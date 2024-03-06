using DTIS;
using Unity.VisualScripting;
using UnityEngine;

public class TaskAttackRaycast : MonoBehaviour
{
    [Tooltip("The Attack Damage Entity can deal")]
    [SerializeField] private int DMG;

    [Tooltip("The Transform of the hitbox of the hit")]
    [SerializeField] private Transform HitTransform;

    [SerializeField] private float AttackRadius = 0.5f;

    private RaycastHit2D hit;
    public void DamagePlayer()
    {
        hit = Physics2D.CircleCast(HitTransform.position, AttackRadius, Vector3.forward, 1, 1 << 3); // player layer.
        if (hit)
        {
            // hit.transform.gameObject.TryGetComponent(out PlayerController playerC);
            PlayerController playerC = Util.GetPlayerController();
            if (playerC != null)
            {
                playerC.HpBar.depleteHp(DMG);
            }
        }
    }

    public void DamageEnemy()
    {
        hit = Physics2D.CircleCast(HitTransform.position, AttackRadius, Vector3.forward, 1, 1 << 10); // Enemy layer.
        if (hit)
        {
            hit.transform.gameObject.TryGetComponent(out EntityController EntityC);
            if (EntityC != null)
            {
                EntityC.HpBar.depleteHp(DMG);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(HitTransform.position, AttackRadius);
        Gizmos.DrawRay(hit.point,hit.normal);
    }
}