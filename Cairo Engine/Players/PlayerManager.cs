using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// The Player Manager controls functionality related to each Player's presence in the game, and ensuring that Players are given appropriate Player Controllers.
    /// </summary>
    public class PlayerManager
    {
        /// <summary>
        /// The players.
        /// </summary>
        private static List<Player> players = new List<Player>();
        /// <summary>
        /// The teams that currently exist in the game.
        /// </summary>
        private static List<Team> teams = new List<Team>();

        public static void Init()
        {

        }

        public static void Update()
        {

        }

        #region Players
        /// <summary>
        /// Adds a Player to the Game.
        /// </summary>
        public static void AddPlayer()
        {
            players.Add(new Player(ControllerManager.CreatePlayerController(-1)));
        }

        public static void SetPlayerControllers(PlayerController playerController)
        {
            foreach (Player player in players)
            {
                player.playerController = playerController;
            }
        }

        /// <summary>
        /// Sets the Player Controller for one Player. Use this if some Players need to use a different kind of Controller.
        /// </summary>
        /// <param name="player">The Player</param>
        /// <param name="playerController">Player controller.</param>
        public static void SetPlayerControllerForOne(int playerID, PlayerController playerController)
        {
            players[playerID].playerController = playerController;
        }

        #endregion

        #region Teams
        /// <summary>
        /// Creates a team in the Game.
        /// </summary>
        /// <param name="teamName">Team name.</param>
        /// <param name="teamColor">Team color.</param>
        public static void CreateTeam(string teamName, Color teamColor)
        {
            teams.Add(new Team(teams.Count, teamName, teamColor));
        }
        #endregion
    }
}
