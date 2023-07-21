using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerStates
{

    public class PlayerState : BaseState
    {
        protected Player player;
        public PlayerState(Player sm) : base(sm)
        {
            player = sm;
        }
    }
    public class BaseMovementState : PlayerState
    {
        bool requestedAttack;
        bool requestedDash;

        public BaseMovementState(Player sm) : base(sm) { }

        public override void Update()
        {
            base.Update();

            player.moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


            if (player.moveDirection != Vector2.zero)
            {
                player.lastAnimDir = player.moveDirection;

                if (player.lastAnimDir.x != 0 && player.lastAnimDir.y != 0)
                    player.lastAnimDir.y = 0; // prioritize horizontal anim over vertical anim

                player.animator.SetFloat(Player.HorizontalParameterKey, player.lastAnimDir.x);
                player.animator.SetFloat(Player.VerticalParameterKey, player.lastAnimDir.y);
            }

            if (Input.GetKeyDown(KeyCode.Space))
                requestedAttack = true;
            if (Input.GetKeyDown(KeyCode.LeftControl) && player.currentDashCooldown <= 0)
                requestedDash = true;

            player.currentDashCooldown -= Time.deltaTime;
        }

        public override void StateUpdate()
        {
            base.StateUpdate();

            if (requestedAttack)
                player.QueueState(new AttackState(player));
            if (requestedDash)
                player.QueueState(new DashState(player));
        }
    }
    public class IdleState : BaseMovementState
    {
        bool requestedMove;
        public IdleState(Player sm) : base(sm) { }
        public override void OnEnter()
        {
            base.OnEnter();
            player.animator.PlayInFixedTime(Player.IdleKey);
        }
        public override void Update()
        {
            base.Update();
            if (player.moveDirection != Vector2.zero)
                requestedMove = true;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            player.rb.velocity = Vector2.zero;
        }
        public override void StateUpdate()
        {
            base.StateUpdate();
            if (requestedMove)
                player.QueueState(new WalkState(player));
        }
    }
    public class WalkState : BaseMovementState
    {
        bool requestedRun;
        bool requestedStop;
        public WalkState(Player sm) : base(sm) { }
        public override void OnEnter()
        {
            base.OnEnter();
            player.animator.PlayInFixedTime(Player.WalkKey);
        }
        public override void Update()
        {
            base.Update();
            if (player.InputRun)
                requestedRun = true;
            if (player.moveDirection == Vector2.zero)
                requestedStop = true;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            player.rb.velocity = player.walkSpeed * player.moveDirection.normalized;
        }
        public override void StateUpdate()
        {
            base.StateUpdate();
            if (requestedRun)
                player.QueueState(new RunState(player));
            if (requestedStop)
                player.QueueState(new IdleState(player));
        }
    }
    public class RunState : BaseMovementState
    {
        bool requestedWalk;
        bool requestedStop;
        public RunState(Player sm) : base(sm) { }
        public override void OnEnter()
        {
            base.OnEnter();
            player.animator.PlayInFixedTime(Player.RunKey);
        }
        public override void Update()
        {
            base.Update();
            if (!player.InputRun)
                requestedWalk = true;
            if (player.moveDirection == Vector2.zero)
                requestedStop = true;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            player.rb.velocity = player.runSpeed * player.moveDirection.normalized;
        }
        public override void StateUpdate()
        {
            base.StateUpdate();
            if (requestedWalk)
                player.QueueState(new WalkState(player));
            if (requestedStop)
                player.QueueState(new IdleState(player));
        }
    }
    public class BaseAbilityState : PlayerState
    {
        public BaseAbilityState(Player sm) : base(sm) { }
    }
    public class AttackState : BaseAbilityState
    {
        bool hitboxActivated;
        public AttackState(Player sm) : base(sm)
        {
            duration = player.attackDuration;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            player.animator.PlayInFixedTime(Player.AttackKey);
            player.weaponHitbox.transform.localPosition = player.lastAnimDir;
        }
        public override void Update()
        {
            base.Update();
            if (_elapsedTime > player.attackWindupDuration && !hitboxActivated)
            {
                hitboxActivated = true;
                player.weaponHitbox.gameObject.SetActive(true);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            player.rb.velocity = Vector2.zero;
        }
        public override void OnExit()
        {
            base.OnExit();
            player.weaponHitbox.gameObject.SetActive(false);
        }
    }
    public class DashState : BaseAbilityState
    {
        Vector2 dashDir;
        public DashState(Player sm) : base(sm)
        {
            duration = player.dashDuration;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            player.animator.PlayInFixedTime(Player.RunKey);
            if (player.moveDirection == Vector2.zero)
                dashDir = player.lastAnimDir;
            else
                dashDir = player.moveDirection.normalized;
            player.StartCoroutine(player.SpawnGhost());
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            player.rb.velocity = player.dashSpeed * dashDir;
        }
        public override void OnExit()
        {
            base.OnExit();
            player.StopAllCoroutines();
            player.currentDashCooldown = player.dashCoolDown;
        }
    }
}