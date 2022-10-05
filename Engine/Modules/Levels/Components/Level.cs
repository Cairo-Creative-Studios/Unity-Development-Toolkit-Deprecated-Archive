//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UDT
{
    public class Level : MonoBehaviour
    {
        public LevelTemplate template;
        /// <summary>
        /// Draw this instance.
        /// </summary>
        public void Draw()
        {
            SceneManager.LoadScene(template.sceneName, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(template.sceneName);
            GameObject[] levelObjects = scene.GetRootGameObjects();
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
