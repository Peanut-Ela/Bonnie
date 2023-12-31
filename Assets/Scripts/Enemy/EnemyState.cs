using EnemyStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyStates
{


    public class EnemyState : BaseState
    {
        protected Enemy enemy;
        public EnemyState(Enemy sm) : base(sm)
        {
            enemy = sm;
        }

        protected void LerpPosition(Transform transform, Vector3 targetPosition, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }

        protected void FlipSprite(bool flip)
        {
            enemy.sr.flipX = flip;
        }
    }


    public class IdleState : EnemyState
    {
        public IdleState(Enemy sm) : base(sm)
        {
            duration = Random.Range(enemy.idleDurationMin, enemy.idleDurationMax);
        }
        public override void OnEnter()
        {
            base.OnEnter();
            enemy.animator.PlayInFixedTime(Enemy.IdleKey);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = Vector2.zero;
        }
        public override void OnStateExpired()
        {
            base.OnStateExpired();
            enemy.QueueState(new WanderState(enemy));
        }
    }
    public class WanderState : EnemyState
    {
        Vector2 walkDir;
        public WanderState(Enemy sm) : base(sm)
        {
            duration = 1;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            enemy.animator.PlayInFixedTime(Enemy.WalkKey);
            walkDir = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
            if ((enemy.sr.flipX && walkDir.x > 0) || (!enemy.sr.flipX && walkDir.x < 0))
                enemy.sr.flipX = !enemy.sr.flipX;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = walkDir * enemy.moveSpeed;
        }
    }

    public class ChargeWindupState : EnemyState
    {
        Vector2 windupDir;
        public ChargeWindupState(Enemy sm) : base(sm)
        {
            duration = 0.25f;
        }
        public override void OnEnter()
        {
            base.OnEnter();

            windupDir = -enemy.moveDirection;
            enemy.bufferedState = enemy.AttackState;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = windupDir * 3f;
        }
    }

    public class ChargeState : EnemyState
    {
        private float chargeSpeed = 10f;
        Vector2 targetDir;

        public ChargeState(Enemy sm) : base(sm)
        {
            duration = Vector3.Distance(enemy.transform.position, Player.instance.transform.position) / chargeSpeed;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // Play charge animation
            targetDir = (Player.instance.transform.position - enemy.transform.position).normalized;
            enemy.animator.PlayInFixedTime(Enemy.ChargeKey);
            FlipSprite(targetDir.x < 0);

            enemy.bufferedState = new FirstPunchState(enemy);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = targetDir * chargeSpeed;
            // Move towards the target position at charge speed

        }
        public override void OnExit()
        {
            base.OnExit();
            enemy.rb.velocity = Vector2.zero;
        }
        private void PerformAttack()
        {
            // Do whatever actions are needed for the attack
            // For example, play attack animation, deal damage, etc.

            // Transition to the next state after the attack
            enemy.QueueState(new FirstPunchState(enemy));
        }
    }

    public class HurtState : EnemyState
    {
        private float hurtDuration = 0.5f; // Duration of the hurt animation

        public HurtState(Enemy sm) : base(sm)
        {
            // Set the duration of the hurt state
            duration = hurtDuration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // Play hurt animation
            enemy.animator.PlayInFixedTime(Enemy.HurtKey);
            enemy.Health -= Player.instance.damage;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = Vector2.zero;
        }

        public override void OnExit()
        {
            base.OnExit();
            // Transition to the appropriate state after the hurt state
            enemy.QueueState(new IdleState(enemy));
        }
    }
    public class DeathState : EnemyState
    {
        private float fadeDuration = 1f;
        private int fadeIterations = 3;

        public DeathState(Enemy sm) : base(sm)
        {
            // Set the duration of the death state
            duration = fadeDuration * fadeIterations * 2; // Fade in and out iterations
        }

        public override void OnEnter()
        {
            base.OnEnter();
            enemy.animator.PlayInFixedTime(Enemy.HurtKey);
            // Play death animation or perform any necessary actions
            // Example: enemy.animator.PlayInFixedTime(Enemy.DeathKey);
            // You can also disable any colliders or gameplay components

            // Start the coroutine to handle the death sequence
            enemy.Die();
        }
    }

    public class FirstPunchState : EnemyState
    {
        private Vector3 targetHandPosition;

        public FirstPunchState(Enemy sm) : base(sm)
        {
            duration = enemy.attackDur * 0.5f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            FlipSprite(enemy.sr.flipX);
            enemy.hand.gameObject.SetActive(true);
            enemy.hand.transform.localPosition = Vector2.zero;
            enemy.hand.flipX = enemy.sr.flipX;

            targetHandPosition = enemy.moveDirection * 1f; // Calculate the target position for the hand movement
            enemy.bufferedState = new SecondPunchState(enemy, targetHandPosition);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = Vector2.zero;
            enemy.hand.transform.localPosition = Vector2.Lerp(Vector2.zero, targetHandPosition, Mathf.Clamp01(1 - age / duration));
        }

        public override void OnExit()
        {
            base.OnExit();
            enemy.hand.gameObject.SetActive(false);

        }
    }

    public class SecondPunchState : EnemyState
    {
        Vector2 targetPos;

        public SecondPunchState(Enemy sm, Vector2 targetPos) : base(sm)
        {
            duration = enemy.attackDur * 0.5f;
            this.targetPos = targetPos;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            enemy.outerhand.gameObject.SetActive(true);
            enemy.outerhand.transform.localPosition = Vector2.zero;
            enemy.outerhand.flipX = enemy.sr.flipX;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = Vector2.zero;

            enemy.outerhand.transform.localPosition = Vector2.Lerp(Vector2.zero, targetPos, Mathf.Clamp01(1 - age / duration));
        }

        public override void OnExit()
        {
            base.OnExit();
            enemy.outerhand.gameObject.SetActive(false);

            // Apply knockback to the player
            Vector3 knockbackDirection = (Player.instance.transform.position - enemy.transform.position).normalized;
            Player.instance.ApplyKnockback(knockbackDirection);

            // Deal damage to the player
            Player.instance.TakeDamage(enemy.damage);
        }
    }

    public class ShootState : EnemyState
    {
        Enemy_3 shootingEnemy;
        private float shootDuration = 0.5f; // Duration of the shooting animation

        public ShootState(Enemy sm) : base(sm)
        {
            shootingEnemy = sm as Enemy_3;
            duration = shootDuration;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // Play shooting animation
            enemy.animator.PlayInFixedTime(Enemy_3.ShootKey);
            shootingEnemy.DoAttack();
            // Spawn a bullet prefab at the shoot point position and rotation

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = Vector2.zero;

        }
    }

    public class ExplodeState : EnemyState
    {
        Enemy_2 explodingEnemy;

        public ExplodeState(Enemy sm) : base(sm)
        {
            explodingEnemy = sm as Enemy_2;
            duration = 0.5f; // The explosion state doesn't have a specific duration
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AudioSource.PlayClipAtPoint(explodingEnemy.explosionSound, enemy.transform.position);

            enemy.rb.simulated = false;
            enemy.animator.PlayInFixedTime(Enemy_2.ExplodeKey);

            // Check if the enemy has collided with the player during the attack sequence
            Collider2D collision = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, LayerMask.GetMask("Player"));
            if (collision != null)
            {
                // Perform explode effect on collision with the player
                Player.instance.ApplyKnockback((Player.instance.transform.position - enemy.transform.position).normalized);
                Player.instance.TakeDamage(enemy.damage);
            }

        }


        public override void OnStateExpired()
        {
            base.OnStateExpired();
            enemy.Despawn();
        }
    }
    public class ChargeExplosion : EnemyState
    {
        private float chargeSpeed = 10f;
        Vector2 targetDir;

        public ChargeExplosion(Enemy sm) : base(sm)
        {
            duration = Vector3.Distance(enemy.transform.position, Player.instance.transform.position) / chargeSpeed;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Play charge animation
            targetDir = (Player.instance.transform.position - enemy.transform.position).normalized;
            enemy.animator.PlayInFixedTime(Enemy.ChargeKey);
            FlipSprite(targetDir.x < 0);

            enemy.bufferedState = new ExplodeState(enemy);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            enemy.rb.velocity = targetDir * chargeSpeed;
            // Move towards the target position at charge speed

        }
        public override void OnExit()
        {
            base.OnExit();
            enemy.rb.velocity = Vector2.zero;
        }
    }
}
