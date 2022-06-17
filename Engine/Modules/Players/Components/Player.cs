using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// A Player's actual presence in the Game. 
    /// When a Player Joins, they will be added to the Engine's Player list and the Engine will ensure that Player's are given Controllers based on the current Game Mode.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The device assigned to this Player Controller, -1 is Keyboard and Mouse, 0> is Gamepads
        /// </summary>
        public int device = -1;
        /// <summary>
        /// The Player's Profile Name for Data Management.
        /// </summary>
        public string Profile = "";
        /// <summary>
        /// The Controller the Player is using (Usually Assigned through the Engine's Game Mode functionality.
        /// </summary>
        public PlayerController playerController;

        public Player(PlayerController playerController)
        {
            this.playerController = playerController;
        }
    }
}
