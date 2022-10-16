using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;

namespace ChordCanvas
{
    internal class Font
    {
        public Font(Canvas2DContext ctx, string fname, double size)
        {
            _ctx = ctx;
            _fname = fname;
            _size = size;
        }

        private readonly Canvas2DContext _ctx;
        private readonly string _fname;
        private readonly double _size;
        public async Task Set()
        {
            if (_ctx is null) return;

            await _ctx.SetFontAsync(_size + "px " + _fname);
            await _ctx.SetTextBaselineAsync(TextBaseline.Top);
        }
    }
    internal class FontSize
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
