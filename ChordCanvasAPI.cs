using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;

namespace ChordCanvas
{
    public class CanvasAPI
    {
        public enum Layout
        {
            One,
            Two
        }

        public static async Task CreateImage(Canvas2DContext ctx, Chord chord, Layout layout, double size)
        {
            ChordDrawer drawer = new(ctx, chord, size);

            switch (layout)
            {
                case Layout.One:
                {
                    await drawer.DrawChordBox();
                    await drawer.DrawBars();
                    await drawer.DrawChordPositionsAndFingers();
                    await drawer.DrawChordName();
                    await drawer.DrawStringNames();
                }
                break;

                case Layout.Two:
                {
                    await drawer.DrawChordBox();
                    await drawer.DrawChordPositions();
                    await drawer.DrawBars();
                    await drawer.DrawChordName();
                    await drawer.DrawFingers();
                }
                break;
            }
        }
    }
}
