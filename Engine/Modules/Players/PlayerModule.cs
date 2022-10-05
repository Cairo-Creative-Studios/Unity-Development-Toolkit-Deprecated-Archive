/*! \addtogroup playermodule Player Module
 *  Additional documentation for group 'Player Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT
{
    /// <summary>
    /// The Player Module controls functionality related to each Player's presence in the game, and ensuring that Players are given appropriate Player Controllers.
    /// </summary>
    public class PlayerModule
    {
        /// <summary>
        /// The players.
        /// </summary>
        private static List<Player> players = new List<Player>();
        /// <summary>
        /// The teams that currently exist in the game.
        /// </summary>
        private static List<Team> teams = new List<Team>();


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
