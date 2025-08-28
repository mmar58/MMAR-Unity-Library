namespace MMAR.Extensions
{
    using UnityEngine;
    using UnityEngine.UI;

    public static class RawImageExtensions
    {
        /// <summary>Sets the RawImage alpha to the given value (0..1) without changing RGB.</summary>
        public static void SetAlpha(this RawImage img, float a)
        {
            if (img == null) return;
            var c = img.color;
            c.a = Mathf.Clamp01(a);
            img.color = c;
        }
    }

}