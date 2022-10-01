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
    /// Rotates the transform so its forward direction faces <see cref="lookAtTarget"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-billboard.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Billboard")]
    public class TweenBillboard : TweenBase
    {
        [Header("Billboard:")]
        [Tooltip("The transform to look at, leave blank to default to main camera.")]
        public Transform lookAtTarget;
        public bool horizontalAxis = false;
        public bool directLook = true;

        /// <summary>
        /// The position the billboard will look at.
        /// </summary>
        [HideInInspector]
        public Vector3 lookAtPos;

        private Vector3 currentPosition;
        private Quaternion currentRotation;

        private void OnDrawGizmosSelected()
        {
            if(Application.isPlaying == true)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.forward);
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.forward, lookAtPos);
                Gizmos.DrawLine(transform.position, lookAtPos);
            }
        }

        public override void UpdateTween()
        {
            if(lookAtTarget != null)
            {
                if (lookAtPos != lookAtTarget.position || currentPosition != transform.position || transform.rotation != currentRotation)
                {
                    if (directLook == true)
                    {
                        lookAtPos = 2 * transform.position - lookAtTarget.position;
                        if (horizontalAxis == true)
                        {
                            lookAtPos.y = transform.position.y;
                        }
                    }
                    else
                    {
                        if (horizontalAxis == false)
                        {
                            lookAtPos = lookAtTarget.position;
                        }
                        else
                        {
                            lookAtPos = new Vector3(lookAtTarget.position.x, transform.position.y, lookAtTarget.position.z);
                        }
                    }
                    onTweenUpdate.Invoke();
                    transform.LookAt(lookAtPos);
                    currentPosition = transform.position;
                    onTweenComplete.Invoke();
                }
            }
            else
            {
                lookAtTarget = Camera.main.transform;
            }        
        }
    }
}