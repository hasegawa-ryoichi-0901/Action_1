using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine;

#if CLIENT
using DG.Tweening;
using UnityEngine;
using MathLib = UnityEngine.Mathf;
#endif

#if SERVER || SHARE||TEST
using MathLib = System.Math;
#endif

namespace yv.shared {
    public static class EasingUtils {
        public enum EasingType {
            easeLiner = 0,

            easeInSine = 1,
            easeInQuad = 2,
            easeInCubic = 3,
            easeInQuart = 4,
            easeInBack = 5,

            easeOutSine = 6,
            easeOutQuad = 7,
            easeOutCubic = 8,
            easeOutQuart = 9,
            easeOutBack = 10,
            
            easeInExpo = 11,
            easeOutExpo = 12,
            
            easeInOutSine = 21,
        }
#if CLIENT
        public static Dictionary<EasingType, Ease> EaseTypeDic = new Dictionary<EasingType, Ease>() {
            {EasingType.easeLiner, Ease.Linear},
            {EasingType.easeInSine, Ease.InSine},
            {EasingType.easeInQuad, Ease.InQuad},
            {EasingType.easeInCubic, Ease.InCubic},
            {EasingType.easeInQuart, Ease.InQuart},
            {EasingType.easeInBack, Ease.InBack},
            {EasingType.easeOutSine, Ease.OutSine},
            {EasingType.easeOutQuad, Ease.OutQuad},
            {EasingType.easeOutCubic, Ease.OutCubic},
            {EasingType.easeOutQuart, Ease.OutQuart},
            {EasingType.easeOutBack, Ease.OutBack},
            {EasingType.easeInExpo, Ease.InExpo},
            {EasingType.easeOutExpo, Ease.OutExpo},
            {EasingType.easeInOutSine, Ease.InOutSine},
        };
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInSine(float t) { return 1.0f - (float)Math.Cos((t * Math.PI) / 2.0f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInQuad(float t) { return t * t; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInCubic(float t) { return t * t * t; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInQuart(float t) { return t * t * t * t; }

        private static float c1 = 1.70158f;
        private static float c3 = c1 + 1.0f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInBack(float t) { return c3 * t * t * t - c1 * t * t; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseOutSine(float t) { return (float)Math.Sin((t * Math.PI) / 2.0f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseOutQuad(float t) { return 1.0f - (1.0f - t) * (1.0f - t); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseOutCubic(float t) { return 1.0f - (float)Math.Pow(1.0f - t, 3.0f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseOutQuart(float t) { return 1.0f - (float)Math.Pow(1.0f - t, 4.0f); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseOutBack(float t) {
            return 1.0f + c3 * (float)Math.Pow(t - 1.0f, 3.0f) + c1 * (float)Math.Pow(t - 1.0f, 2.0f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInExpo(float t) {
            return t == 0.0f ? 0.0f : (float)Math.Pow(2.0f, 10.0f * t - 10.0f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseOutExpo(float t) {
            return t == 1.0f ? 1.0f : 1.0f - (float)Math.Pow(2.0f, -10.0f * t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPosEaseInOutSine(float t) {
            return (float)-(Math.Cos(Math.PI * t) - 1) / 2;
        }
        public static float GetPos (EasingUtils.EasingType easeType, float t) {
            switch (easeType) {
                case EasingUtils.EasingType.easeLiner:
                    return t;
                case EasingUtils.EasingType.easeInSine:
                    return GetPosEaseInSine(t);
                case EasingUtils.EasingType.easeOutSine:
                    return GetPosEaseOutSine(t);
                
                case EasingUtils.EasingType.easeInQuad:
                    return GetPosEaseInQuad(t);
                case EasingUtils.EasingType.easeOutQuad:
                    return GetPosEaseOutQuad(t);
                
                case EasingUtils.EasingType.easeInCubic:
                    return GetPosEaseInCubic(t);
                case EasingUtils.EasingType.easeOutCubic:
                    return GetPosEaseOutCubic(t);
                
                case EasingUtils.EasingType.easeInQuart:
                    return GetPosEaseInQuart(t);
                case EasingUtils.EasingType.easeOutQuart:
                    return GetPosEaseOutQuart(t);
                
                case EasingUtils.EasingType.easeInBack:
                    return GetPosEaseInBack(t);
                case EasingUtils.EasingType.easeOutBack:
                    return GetPosEaseOutBack(t);
                
                case EasingUtils.EasingType.easeInExpo:
                    return GetPosEaseInExpo(t);
                case EasingUtils.EasingType.easeOutExpo:
                    return GetPosEaseOutExpo(t);
                
                case EasingUtils.EasingType.easeInOutSine:
                    return GetPosEaseInOutSine(t);
            }

            return 0.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPos (EasingUtils.EasingType easeType, float v, float max, float min) {
            if (max == min) {
                return 1.0f;
            }
            var maxDistance = max - min;
            var valDistance = v - min;
            var pos = valDistance / maxDistance;
            return GetPos(easeType, pos);
        }
    }
}