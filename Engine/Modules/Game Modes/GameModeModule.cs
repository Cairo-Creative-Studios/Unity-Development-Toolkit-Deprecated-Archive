using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace CairoEngine
{
    /// <summary>
    /// Controls the Game Mode that is used to Play the Game. Using the Game Mode Module isn't necessary for building Gameplay, but it vastly simplifies development of Game Mode centric Game development.
    /// </summary>
    public class GameModeModule
    {

        /// <summary>
        /// The Game Modes present in the Game.
        /// </summary>
        private static List<GameModeTemplate> gameModeInfos = new List<GameModeTemplate>();

        /// <summary>
        /// The Game Mode information to use for the Current Game Mode
        /// </summary>
        public static GameModeTemplate gameMode;

        /// <summary>
        /// Whether the creator of the Game has initialized the first Game Mode.
        /// </summary>
        public static bool firstGameModeSet = false;
        /// <summary>
        /// The custom settings applied to the Game Mode by the User.
        /// </summary>
        public static Dictionary<string, object> userSettings = new Dictionary<string, object>();
        /// <summary>
        /// Each Team's Score in the current Round is tallied in this Dictionary. 
        /// </summary>
        public static Dictionary<Team, int> teamScores = new Dictionary<Team, int>();
        /// <summary>
        /// When a Team wins a Round, they're added to the roundsWon List, and the amount of Times <see langword="async"/> Team has won is tallied at the end.
        /// </summary>
        public static List<Team> roundsWon = new List<Team>();

        public static TimelineAsset currentCinematic;
        /// <summary>
        /// Time since the Game Mode Started (Not including Cinematics)
        /// </summary>
        public static float time;

        public static void Init()
        {
            gameModeInfos.AddRange(Resources.LoadAll<GameModeTemplate>(""));
        }

        /// <summary>
        /// Updates information and determines Win/Loss information of Rounds or the Whole Game.
        /// </summary>
        public static void Update()
        {
            //Initialzie the Game Mode Module with the Template Game Mode Info
            if (!firstGameModeSet)
                if (gameMode == null)
                    gameMode = ScriptableObject.CreateInstance<GameModeTemplate>();
            

            if (currentCinematic == null)
            {
                if (gameMode == null)
                    gameMode = ScriptableObject.CreateInstance<GameModeTemplate>();

                //Add to Game Duration Time
                time += Time.deltaTime;

                //Check if a Team has met the Score to Win.
                foreach (Team team in teamScores.Keys)
                {
                    if (teamScores[team] >= gameMode.scoreToWin)
                    {
                        HandleVictory(team);
                        return;
                    }
                }

                //Handle Time Out (The Case of a Time out can result in a Draw)
                if (time > gameMode.roundDuration && gameMode.roundDuration > 0)
                {
                    int highestScore = -1;

                    List<Team> winningTeams = new List<Team>();

                    foreach (Team team in teamScores.Keys)
                    {
                        if (teamScores[team] > highestScore)
                        {
                            winningTeams.Clear();
                            highestScore = teamScores[team];
                            winningTeams.Add(team);
                        }
                        if (teamScores[team] == highestScore)
                        {
                            winningTeams.Add(team);
                        }
                    }

                    foreach (Team team in winningTeams)
                    {
                        HandleVictory(team);
                    }
                }
            }
        }

        //TODO: Finish Game Mode code to handle Spawning, Scoring, and Win/Loss conditions.
        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void StartGame(string gameModeName)
        {
            gameMode = FindGameModeInfo(gameModeName);
            if (gameMode != null)
            {
                Debug.Log("Requested Game Mode " + gameModeName + " was found and set.");
                firstGameModeSet = true;

                //Play the Game Mode's Openning Cinematic.
                if (gameMode.gameStartClip != null)
                {
                    currentCinematic = gameMode.gameStartClip;
                }

                EntityModule.SpawnPawn(gameMode.defaultPawnInfo, LevelModule.spawnPoints[0].transform.position);
            }
            else
                Debug.LogWarning("The requested Game Mode: " + gameModeName + " does not exist.");
        }

        private static void HandleStartSpawn()
        {

        }

        public static void SpawnDefaultPawn()
        {
            EntityModule.SpawnPawn(gameMode.defaultPawnInfo, LevelModule.spawnPoints[0].transform.position);

        }

        /// <summary>
        /// Forces the Game to end.
        /// </summary>
        public static void ForceQuit()
        {

        }

        /// <summary>
        /// A team was Victorious!
        /// </summary>
        public static void HandleVictory(Team winningTeam)
        {

        }

        /// <summary>
        /// Ends the Current Game.
        /// </summary>
        public static void EndGame()
        {
            GameModeModule.ForceQuit();
        }

        /// <summary>
        /// Adds points to the Team's score 
        /// </summary>
        /// <param name="team">The Team to give points to</param>
        /// <param name="points">The Points to give.</param>
        public static void TeamScore(Team team, int points)
        {
            GameModeModule.teamScores[team] += points;
        }


        /// <summary>
        /// Finds the Game Mode by ID
        /// </summary>
        /// <param name="name">Name.</param>
        private static GameModeTemplate FindGameModeInfo(string name)
        {
            foreach (GameModeTemplate gameModeInfo in gameModeInfos)
            {
                if (gameModeInfo.ID == name)
                {
                    return gameModeInfo;
                }
            }
            return null;
        }
    }
}
