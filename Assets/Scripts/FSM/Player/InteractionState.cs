using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;
using Characters;

namespace FiniteStateMachine.Player
{

    [CreateAssetMenu(fileName = NAME, menuName = "FSM/Player/"+ NAME)]
    public class InteractionState : FSMState
    {
        public const string NAME = "Interaction State";

        private IActor _actor;


        public override bool StartState(Dictionary<string, object> data)
        {
            _actor = (IActor)data["actor"];
            _actor.GetActorComponent<IInteractor>(0).GetInteractable().Interact(new Dictionary<string, object>() { { "actor", _actor } });

            return true;
        }

        public override void UpdateState(Dictionary<string, object> data)
        {

        }
    }
}