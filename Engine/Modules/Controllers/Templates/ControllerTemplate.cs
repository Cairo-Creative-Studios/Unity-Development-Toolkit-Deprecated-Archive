//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using B83.Unity.Attributes;

namespace CairoEngine.Controllers
{
    [CreateAssetMenu(menuName = "Cairo Game/Gameplay/Controller")]
    public class ControllerTemplate : ScriptableObject
    {
        [Tooltip("")]
        public string ID = "";
        [Tooltip("If the Controller is a Team Player, it will be used on the Score Board, disable to control pawns that don't score.")]
        public bool teamPlayer = false;
        [Tooltip("The Inputs belonging to the Controller")]
        public InputMap inputMap;
        [Tooltip("Whether to create the Player Controller Game Object when the Game Starts")]
        public bool startWithGame = true;

    }
}
