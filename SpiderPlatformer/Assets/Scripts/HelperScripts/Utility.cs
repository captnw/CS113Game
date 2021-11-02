using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderPlatformer
{
    public static class Utility
    {
        // code from: https://forum.unity.com/threads/whats-the-best-way-to-rotate-a-vector2-in-unity.729605/
        public static Vector2 rotate(in Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }

        public static void fallfaster(ref Vector2 velocity, float fallMultiplier)
        {
            // If we're falling, fall faster over time
            // Code snippet is from: https://www.youtube.com/watch?v=7KiK0Aqtmzc
            if (velocity.y < 0)
            {
                velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}