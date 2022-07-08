//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using B83.Unity.Attributes;
using UnityEngine.InputSystem;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Gameplay/Controller")]
    public class ControllerTemplate : Resource
    {
        [Tooltip("If the Controller is a Team Player, it will be used on the Score Board, disable to control pawns that don't score.")]
        public bool teamPlayer = false;
        [Tooltip("The Controller's Inputs. If the Controller is an AI Controller, it will interface Network Outputs to this Dictionary.")]
        public SDictionary<string, InputAction> inputs = new SDictionary<string, InputAction>();
    }
}
