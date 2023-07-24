using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPCStates
{
    public class NPCState : BaseState
    {
        protected NPC npc;
        protected bool requestedTalk = false;

        public NPCState(NPC sm) : base(sm)
        {
            npc = sm;
        }

        public override void Update()
        {
            base.Update();

            if (npc.playerIsClose && Input.GetKeyDown(KeyCode.E))
                requestedTalk = true;
        }

        public override void StateUpdate()
        {
            base.StateUpdate();

            if (requestedTalk)
            {
                npc.QueueState(new DialogueState(npc));
                npc.rb.velocity = Vector2.zero; // Stop NPC from moving during dialogue
            }
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
        }

        public override void Update()
        {
            base.Update();
            npc.visualCue.SetActive(npc.playerIsClose);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            npc.rb.velocity = npc.playerIsClose ? Vector2.zero : npc.moveDirection * npc.moveSpeed;
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

        public override void Update()
        {
            base.Update();
            npc.visualCue.SetActive(npc.playerIsClose);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            npc.rb.velocity = walkDir * npc.moveSpeed;
        }
    }

    public class DialogueState : NPCState
    {

        //private readonly NPC npc;
        public DialogueState(NPC sm) : base(sm)
        {
            this.npc = sm;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            npc.animator.PlayInFixedTime(NPC.IdleKey);
            npc.visualCue.SetActive(false);
            DialogueManager.instance.dialoguePanel.SetActive(true);
            DialogueManager.instance.currentDialogue = DialogueManager.GetDialogueID(npc.openingDialogueId);
            DialogueManager.instance.StartTyping(npc);
        }

        public override void Update()
        {
            base.Update();

            if (!npc.playerIsClose)
            {
                DialogueManager.instance.zeroText();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            npc.rb.velocity = Vector2.zero;
        }
    }
}

