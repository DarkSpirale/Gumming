using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DTerrain
{
    /// <summary>
    /// Simple script that paints with Clear color the primary layer and paints using Black the secondary layer.
    /// Additionally, on right click paints primary layer with black.
    /// Use with SampleScene1.
    /// </summary>
    public class ClickAndDestroy_OLD : MonoBehaviour
    {
        [SerializeField]
        protected ShapeName shapeToBeDestroyed;
        [SerializeField]
        protected ShapeName shapeToBeBuilt;
        [SerializeField]
        protected int circleSize = 16;
        [SerializeField]
        protected int rectangleWidth = 4;
        [SerializeField]
        protected int rectangleHeight = 4;
        [SerializeField]
        protected int rangeLength = 4;
        [SerializeField]
        protected int outlineSize = 4;


        protected Shape shapeToDestroy;
        protected Shape shapeToOutline;
        protected Shape shapeToBuild;

        [SerializeField]
        protected BasicPaintableLayer primaryLayer;
        [SerializeField]
        protected BasicPaintableLayer secondaryLayer;

        public void Start()
        {
            
        }

        public void Update()
        {
            switch (shapeToBeDestroyed)
            {
                case ShapeName.circle:
                    shapeToDestroy = Shape.GenerateShapeCircle(circleSize);
                    shapeToOutline = Shape.GenerateShapeCircle(circleSize + outlineSize);
                    break;
                case ShapeName.rectangle:
                    shapeToDestroy = Shape.GenerateShapeRect(rectangleWidth, rectangleHeight);
                    shapeToOutline = Shape.GenerateShapeRect(rectangleWidth + outlineSize, rectangleHeight + outlineSize);
                    break;
                case ShapeName.range:
                    shapeToDestroy = Shape.GenerateShapeRange(rangeLength);
                    shapeToOutline = Shape.GenerateShapeRange(rangeLength + outlineSize);
                    break;
            }

            switch (shapeToBeBuilt)
            {
                case ShapeName.circle:
                    shapeToBuild = Shape.GenerateShapeCircle(circleSize);
                    break;
                case ShapeName.rectangle:
                    shapeToBuild = Shape.GenerateShapeRect(rectangleWidth, rectangleHeight);
                    break;
                case ShapeName.range:
                    shapeToBuild = Shape.GenerateShapeRange(rangeLength);
                    break;
            }

            if (Input.GetMouseButton(0))
            {
                OnLeftMouseButtonClick();
            }

            if(Input.GetMouseButton(1))
            {
                OnRightMouseButtonClick();
            }
        }

        protected virtual void OnLeftMouseButtonClick()
        {

            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;
            WorldManager.instance.DestroyShape(shapeToDestroy, p);

            /*primaryLayer?.Paint(new PaintingParameters() 
            { 
                Color = Color.clear, 
                Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU) - circleSize), 
                Shape = destroyCircle, 
                PaintingMode=PaintingMode.REPLACE_COLOR,
                DestructionMode = DestructionMode.DESTROY
            });

            secondaryLayer?.Paint(new PaintingParameters() 
            { 
                Color = new Color(0.0f,0.0f,0.0f,0.75f), 
                Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - circleSize-outlineSize, (int)(p.y * secondaryLayer.PPU) - circleSize-outlineSize), 
                Shape = outlineCircle, 
                PaintingMode=PaintingMode.NONE,
                DestructionMode = DestructionMode.NONE
            });
            */         
        }

        protected virtual void OnRightMouseButtonClick()
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;
            primaryLayer?.Paint(new PaintingParameters()
            {
                Color = Color.black,
                Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU) - circleSize),
                Shape = shapeToBuild,
                PaintingMode = PaintingMode.REPLACE_COLOR,
                DestructionMode = DestructionMode.BUILD
            });

        }

        
    }

    
}
