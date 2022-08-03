using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    /// <summary>
    /// Shape is a simple class that holds a list of Ranges (not ranges) and then is used to destroy terrain with.
    /// To make complicated shape destructions (not squares, circles etc.) don't use it as it supports only list of ranges.
    /// </summary>
    public class Shape
    {

        public List<Range> Ranges;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Shape(int w, int h)
        {
            Width = w;
            Height = h;
            Ranges = new List<Range>();
        }

        public void SetSize(int w, int h)
        {
            foreach (Range r in Ranges)
            {
                r.Min -= Height - h;
                r.Max -= Height - h;
            }

            Width = w;
            Height = h;
        }

        public Vector2 GetSize()
        {
            return new Vector2(Width, Height);
        }

        public static Shape GenerateShapeRange(int length)
        {
            Shape s = new Shape(1, length);
            s.Ranges.Add(new Range(0, length));
            return s;
        }

        /// <summary>
        /// Generates a Shape: circle.
        /// </summary>
        /// <param name="r">Radius</param>
        /// <returns>Shape: circle.</returns>
        public static Shape GenerateShapeCircle(int r)
        {
            int centerX = r;
            int centerY = r;
            Shape s = new Shape(2 * r, 2 * r);
            for (int i = 0; i <= 2 * r; i++)
            {
                bool down = false;
                int min = 0;
                int max = 0;
                for (int j = 0; j <= 2 * r; j++)
                {
                    if (Mathf.Sqrt((centerX - i) * (centerX - i) + (centerY - j) * (centerY - j)) < r)
                    {
                        if (down == false)
                        {
                            down = true;
                            min = j;
                        }
                    }
                    else if (down)
                    {
                        max = j;
                        break;
                    }
                }
                if (down)
                {
                    Range range = new Range(min, max);
                    s.Ranges.Add(range);
                }
            }
            return s;
        }

        public static Shape GenerateShapeRect(int w, int h)
        {
            Shape s = new Shape(w, h);

            for(int i = 0; i<w;i++)
            {
                s.Ranges.Add(new Range(0, h-1)); //0,1,2...h-1
            }

            return s;
        }

        public static Shape GenerateShapeTriangle(int w, int h)
        {
            Shape s = new Shape(w, h);

            float xBot = w / 2f;
            float yBot = h;
            float dXdY = xBot / yBot;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    float pos1 = Mathf.Abs(xBot - i) / Mathf.Abs(yBot - j);
                    float pos2 = Mathf.Abs(xBot - (i + 1)) / Mathf.Abs(yBot - j);
                    float posAvg = (pos1 + pos2) / 2f;

                    if(posAvg > dXdY)
                    {
                        Range range = new Range(0, j - 1);
                        s.Ranges.Add(range);
                        break;
                    }
                }
            }

            return s;
        }

        public static Shape GenerateShapeFromSprite(Sprite sprite)
        {
            Texture2D texture = GetTextureFromSprite(sprite);

            return Shape.GenerateShapeFromTexture(texture);
        }

        public static Shape GenerateShapeFromTexture(Texture2D texture)
        {
            int w = texture.width;
            int h = texture.height;
            int minWidth = 10000;
            int maxWidth = -1;
            int minHeight = 10000;
            int maxHeight = -1;

            Shape s = new Shape(w, h);

            for (int i = 0; i < w; i++)
            {
                int min = 0;
                int max;
                bool currentRange = false;
                bool rangeInCurrentColumn = false;
                List<Color> pixelsInRange = new List<Color>();

                for (int j = 0; j < h; j++)
                {
                    Color pixel = texture.GetPixel(i, j);
                    
                    if (pixel.a > 0)
                    {
                        pixelsInRange.Add(pixel);
                        if (!currentRange)
                        {
                            min = j;
                            currentRange = true;
                        }

                        //Calculate width and heigth to redefine shape size depending on the texture's pixels
                        if (i < minWidth) minWidth = i;
                        if (i > maxWidth) maxWidth = i;
                        if (j < minHeight) minHeight = j;
                        if (j > maxHeight) maxHeight = j;
                    }
                    else if (currentRange)
                    {
                        max = j - 1;
                        currentRange = false;

                        Range range = new Range(min, max, rangeInCurrentColumn, pixelsInRange.ToArray());
                        s.Ranges.Add(range);

                        rangeInCurrentColumn = true;
                    }


                }
                if (currentRange)
                {
                    max = h - 1;
                    Range range = new Range(min, max, rangeInCurrentColumn, pixelsInRange.ToArray());
                    s.Ranges.Add(range);
                }
            }

            if(minWidth == 10000 || maxHeight == 10000)
            {
                Debug.LogError("Given texture for shape is transparent.");
                return null;
            }

            s.SetSize(maxWidth - minWidth + 1, maxHeight - minHeight + 1);

            return s;
        }

        public static Texture2D GetTextureFromSprite(Sprite sprite)
        {
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

            if (sprite.rect.x != 0 || sprite.rect.y != 0)
            {
                Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
                texture.SetPixels(pixels);
                texture.Apply();
            }
            else
            {
                texture = sprite.texture;
            }

            return texture;
        }
    }

    public enum ShapeName
    {
        rectangle,
        circle,
        triangle,
        range,
        texture
    }

    public enum TriangleDir
    {
        right,
        left,
        up,
        down
    }
}
