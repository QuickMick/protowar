using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Protobase.manager;
using Protobase.resources;
using Protobase.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protobase.entity
{
   

    //public class EntityContext
    //{
    //    public static Texture2D COLOR;

    //    #region gameflags
    //    private HashSet<GameFlags> GameFlags = new HashSet<GameFlags>();

    //    public bool IsFlagSet(params GameFlags[] flags)
    //    {
    //        foreach (GameFlags f in flags)
    //        {
    //            if (this.GameFlags.Contains(f))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    public void AddFlag(GameFlags s)
    //    {
    //        this.GameFlags.Add(s);
    //    }

    //    public void RemoveFlag(GameFlags s)
    //    {
    //        this.GameFlags.Remove(s);
    //    }

    //    #endregion gameflags

    //    public EntityContext(InputManager im, SpriteBatch sb, GraphicsDeviceManager gdm, WorldManager em, Camera2D cam)
    //    {
    //        this.InputManager = im;
    //        this.EntityManager = em;
    //        this.SpriteBatch = sb;
    //        this.Graphics = gdm;
    //        this.Camera = cam;

    //        COLOR = new Texture2D(this.Graphics.GraphicsDevice, 1, 1);
    //        COLOR.SetData<Color>(new Color[] { Color.White });// fill the texture with white
    //    }

    //    public InputManager InputManager { get; private set; }

    //    public WorldManager EntityManager { get; private set; }

    //    public GraphicsDeviceManager Graphics { get; private set; }

    //    public SpriteBatch SpriteBatch { get; private set; }

    //    public Camera2D Camera { get; private set; }

    //    #region util methods
    //    public void DrawLine(Vector2 start, Vector2 end, Color color, float lineThickness = 0.1f)
    //    {
    //        Vector2 edge = end - start;
    //        // calculate angle to rotate line
    //        float angle = (float)Math.Atan2(edge.Y, edge.X);

    //        this.SpriteBatch.Draw(
    //            texture: COLOR, // Texture
    //            position: new Vector2(start.X, start.Y),
    //            color: color, // If you don't want to add tinting use white
    //            rotation: angle,                      // Rotation
    //            origin: new Vector2(0, lineThickness / 4),                   // Origin
    //            scale: new Vector2(edge.Length(), lineThickness),
    //            //  effect: SpriteEffects.None,     // Mirroring effects
    //            layerDepth: 0,
    //            sourceRectangle: null,
    //            destinationRectangle: null

    //        );                 // Layer depth
    //    }


    //    /// <summary>
    //    /// Draw a line into a SpriteBatch
    //    /// </summary>
    //    /// <param name="batch">SpriteBatch to draw line</param>
    //    /// <param name="color">The line color</param>
    //    /// <param name="point1">Start Point</param>
    //    /// <param name="point2">End Point</param>
    //    /// <param name="Layer">Layer or Z position</param>
    //    public void DrawLine2(Vector2 point1, Vector2 point2, Color color, float thinkness = 0.1f)
    //    {
    //        float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
    //        float length = (point2 - point1).Length();

    //        Vector2 dir = point2 - point1;
    //        dir.Normalize();

    //        point1 += dir * thinkness / 4;

    //        dir = VectorMath.RotateAroundOrigin(dir, 90, true);

    //        point1 += dir * (thinkness / 2);


    //        this.SpriteBatch.Draw(COLOR, point1, null, color,
    //                   angle, Vector2.Zero, new Vector2(length, thinkness),
    //                   SpriteEffects.None, 0);
    //    }

    //    public void DrawPolyLine(Vector2[] points, Color color, float width = 1, bool closed = false)
    //    {
    //        for (int i = 0; i < points.Length - 1; i++)
    //            this.DrawLine(points[i], points[i + 1], color, width);
    //        if (closed)
    //            this.DrawLine(points[points.Length - 1], points[0], color, width);
    //    }


    //    public void DrawCircle(float steps, float radius, Vector2 position, Color color, float thinkness = 0.1f)
    //    {
    //        double angleStep = steps / radius;

    //        List<Vector2> points = new List<Vector2>();

    //        for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
    //        {
    //            // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
    //            int x = (int)Math.Round(radius + radius * Math.Cos(angle));
    //            int y = (int)Math.Round(radius + radius * Math.Sin(angle));

    //            points.Add(position + new Vector2(x, y));
    //        }

    //        this.DrawPolyLine(points.ToArray(), color, thinkness, true);
    //    }

    //    /* public void DrawLine3(Vector2 begin, Vector2 end, Color color, int width = 1)
    //     {
    //         Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
    //         Vector2 v = Vector2.Normalize(begin - end);
    //         float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
    //         if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
    //         this.SpriteBatch.Draw(COLOR, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
    //     }*/

    //    public void DrawSprite(Texture2D tex, Vector2 position, float width, float height, Color color, float rotation = 0)
    //    {
    //        float w = tex.Width;
    //        float h = tex.Height;

    //        this.SpriteBatch.Draw(
    //          texture: tex, // Texture
    //          position: position,             // Position
    //          color: color, // If you don't want to add tinting use white
    //          rotation: rotation,
    //          sourceRectangle: new Rectangle(0, 0, (int)w, (int)h),
    //          origin: new Vector2(w / 2, h / 2), //Vector2.One / 2,                     
    //          scale: new Vector2(width / w, height / h) // scale        
    //        );


    //    }

    //    public void DrawImage(Texture2D tex, Vector2 position, float width, float height, Color color, float rotation = 0)
    //    {
    //        float w = tex.Width;
    //        float h = tex.Height;

    //        this.SpriteBatch.Draw(
    //          texture: tex, // Texture
    //          position: position,             // Position
    //          color: color, // If you don't want to add tinting use white
    //          rotation: rotation,
    //          sourceRectangle: new Rectangle(0, 0, (int)w, (int)h),
    //          origin: new Vector2(0, 0), //Vector2.One / 2,                     
    //          scale: new Vector2(width / w, height / h) // scale        
    //        );


    //    }

    //    public void DrawRectangle(Vector2 position, Vector2 size, float thicknessOfBorder, Color borderColor)
    //    {

    //        // Draw top line
    //        //     spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

    //        this.SpriteBatch.Draw(
    //            texture: COLOR, // Texture
    //            position: new Vector2(position.X, position.Y),
    //            scale: new Vector2(size.X, thicknessOfBorder),
    //            color: borderColor // If you don't want to add tinting use white
    //            );


    //        // Draw left line
    //        // spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

    //        this.SpriteBatch.Draw(
    //            texture: COLOR, // Texture
    //            position: new Vector2(position.X, position.Y),
    //            scale: new Vector2(thicknessOfBorder, size.Y),
    //            color: borderColor // If you don't want to add tinting use white
    //            );

    //        // Draw right line
    //        /* spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
    //                                         rectangleToDraw.Y,
    //                                         thicknessOfBorder,
    //                                         rectangleToDraw.Height), borderColor);
    //         */

    //        this.SpriteBatch.Draw(
    //            texture: COLOR, // Texture
    //            position: new Vector2((position.X + size.X - thicknessOfBorder), position.Y),
    //            scale: new Vector2(thicknessOfBorder, size.Y),
    //            color: borderColor // If you don't want to add tinting use white
    //            );


    //        // Draw bottom line
    //        /*  spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
    //                                          rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
    //                                          rectangleToDraw.Width,
    //                                          thicknessOfBorder), borderColor);
    //          */

    //        this.SpriteBatch.Draw(
    //            texture: COLOR, // Texture
    //            position: new Vector2(position.X, position.Y + size.Y - thicknessOfBorder),
    //            scale: new Vector2(size.X, thicknessOfBorder),
    //            color: borderColor // If you don't want to add tinting use white
    //            );
    //    }

    //    public Texture2D CreateCircle(int radius)
    //    {
    //        int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
    //        Texture2D texture = new Texture2D(this.Graphics.GraphicsDevice, outerRadius, outerRadius);

    //        Color[] data = new Color[outerRadius * outerRadius];

    //        // Colour the entire texture transparent first.
    //        for (int i = 0; i < data.Length; i++)
    //            data[i] = Color.Transparent;

    //        // Work out the minimum step necessary using trigonometry + sine approximation.
    //        double angleStep = 1f / radius;

    //        for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
    //        {
    //            // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
    //            int x = (int)Math.Round(radius + radius * Math.Cos(angle));
    //            int y = (int)Math.Round(radius + radius * Math.Sin(angle));

    //            data[y * outerRadius + x + 1] = Color.White;
    //        }

    //        texture.SetData(data);
    //        return texture;
    //    }

    //    #endregion util methods
    //}
}
