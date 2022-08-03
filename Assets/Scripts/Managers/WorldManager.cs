using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private BasicPaintableLayer primaryLayer;
    [SerializeField]
    private BasicPaintableLayer secondaryLayer;

    public static WorldManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one instance of WorldManager existing.");
            return;
        }

        instance = this;
    }

    public void DestroyShape(Shape destroyedShape, Vector2 pos)
    {
        PaintToDestroy(destroyedShape, pos);
    }

    public void DestroyShape(Shape destroyedShape, Shape outlineShape, Vector2 pos)
    {
        PaintToDestroy(destroyedShape, pos);
        PaintToOutline(outlineShape, pos);      
    }

    public void BuildShape(Shape builtShape, Vector2 pos, Sprite sprite)
    {
        Texture2D texture = null;
        if(sprite != null) texture = GetTextureFromSprite(sprite);

        PaintToBuild(builtShape, pos, texture);
    }

    private void PaintToDestroy(Shape destroyedShape, Vector2 pos)
    {
        primaryLayer.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(pos.x * primaryLayer.PPU) - destroyedShape.Width / 2, (int)(pos.y * primaryLayer.PPU) - destroyedShape.Height / 2),
            Shape = destroyedShape,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });
    }

    private void PaintToOutline(Shape outlineShape, Vector2 pos)
    {
        secondaryLayer.Paint(new PaintingParameters()
        {
            Color = new Color(0.0f, 0.0f, 0.0f, 0.75f),
            Position = new Vector2Int((int)(pos.x * secondaryLayer.PPU) - outlineShape.Width / 2, (int)(pos.y * secondaryLayer.PPU) - outlineShape.Height / 2),
            Shape = outlineShape,
            PaintingMode = PaintingMode.NONE,
            DestructionMode = DestructionMode.NONE
        });
    }

    private void PaintToBuild(Shape builtShape, Vector2 pos, Texture2D texture = null)
    {
        PaintingMode paintingMode;
        if (texture == null)
            paintingMode = PaintingMode.REPLACE_COLOR;
        else
        {
            Debug.Log("From texture");
            paintingMode = PaintingMode.FROM_TEXTURE;

        }

        primaryLayer.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(pos.x * primaryLayer.PPU) - builtShape.Width / 2, (int)(pos.y * primaryLayer.PPU) - builtShape.Height / 2),
            Shape = builtShape,
            PaintingMode = paintingMode,
            DestructionMode = DestructionMode.BUILD,
        });
    }

    private Texture2D GetTextureFromSprite(Sprite sprite)
    {
        return Shape.GetTextureFromSprite(sprite);
    }
}
