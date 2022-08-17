//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CairoEngine
{
    public class Level : MonoBehaviour
    {
        public LevelTemplate template;

        /// <summary>
        /// The child instances of the Level.
        /// </summary>
        public List<CObject> childInstances = new List<CObject>();

        /// <summary>
        /// Draw this instance.
        /// </summary>
        public void Draw()
        {
            SceneManager.LoadScene(template.sceneName, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(template.sceneName);
            GameObject[] levelObjects = scene.GetRootGameObjects();

            foreach (GameObject levelObject in levelObjects)
            {
                levelObject.transform.parent = transform;

                CObject levelCObject = levelObject.GetComponent<CObject>();
                if(levelCObject!=null)
                    childInstances.Add(levelCObject);
            }
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
