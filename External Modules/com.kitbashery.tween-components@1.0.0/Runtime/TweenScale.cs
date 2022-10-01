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
    /// Scales to a destination scale following a <see cref="AnimationCurve"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-scale.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Scale")]
    public class TweenScale : TweenBase
    {
        [Header("Scale:")]
        public Vector3 target;
        public bool snappyScale = false;

        /// <summary>
        /// The next scale the transform will be at (usefull for networking/physics).
        /// </summary>
        [HideInInspector]
        public Vector3 nextScale;

        private Vector3 initialScale;
        private Vector3 initialTarget;
        private Vector3 modifiedTarget;
        private Vector3 currentTarget;

        private Renderer gizmoRend;

        void Start()
        {
            initialScale = transform.localScale;
            initialTarget = target;
            nextScale = initialScale;
            currentTarget = target;
        }

        private void OnDrawGizmosSelected()
        {
            if(Application.isPlaying == false)
            {
                if (gizmoRend == null)
                {
                    gizmoRend = GetComponent<Renderer>();
                }
                else
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireCube(transform.position, new Vector3(gizmoRend.bounds.size.x * target.x, gizmoRend.bounds.size.y * target.y, gizmoRend.bounds.size.z * target.z));
                }
            }
        }

        public override void UpdateTween()
        {
            if(target != initialTarget)
            {
                UpdateTarget();
            }

            if (lerpTime < curve.keys[curve.length - 1].time)
            {
                lerpTime += Time.deltaTime;
                nextScale = Vector3.Lerp(transform.localScale, currentTarget, curve.Evaluate(lerpTime));
                if (snappyScale == true)
                {
                    nextScale = new Vector3(Mathf.RoundToInt(nextScale.x), Mathf.RoundToInt(nextScale.y), Mathf.RoundToInt(nextScale.z));
                }
                transform.localScale = nextScale;
                onTweenUpdate.Invoke();
            }
            else
            {
                switch (wrapMode)
                {
                    case WrapMode.PingPong:

                        if (currentTarget == initialScale)
                        {
                            currentTarget = target;
                        }
                        else
                        {
                            currentTarget = initialScale;
                        }
                        lerpTime = 0;

                        break;


                    case WrapMode.Loop:

                        transform.localScale = initialScale;
                        lerpTime = 0;

                        break;


                    case WrapMode.Once:

                        transform.localScale = initialScale;
                        tweening = false;

                        break;

                    case WrapMode.ClampForever:

                        transform.localScale = currentTarget;
                        tweening = false;

                        break;

                    case WrapMode.Default:

                        lerpTime = 0;

                        break;

                    default:

                        lerpTime = 0;

                        break;
                }

                onTweenComplete.Invoke();
            }
        }

        private void UpdateTarget()
        {
            modifiedTarget = initialScale + target;
            if (currentTarget != initialScale)
            {
                currentTarget = modifiedTarget;
            }
        }

        public void ScaleToTarget()
        {
            currentTarget = target;
            tweening = true;
        }

        public void ScaleToInitialScale()
        {
            currentTarget = initialScale;
            tweening = true;
        }

        public void ResetToInitial()
        {
            transform.localScale = initialScale;
            target = initialTarget;
            nextScale = initialScale;
            currentTarget = target;
            tweening = tweeningAtStart;
            ResetTweenState();
        }
    }
}