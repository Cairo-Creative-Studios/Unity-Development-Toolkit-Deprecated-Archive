using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "CairoGame/Game Mode")]
    public class GameModeInfo : ScriptableObject
    {
        public string ID = "Default";

        [Header("Customization")]
        [Tooltip("The score to reach to win the Match")]
        public int scoreToWin = 25;
        [Tooltip("The amount of time each Round Lasts")]
        public float roundDuration = 60.0f * 15;
        [Tooltip("The amount of Rounds in the Game.")]
        public int roundCount = 1;

        [Header("Players")]
        [Tooltip("Whether the Game Mode should control the Teams in the Game.")]
        public bool controlTeams = false;
        [Tooltip("The default teams in the Game Mode. If the Game Mode controls Teams, these will be used. Use the Asset Menu/CairoGame menu to create a new Team Template.")]
        public List<TeamInfo> defaultTeams = new List<TeamInfo>();
        [Tooltip("The Default inventory for the Game Mode. Key = Entity Type, Value = List of Inventory Items")]
        public Dictionary<string, List<InventoryItemInfo>> defaultInventory = new Dictionary<string, List<InventoryItemInfo>>();

        [Header("Default Objects")]
        [Tooltip("What Player Controller to use for joining Players if not told otherwise.")]
        public GameObject defaultPlayerControllerPrefab;
        [Tooltip("What AI Controller to use for added AI bots if not told otherwise.")]
        public GameObject defaultAIControllerPrefab;
        [Tooltip("What Pawn Info to use for a Player/AI's Spawned Entity if not told otherwise.")]
        public PawnInfo defaultPawnInfo;

        [Header("Cinematics")]
        [Tooltip("The animation to play at the Beginning of the Game Mode")]
        public TimelineAsset gameStartClip;
        [Tooltip("The Animation to play at the beginnning of the Game Mode")]
        public TimelineAsset gameEndClip;

    }

}
