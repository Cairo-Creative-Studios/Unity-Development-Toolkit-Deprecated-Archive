using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CairoEngine
{
    public class Level : MonoBehaviour
    {
        public LevelInfo info;

        /// <summary>
        /// The child instances of the Level.
        /// </summary>
        public List<GameObject> childInstances = new List<GameObject>();

        /// <summary>
        /// Draw this instance.
        /// </summary>
        public void Draw()
        {
            SceneManager.LoadScene(info.sceneName, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(info.sceneName);
            GameObject[] levelObjects = scene.GetRootGameObjects();

            foreach (GameObject levelObject in levelObjects)
            {
                levelObject.transform.parent = transform;
            }
            childInstances.AddRange(levelObjects);
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
