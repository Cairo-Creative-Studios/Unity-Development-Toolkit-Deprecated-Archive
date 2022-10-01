using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 MIT License

Copyright (c) 2022 Kitbashery

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.



Need support or additional features? Please visit https://kitbashery.com/
*/

namespace Kitbashery.Tween
{
    /// <summary>
    /// Consolidates <see cref="TweenBase"/> update loops improving performance.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-manager.html")]
    [DefaultExecutionOrder(-20)]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Manager")]
    public class TweenManager : MonoBehaviour
    {
        public static TweenManager Instance;

        [field: SerializeField, Tooltip("Pauses all tweens.")]
        public bool pauseTweens { get; set; } = false;

        [HideInInspector]
        public List<TweenBase> tweens = new List<TweenBase>();

        #region Initialization & Updates:

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (pauseTweens == false)
            {
                for (int i = 0; i < tweens.Count; ++i)
                {
                    if (tweens[i].tweening == true && tweens[i].useFixedUpdate == false)
                    {
                        Move(tweens[i]);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (pauseTweens == false)
            {
                for (int i = 0; i < tweens.Count; ++i)
                {
                    if (tweens[i].tweening == true && tweens[i].useFixedUpdate == true)
                    {
                        Move(tweens[i]);
                    }
                }
            }
        }

        #endregion

        private void Move(TweenBase tween)
        {
            if (tween.tweenWhenVisible == true)
            {
                if (tween.IsVisibleToMainCamera() == true)
                {
                    tween.UpdateTween();
                }
            }
            else
            {
                tween.UpdateTween();
            }
        }

        public void Register(TweenBase tween)
        {
            tweens.Add(tween);
        }

        public void Unregister(TweenBase tween)
        {
            tweens.Remove(tween);
        }
    }
}