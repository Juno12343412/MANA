using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDBase.Utils
{
    public static class TimeUtils {

        public static IEnumerator TimeStop(float t) {

            Time.timeScale = 0.1f;
            float progress = 0f;
            yield return null;

            while (progress <= t) {

                progress += Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = 1f;
        }
    }
}
