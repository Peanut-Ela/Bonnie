using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPCStates
{

    public class NPCState : BaseState
    {
        // States to make:
        // idle
        // wander - move in random direction over a period
        // eat
        // 
        protected NPC npc;
        public NPCState(NPC sm) : base(sm)
        {
            npc = sm;
        }
        public override void Update()
        {
            base.Update();


        }
    }
    public class IdleState : NPCState
    {
        public IdleState(NPC sm) : base(sm)
        {
            duration = Random.Range(npc.idleDurationMin, npc.idleDurationMax);
        }
        public override void OnEnter()
        {
            base.OnEnter();
            npc.animator.PlayInFixedTime(NPC.IdleKeys[Random.Range(0, NPC.IdleKeys.Length)]);
            //npc.animator.PlayInFixedTime(NPC.IdleKey);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            npc.rb.velocity = Vector2.zero;
        }
        public override void OnStateExpired()
        {
            base.OnStateExpired();
            npc.QueueState(new WanderState(npc));
        }
    }
    public class WanderState : NPCState
    {
        Vector2 walkDir;
        public WanderState(NPC sm) : base(sm)
        {
            duration = 1;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            npc.animator.PlayInFixedTime(NPC.WalkKey);
            walkDir = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
            if ((npc.sr.flipX && walkDir.x > 0) || (!npc.sr.flipX && walkDir.x < 0))
                npc.sr.flipX = !npc.sr.flipX;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            npc.rb.velocity = walkDir * npc.moveSpeed;
        }
    }
}
