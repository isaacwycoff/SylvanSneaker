using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.UI
{
    public interface IDevConsole
    {
        bool Expanded { get; set; }
        void Draw(TimeSpan timeDelta);
        void SetDebugLine(string line);
        void WriteLine(string line);
    }

    public class DevConsole : IDevConsole
    {
        private SpriteBatch Batch { get; set; }
        private SpriteFont Font { get; set; }

        private List<string> Lines { get; set; }        // everything about lines works backwards, since it scrolls up

        private string DebugLine { get; set; }

        public bool Expanded { get; set; }

        private const int MaxLines = 10;

        private const int LineHeight = 25;

        public DevConsole(SpriteBatch batch, SpriteFont font)
        {
            this.Batch = batch;
            this.Font = font;

            this.DebugLine = "";
            this.Lines = new List<string>();

            this.Expanded = false;
        }

        public void Draw(TimeSpan timeDelta)
        {
            // Find the center of the string
            Vector2 FontOrigin = Font.MeasureString(DebugLine) / 2;
            // Draw the string

            this.DrawDebugLine();

            if (Expanded)
            {
                this.DrawExpandedConsole();
            }
        }

        public void SetDebugLine(string line)
        {
            DebugLine = line;
        }

        public void WriteLine(string line)
        {
            this.Lines.Insert(0, line);

            if (Lines.Count > MaxLines)
            {
                Lines.RemoveAt(MaxLines);
            }

            //            this.DebugLine = line;
        }

        private void DrawDebugLine()
        {
            this.DrawLine(DebugLine, 10, 490 + LineHeight);
        }

        private void DrawExpandedConsole()
        {
            var x = 10;
            var y = 490;

            foreach (var line in Lines)
            {
                this.DrawLine(line, x, y);
                y -= LineHeight;            // FIXME: magic #s
            }
        }

        private void DrawLine(string line, int x, int y)
        {
            Batch.DrawString(Font, line, new Vector2(x + 1, y + 1), Color.DarkBlue);
            Batch.DrawString(Font, line, new Vector2(x, y), Color.WhiteSmoke);
        }
    }
}
