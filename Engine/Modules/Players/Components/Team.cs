//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    /// <summary>
    /// Teams can represent individual teams in a Multiplayer game, or the Player's friends and Enemies in singleplayer.
    /// </summary>
    public class Team
    {
        /// <summary>
        /// The team identifier.
        /// </summary>
        public int teamID = 0;
        /// <summary>
        /// The name of the team.
        /// </summary>
        public string teamName = "";
        /// <summary>
        /// The color of the team, used for representation in-game.
        /// </summary>
        public Color teamColor = new Color(1.0f, 0.0f, 0.0f);
        /// <summary>
        /// The Controllers given to the Members of the Teams.
        /// </summary>
        //public List<Controller> members = new List<Controller>();
        /// <summary>
        /// The friendly teams according to this Team, can be used to form temporary alliances between different Teams.
        /// </summary>
        public List<Team> friendlyTeams = new List<Team>();

        /// <summary>
        /// Initializes a new instance of a <see cref="T:Team"/> class.
        /// </summary>
        /// <param name="ID">Identifier.</param>
        /// <param name="teamName">Team name.</param>
        /// <param name="teamColor">Team color.</param>
        public Team(int ID, string teamName, Color teamColor)
        {
            this.teamID = ID;
            this.teamName = teamName;
            this.teamColor = teamColor;
        }
    }
}
