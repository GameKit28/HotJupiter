using UnityEngine;

namespace HexasphereGrid {

    public static class ShaderParams {

        public static int MainTex = Shader.PropertyToID("_MainTex");
        public static int BaseMap = Shader.PropertyToID("_BaseMap");
        public static int Color2 = Shader.PropertyToID("_Color2");
        public static int Color = Shader.PropertyToID("_Color");
        public static int BaseColor = Shader.PropertyToID("_BaseColor");
        public static int TileAlpha = Shader.PropertyToID("_TileAlpha");
        public static int ColorShift = Shader.PropertyToID("_ColorShift");
        public static int Center = Shader.PropertyToID("_Center");

        public const string SKW_HIGHLIGHT_TINT_BACKGROUND = "_TINT_BACKGROUND";

    }

}