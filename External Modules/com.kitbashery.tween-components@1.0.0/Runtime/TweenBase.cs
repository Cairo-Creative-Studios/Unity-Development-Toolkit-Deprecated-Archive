using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    /// Base class for all tween components.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-base.html")]
    [AddComponentMenu("Kitbashery/Tween/TweenBase.cs")]
    public class TweenBase : MonoBehaviour
    {
        #region Variables:

        [field: SerializeField, Header("Tween Settings:")]
        public bool tweening { get; set; } = true;

        [Tooltip("Check this if you are tweening a rigidbody.")]
        public bool useFixedUpdate = false;
        /// <summary>
        /// The starting state of tweening.
        /// </summary>
        [HideInInspector]
        public bool tweeningAtStart;

        [Tooltip("Only tween if the render is visible to the main camera.")]
        public bool tweenWhenVisible = false;
        [Tooltip("The renderer that if visible allows movement. (also used in material tweening).")]
        public Renderer rend;
        [HideInInspector]
        public Camera visibilityCam;

        [Tooltip("Time in seconds untill the tween starts playing.")]
        [Min(0)]
        public float startDelay = 0;
        [HideInInspector]
        public float lerpTime = 0;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public WrapMode wrapMode = WrapMode.PingPong;
        public UnityEvent onTweenUpdate;
        [Header("Called each time a tween reaches its current target.")]
        public UnityEvent onTweenComplete;

        #endregion

        #region Initialization & Updates:

        private void OnValidate()
        {
            curve.preWrapMode = wrapMode;
            curve.postWrapMode = wrapMode;
        }

        private void Awake()
        {
            if (TweenManager.Instance == null)
            {
                TweenManager.Instance = new GameObject("Tween Manager").AddComponent<TweenManager>();
                Debug.LogWarning("TweenManager instance not found, creating one...");
            }

            if (startDelay > 0)
            {
                DelayTween(startDelay);
                tweeningAtStart = false;
            }
            else
            {
                tweeningAtStart = tweening;
            }

            if(visibilityCam == null)
            {
                visibilityCam = Camera.main;
            }
        }

        void OnEnable()
        {
            if(TweenManager.Instance != null)
            {
                TweenManager.Instance.Register(this);
            }
        }

        void OnDisable()
        {
            if (TweenManager.Instance != null)
            {
                TweenManager.Instance.Unregister(this);
            }
        }

        #endregion]

        #region Methods:

        public virtual void UpdateTween() { onTweenUpdate.Invoke(); }

        /// <summary>
        /// Disables movement untill <paramref name="time"/> has elapsed.
        /// </summary>
        /// <param name="time">The time to delay movement.</param>
        public void DelayTween(float time)
        {
            StartCoroutine(Delay(time));
        }

        private IEnumerator Delay(float time)
        {
            tweening = false;
            yield return new WaitForSeconds(time);
            tweening = true;
        }

        /// <summary>
        /// Sets the tweens state back to its initial state with a delay if it has one.
        /// </summary>
        public void ResetTweenState()
        {
            if (startDelay > 0)
            {
                DelayTween(startDelay);
            }
            else
            {
                tweening = tweeningAtStart;
            }
        }

        public bool IsVisibleToMainCamera()
        {
            if (rend != null)
            {
                return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(visibilityCam), rend.bounds);
            }
            else
            {
                Debug.LogWarning("A tween component has moveOnlyInCameraViuew checked but has no Renderer specified. It will not move.", gameObject);
                return false;
            }
        }

        public void DebugRootDirection()
        {
            Transform root = transform.root;
            if ((root.rotation.eulerAngles.y > 315 && root.rotation.eulerAngles.y < 360) || (root.rotation.eulerAngles.y > 0 && root.rotation.eulerAngles.y < 45))
            {
                Debug.Log("|Tween Base|: Root transform facing forward.", gameObject);
            }
            else if (root.rotation.eulerAngles.y > 45 && root.rotation.eulerAngles.y < 135)
            {
                Debug.Log("|Tween Base|: Root transform facing right.", gameObject);
            }
            else if (root.rotation.eulerAngles.y > 135 && root.rotation.eulerAngles.y < 225)
            {
                Debug.Log("|Tween Base|: Root transform facing back", gameObject);
            }
            else if (root.rotation.eulerAngles.y > 225 && root.rotation.eulerAngles.y < 315)
            {
                Debug.Log("|Tween Base|: Root transform facing left.", gameObject);
            }
        }

        /// <summary>
        /// Reverses <see cref="curve"/>.
        /// </summary>
        public void ReverseCurve()
        {
            Keyframe[] keys = curve.keys;
            WrapMode postWrapmode = curve.postWrapMode;
            curve.postWrapMode = curve.preWrapMode;
            curve.preWrapMode = postWrapmode;
            for (int i = 0; i < keys.Length; i++)
            {
                Keyframe key = keys[i];
                key.time = -key.time;
                key.inTangent = -key.outTangent;
                key.outTangent = -key.inTangent;
                keys[i] = key;
            }
            curve.keys = keys;
        }


        /// <summary>
        /// Sets the tween to use a linear animation curve.
        /// </summary>
        public void LinearCurve()
        {
            curve = AnimationCurve.Linear(0, 0, 1, 1);
            curve.preWrapMode = wrapMode;
            curve.postWrapMode = wrapMode;
        }

        /// <summary>
        /// Sets the tween to use a constant flat animation curve.
        /// </summary>
        public void FlattenCurve(float value)
        {
            curve = AnimationCurve.Constant(0, 1, value);
            curve.preWrapMode = wrapMode;
            curve.postWrapMode = wrapMode;
        }

        #endregion
    }
}