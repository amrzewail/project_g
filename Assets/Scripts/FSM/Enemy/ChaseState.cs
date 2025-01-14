using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;
using Characters;

namespace FiniteStateMachine.Enemy
{

    [CreateAssetMenu(fileName = NAME, menuName = "FSM/Enemy/" + NAME)]
    public class ChaseState : FSMState
    {
        public const string NAME = "Chase State";

        private IActor _actor;
        private IMover _mover;
        public float chaseSpeed = 5;

        public override bool StartState(Dictionary<string, object> data)
        {
            _actor = (IActor)data["actor"];
            _mover = _actor.GetActorComponent<IMover>(0);

            return true;
        }

        public override void UpdateState(Dictionary<string, object> data)
        {

            _mover.Move(_actor.GetActorComponent<ITargeter>(0).GetTarget().targetPosition, chaseSpeed);
        }
    }
}