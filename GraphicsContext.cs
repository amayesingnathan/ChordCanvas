using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordCanvas
{
    internal class GraphicsContext
    {
        private Canvas2DContext _ctx;

        public GraphicsContext(Canvas2DContext ctx)
        {
            _ctx = ctx;
        }

        public Pen CreatePen(string colour, double size)
        {
            return new Pen(_ctx, colour, size);
        }
        public Font CreateFont(string name, double size)
        {
            return new Font(_ctx, name, size);
        }

        public async Task DrawLine(Pen pen, double x1, double y1, double x2, double y2)
        {
            if (_ctx is null) 
                return;

            await _ctx.BeginPathAsync();
            await pen.Set();
            await _ctx.MoveToAsync(x1, y1);
            await _ctx.LineToAsync(x2, y2);
            await _ctx.StrokeAsync();
        }

        public async Task FillRectangle(string color, double x1, double y1, double x2, double y2)
        {
            if (_ctx is null) 
                return;

            await _ctx.BeginPathAsync();
            await _ctx.SetFillStyleAsync(color);
            await _ctx.RectAsync(x1, y1, x2, y2);
            await _ctx.FillAsync();
        }
        public async Task DrawCircle(Pen pen, double x1, double y1, double diameter)
        {
            if (_ctx is null) 
                return;

            var radius = diameter / 2;
            await _ctx.BeginPathAsync();
            await pen.Set();
            await _ctx.ArcAsync(x1 + radius, y1 + radius, radius, 0, 2 * Math.PI, false);
            await _ctx.StrokeAsync();
        }
        public async Task FillCircle(string color, double x1, double y1, double diameter)
        {
            if (_ctx is null)
                return;

            var radius = diameter / 2;
            await _ctx.BeginPathAsync();
            await _ctx.SetFillStyleAsync(color);
            await _ctx.ArcAsync(x1 + radius, y1 + radius, radius, 0, 2 * Math.PI, false);
            await _ctx.FillAsync();
        }

        public async Task DrawString(string text, Font font, string color, double x, double y, TextAlign align = TextAlign.Center)
        {
            if (_ctx is null) 
                return;

            await font.Set();
            await _ctx.SetTextAlignAsync(align);
            await _ctx.SetFillStyleAsync(color);
            await _ctx.FillTextAsync(text, x, y);
        }
    }
}
