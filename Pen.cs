using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;

namespace ChordCanvas
{
    internal class Pen
    {
        public Pen(Canvas2DContext ctx, string color, double size)
        {
            _ctx = ctx;
            _color = color;
            _size = size;
        }

        private readonly Canvas2DContext _ctx;
        private readonly string _color;
        private readonly double _size;
        public async Task Set()
        {
            if (_ctx is null) return;

            await _ctx.SetStrokeStyleAsync(_color);
            await _ctx.SetLineWidthAsync((float)_size);
            await _ctx.SetLineCapAsync(LineCap.Round);
        }
    }
}
