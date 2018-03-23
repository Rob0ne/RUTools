using UnityEngine;

namespace RUT.Utilities
{
    /// <summary>
    /// Easing utilities.
    /// </summary>
    public static class UEase
    {
        public enum EaseType
        {
            constant,
            linear,

            easeInQuad,
            easeOutQuad,
            easeInOutQuad,
            easeInCubic,
            easeOutCubic,
            easeInOutCubic,
            easeInQuart,
            easeOutQuart,
            easeInOutQuart,
            easeInQuint,
            easeOutQuint,
            easeInOutQuint,
            easeInSine,
            easeOutSine,
            easeInOutSine,
            easeInExpo,
            easeOutExpo,
            easeInOutExpo,
            easeInCirc,
            easeOutCirc,
            easeInOutCirc,

            spring,

            easeInBounce,
            easeOutBounce,
            easeInOutBounce,

            easeInBack,
            easeOutBack,
            easeInOutBack,

            easeInElastic,
            easeOutElastic,
            easeInOutElastic,
        }

        public delegate float EaseFunction(float start, float end, float value);

        public static EaseFunction GetEaseFunction(EaseType type)
        {
            EaseFunction f = null;

            switch (type)
            {
                case EaseType.constant:
                    f = Constant;
                    break;
                case EaseType.easeInQuad:
                    f = EaseInQuad;
                    break;
                case EaseType.easeOutQuad:
                    f = EaseOutQuad;
                    break;
                case EaseType.easeInOutQuad:
                    f = EaseInOutQuad;
                    break;
                case EaseType.easeInCubic:
                    f = EaseInCubic;
                    break;
                case EaseType.easeOutCubic:
                    f = EaseOutCubic;
                    break;
                case EaseType.easeInOutCubic:
                    f = EaseInOutCubic;
                    break;
                case EaseType.easeInQuart:
                    f = EaseInQuart;
                    break;
                case EaseType.easeOutQuart:
                    f = EaseOutQuart;
                    break;
                case EaseType.easeInOutQuart:
                    f = EaseInOutQuart;
                    break;
                case EaseType.easeInQuint:
                    f = EaseInQuint;
                    break;
                case EaseType.easeOutQuint:
                    f = EaseOutQuint;
                    break;
                case EaseType.easeInOutQuint:
                    f = EaseInOutQuint;
                    break;
                case EaseType.easeInSine:
                    f = EaseInSine;
                    break;
                case EaseType.easeOutSine:
                    f = EaseOutSine;
                    break;
                case EaseType.easeInOutSine:
                    f = EaseInOutSine;
                    break;
                case EaseType.easeInExpo:
                    f = EaseInExpo;
                    break;
                case EaseType.easeOutExpo:
                    f = EaseOutExpo;
                    break;
                case EaseType.easeInOutExpo:
                    f = EaseInOutExpo;
                    break;
                case EaseType.easeInCirc:
                    f = EaseInCirc;
                    break;
                case EaseType.easeOutCirc:
                    f = EaseOutCirc;
                    break;
                case EaseType.easeInOutCirc:
                    f = EaseInOutCirc;
                    break;
                case EaseType.linear:
                    f = Linear;
                    break;
                case EaseType.spring:
                    f = Spring;
                    break;
                case EaseType.easeInBounce:
                    f = EaseInBounce;
                    break;
                case EaseType.easeOutBounce:
                    f = EaseOutBounce;
                    break;
                case EaseType.easeInOutBounce:
                    f = EaseInOutBounce;
                    break;
                case EaseType.easeInBack:
                    f = EaseInBack;
                    break;
                case EaseType.easeOutBack:
                    f = EaseOutBack;
                    break;
                case EaseType.easeInOutBack:
                    f = EaseInOutBack;
                    break;
                case EaseType.easeInElastic:
                    f = EaseInElastic;
                    break;
                case EaseType.easeOutElastic:
                    f = EaseOutElastic;
                    break;
                case EaseType.easeInOutElastic:
                    f = EaseInOutElastic;
                    break;
            }

            return f;
        }


        public static float Constant(float start, float end, float value)
        {
            return end;
        }

        public static float Linear(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }

        public static float Spring(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));

            return start + (end - start) * value;
        }

        public static float EaseInQuad(float start, float end, float value)
        {
            end -= start;

            return end * value * value + start;
        }

        public static float EaseOutQuad(float start, float end, float value)
        {
            end -= start;

            return -end * value * (value - 2) + start;
        }

        public static float EaseInOutQuad(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
                return end * 0.5f * value * value + start;

            value--;

            return -end * 0.5f * (value * (value - 2) - 1) + start;
        }

        public static float EaseInCubic(float start, float end, float value)
        {
            end -= start;

            return end * value * value * value + start;
        }

        public static float EaseOutCubic(float start, float end, float value)
        {
            value--;
            end -= start;

            return end * (value * value * value + 1) + start;
        }

        public static float EaseInOutCubic(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
                return end * 0.5f * value * value * value + start;

            value -= 2;

            return end * 0.5f * (value * value * value + 2) + start;
        }

        public static float EaseInQuart(float start, float end, float value)
        {
            end -= start;

            return end * value * value * value * value + start;
        }

        public static float EaseOutQuart(float start, float end, float value)
        {
            value--;
            end -= start;

            return -end * (value * value * value * value - 1) + start;
        }

        public static float EaseInOutQuart(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
                return end * 0.5f * value * value * value * value + start;

            value -= 2;

            return -end * 0.5f * (value * value * value * value - 2) + start;
        }

        public static float EaseInQuint(float start, float end, float value)
        {
            end -= start;

            return end * value * value * value * value * value + start;
        }

        public static float EaseOutQuint(float start, float end, float value)
        {
            value--;
            end -= start;

            return end * (value * value * value * value * value + 1) + start;
        }

        public static float EaseInOutQuint(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
                return end * 0.5f * value * value * value * value * value + start;

            value -= 2;

            return end * 0.5f * (value * value * value * value * value + 2) + start;
        }

        public static float EaseInSine(float start, float end, float value)
        {
            end -= start;

            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }

        public static float EaseOutSine(float start, float end, float value)
        {
            end -= start;

            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }

        public static float EaseInOutSine(float start, float end, float value)
        {
            end -= start;

            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }

        public static float EaseInExpo(float start, float end, float value)
        {
            end -= start;

            return end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }

        public static float EaseOutExpo(float start, float end, float value)
        {
            end -= start;

            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }

        public static float EaseInOutExpo(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
                return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;

            value--;

            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        public static float EaseInCirc(float start, float end, float value)
        {
            end -= start;

            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        public static float EaseOutCirc(float start, float end, float value)
        {
            value--;
            end -= start;

            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        public static float EaseInOutCirc(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
                return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;

            value -= 2;

            return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        public static float EaseInBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;

            return end - EaseOutBounce(0, end, d - value) + start;
        }

        public static float EaseOutBounce(float start, float end, float value)
        {
            value /= 1f;
            end -= start;

            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        public static float EaseInOutBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;

            if (value < d * 0.5f)
                return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else
                return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static float EaseInBack(float start, float end, float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;

            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        public static float EaseOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;

            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        public static float EaseInOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;

            if ((value) < 1)
            {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }

            value -= 2;
            s *= (1.525f);

            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        public static float Punch(float amplitude, float value)
        {
            float s = 9;
            if (value == 0)
            {
                return 0;
            }
            else if (value == 1)
            {
                return 0;
            }

            float period = 1 * 0.3f;
            s = period / (2 * Mathf.PI) * Mathf.Asin(0);

            return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
        }

        public static float EaseInElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0)
                return start;

            if ((value /= d) == 1)
                return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float EaseOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0)
                return start;

            if ((value /= d) == 1)
                return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float EaseInOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0)
                return start;

            if ((value /= d * 0.5f) == 2)
                return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1)
                return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;

            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }
    }
}