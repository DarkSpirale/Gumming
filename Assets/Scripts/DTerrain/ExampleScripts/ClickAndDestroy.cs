using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DTerrain
{
    /// <summary>
    /// Simple script that paints with Clear color the primary layer and paints using Black the secondary layer.
    /// Additionally, on right click paints primary layer with black.
    /// Use with SampleScene1.
    /// </summary>
    public class ClickAndDestroy : MonoBehaviour
    {
        public bool enableDestroying;
        public bool enableBuilding;

        public ShapeName destroyedShape;
        public ShapeName builtShape  ;

        public int destroyCircleSize = 16;
        public int destroyRectangleWidth = 4;
        public int destroyRectangleHeight = 4;
        public int destroyRangeLength = 4;
        public int destroyTriangleWidth = 7;
        public int destroyTriangleHeight = 7;
        public Sprite destroyTexture;
        public int buildCircleSize = 16;
        public int buildRectangleWidth = 4;
        public int buildRectangleHeight = 4;
        public int buildRangeLength = 4;
        public int buildTriangleWidth = 7;
        public int buildTriangleHeight = 7;
        public Sprite buildTexture;
        public int outlineSize = 4;
        public bool sameShapeSettings = false;

        public BasicPaintableLayer primaryLayer;
        public BasicPaintableLayer secondaryLayer;

        protected Shape shapeToDestroy;
        protected Shape shapeToOutline;
        protected Shape shapeToBuild;

        private ShapeName previousDestroyedShape;
        private ShapeName previousBuiltShape;

        private Sprite previousDestroyTexture;
        private Sprite previousBuildTexture;
        private Sprite textureToUse;

        private int outlineSizeToUse;
        private int previousDestroyCircleSize;
        private int previousDestroyRectangleWidth;
        private int previousDestroyRectangleHeight;
        private int previousDestroyRangeLength;
        private int previousDestroyTriangleWidth;
        private int previousDestroyTriangleHeight;
        private int previousOutlineSize;
        private int previousBuildCircleSize;
        private int previousBuildRectangleWidth;
        private int previousBuildRectangleHeight;
        private int previousBuildRangeLength;
        private int previousBuildTriangleWidth;
        private int previousBuildTriangleHeight;


        public void Start()
        {
            if (sameShapeSettings) CopyDestroySettingsToBuildSettings();

            RefreshPreviousVariables();
            InitializeShapes();
        }

        public void Update()
        {
            CheckIfNewShapeProperties();

            if(Input.GetMouseButtonDown(0) && enableDestroying)
            {
                OnLeftMouseButtonClick();
            }

            if(Input.GetMouseButtonDown(1) && enableBuilding)
            {
                OnRightMouseButtonClick();
            }
        }

        private void CopyDestroySettingsToBuildSettings()
        {
            builtShape = destroyedShape;
            buildCircleSize = destroyCircleSize;
            buildRectangleWidth = destroyRectangleWidth;
            buildRectangleHeight = destroyRectangleHeight;
            buildRangeLength = destroyRangeLength;
            buildTriangleWidth = destroyTriangleWidth;
            buildTriangleHeight = destroyTriangleHeight;
            buildTexture = destroyTexture;
        }

        private void CheckIfNewShapeProperties()
        {
            if(previousDestroyedShape != destroyedShape || previousDestroyTexture != destroyTexture || previousDestroyCircleSize != destroyCircleSize || previousDestroyRectangleWidth != destroyRectangleWidth ||
                previousDestroyRectangleHeight != destroyRectangleHeight || previousDestroyRangeLength != destroyRangeLength || previousDestroyTriangleWidth != destroyTriangleWidth || previousDestroyTriangleHeight != destroyTriangleHeight ||  previousOutlineSize != outlineSize ||
                previousBuiltShape != builtShape || previousBuildTexture != buildTexture || previousBuildCircleSize != buildCircleSize || previousBuildRectangleWidth != buildRectangleWidth ||
                previousBuildRectangleHeight != buildRectangleHeight || previousBuildRangeLength != buildRangeLength || previousBuildTriangleWidth != buildTriangleWidth || previousBuildTriangleHeight != buildTriangleHeight)
            {
                if (sameShapeSettings) CopyDestroySettingsToBuildSettings();
                RefreshPreviousVariables();
                InitializeShapes();
            }
        }

        private void RefreshPreviousVariables()
        {
            previousDestroyedShape = destroyedShape;
            previousDestroyTexture = destroyTexture;
            previousDestroyCircleSize = destroyCircleSize;
            previousDestroyRectangleWidth = destroyRectangleWidth;
            previousDestroyRectangleHeight = destroyRectangleHeight;
            previousDestroyRangeLength = destroyRangeLength;
            previousDestroyTriangleWidth = destroyTriangleWidth;
            previousDestroyTriangleHeight = destroyTriangleHeight;
            previousBuiltShape = builtShape;
            previousBuildTexture = buildTexture;
            previousBuildCircleSize = buildCircleSize;
            previousBuildRectangleWidth = buildRectangleWidth;
            previousBuildRectangleHeight = buildRectangleHeight;
            previousBuildRangeLength = buildRangeLength;
            previousBuildTriangleWidth = buildTriangleWidth;
            previousBuildTriangleHeight = buildTriangleHeight;
            previousOutlineSize = outlineSize;
        }

        private void InitializeShapes()
        {
            outlineSizeToUse = (destroyedShape == ShapeName.texture) ? 0 : outlineSize;
            textureToUse = null;

            switch (destroyedShape)
            {
                case ShapeName.circle:
                    shapeToDestroy = Shape.GenerateShapeCircle(destroyCircleSize);
                    shapeToOutline = Shape.GenerateShapeCircle(destroyCircleSize + outlineSizeToUse);
                    break;
                case ShapeName.rectangle:
                    shapeToDestroy = Shape.GenerateShapeRect(destroyRectangleWidth, destroyRectangleHeight);
                    shapeToOutline = Shape.GenerateShapeRect(destroyRectangleWidth + outlineSizeToUse, destroyRectangleHeight + outlineSizeToUse);
                    break;
                case ShapeName.range:
                    shapeToDestroy = Shape.GenerateShapeRange(destroyRangeLength);
                    shapeToOutline = Shape.GenerateShapeRange(destroyRangeLength + outlineSizeToUse);
                    break;
                case ShapeName.triangle:
                    shapeToDestroy = Shape.GenerateShapeTriangle(destroyTriangleWidth, destroyTriangleHeight);
                    shapeToOutline = Shape.GenerateShapeTriangle(destroyTriangleWidth + outlineSizeToUse, destroyTriangleHeight + outlineSizeToUse);
                    break;
                case ShapeName.texture:
                    shapeToDestroy = Shape.GenerateShapeFromSprite(destroyTexture);
                    break;
            }

            switch (builtShape)
            {
                case ShapeName.circle:
                    shapeToBuild = Shape.GenerateShapeCircle(buildCircleSize);
                    break;
                case ShapeName.rectangle:
                    shapeToBuild = Shape.GenerateShapeRect(buildRectangleWidth, buildRectangleHeight);
                    break;
                case ShapeName.range:
                    shapeToBuild = Shape.GenerateShapeRange(buildRangeLength);
                    break;
                case ShapeName.triangle:
                    shapeToBuild = Shape.GenerateShapeTriangle(buildTriangleWidth, buildTriangleHeight);
                    break;
                case ShapeName.texture:
                    shapeToBuild = Shape.GenerateShapeFromSprite(buildTexture);
                    textureToUse = buildTexture;
                    break;
            }
        }

        public void OnLeftMouseButtonClick()
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;
            if(outlineSizeToUse > 0)
            {
                WorldManager.instance.DestroyShape(shapeToDestroy, shapeToOutline, p);
            }
            else
            {
                WorldManager.instance.DestroyShape(shapeToDestroy, p);
            }

        }

        public void OnRightMouseButtonClick()
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;
            WorldManager.instance.BuildShape(shapeToBuild, p, textureToUse);
        }
    }



    [CustomEditor(typeof(ClickAndDestroy))]
    public class ClickAndDestroy_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            ClickAndDestroy myTarget = (ClickAndDestroy)target;

            myTarget.enableDestroying = EditorGUILayout.Toggle("Enable Destroying", myTarget.enableDestroying);
            myTarget.enableBuilding = EditorGUILayout.Toggle("Enable Building", myTarget.enableBuilding);
            EditorGUILayout.Space(5f);

            EditorGUILayout.LabelField("Shape Selection", EditorStyles.boldLabel);
            myTarget.sameShapeSettings = EditorGUILayout.Toggle("Use same settings", myTarget.sameShapeSettings);
            EditorGUILayout.Space(5f);

            EditorGUI.indentLevel = 1;

            string stringShape;
            stringShape = myTarget.sameShapeSettings ? "Shape" : "Destroying Shape";
            EditorGUILayout.LabelField(stringShape + " Settings", EditorStyles.boldLabel);

            
            myTarget.destroyedShape = (ShapeName)EditorGUILayout.EnumPopup(stringShape, myTarget.destroyedShape);

            switch (myTarget.destroyedShape)
            {
                case ShapeName.circle:
                    myTarget.destroyCircleSize = EditorGUILayout.IntField("Circle size", myTarget.destroyCircleSize);
                    break;
                case ShapeName.rectangle:
                    myTarget.destroyRectangleWidth = EditorGUILayout.IntField("Rectangle Width", myTarget.destroyRectangleWidth);
                    myTarget.destroyRectangleHeight = EditorGUILayout.IntField("Rectangle Height", myTarget.destroyRectangleHeight);
                    break;
                case ShapeName.range:
                    myTarget.destroyRangeLength = EditorGUILayout.IntField("Range Length", myTarget.destroyRangeLength);
                    break;
                case ShapeName.triangle:
                    myTarget.destroyTriangleWidth = EditorGUILayout.IntField("Triangle Width", myTarget.destroyTriangleWidth);
                    myTarget.destroyTriangleHeight = EditorGUILayout.IntField("Triangle Height", myTarget.destroyTriangleHeight);
                    break;
                case ShapeName.texture:
                    myTarget.destroyTexture = (Sprite)EditorGUILayout.ObjectField("Texture", myTarget.destroyTexture, typeof(Sprite), true);
                    break;
            }
            myTarget.outlineSize = EditorGUILayout.IntField("Outline size", myTarget.outlineSize);

            if (!myTarget.sameShapeSettings)
            {
                EditorGUILayout.Space(5f);
                EditorGUILayout.LabelField("Building Shape Settings", EditorStyles.boldLabel);
                myTarget.builtShape = (ShapeName)EditorGUILayout.EnumPopup("Building Shape", myTarget.builtShape);

                switch (myTarget.builtShape)
                {
                    case ShapeName.circle:
                        myTarget.buildCircleSize = EditorGUILayout.IntField("Circle size", myTarget.buildCircleSize);
                        break;
                    case ShapeName.rectangle:
                        myTarget.buildRectangleWidth = EditorGUILayout.IntField("Rectangle Width", myTarget.buildRectangleWidth);
                        myTarget.buildRectangleHeight = EditorGUILayout.IntField("Rectangle Height", myTarget.buildRectangleHeight);
                        break;
                    case ShapeName.range:
                        myTarget.buildRangeLength = EditorGUILayout.IntField("Range Length", myTarget.buildRangeLength);
                        break;
                    case ShapeName.triangle:
                        myTarget.buildTriangleWidth = EditorGUILayout.IntField("Triangle Width", myTarget.buildTriangleWidth);
                        myTarget.buildTriangleHeight = EditorGUILayout.IntField("Triangle Height", myTarget.buildTriangleHeight);
                        break;
                    case ShapeName.texture:
                        myTarget.buildTexture = (Sprite)EditorGUILayout.ObjectField("Texture", myTarget.buildTexture, typeof(Sprite), true);
                        break;
                }
            }

            EditorGUILayout.Space(10f);
            EditorGUI.indentLevel = 0;

            EditorGUILayout.LabelField("Layers Selection", EditorStyles.boldLabel);
            myTarget.primaryLayer = (BasicPaintableLayer)EditorGUILayout.ObjectField("Primary Layer", myTarget.primaryLayer, typeof(BasicPaintableLayer), true);
            myTarget.secondaryLayer = (BasicPaintableLayer)EditorGUILayout.ObjectField("Secondary Layer", myTarget.secondaryLayer, typeof(BasicPaintableLayer), true);
        }
    }
}

