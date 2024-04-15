using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;

namespace ChordCanvas
{
    internal class ChordDrawer
    {
        private GraphicsContext _Ctx;

        private double _Size;
        private string _ChordName = "";
        private List<int> _ChordPositions = new List<int>();
        private List<Chord.Fingers> _Fingers = Enumerable.Repeat(Chord.Fingers.NoFinger, 6).ToList();
        private List<string> _StringNames = new List<string> { "E", "A", "D", "G", "B", "e" };

        private const string _FontName = "Arial";
        private const int _FretCount = 5;

        private double FretWidth;
        private double LineWidth;
        private double BoxWidth;
        private double BoxHeight;

        private double ImageWidth { get; set; }
        private double ImageHeight { get; set; }
        private double xStart; //upper corner of the chordbox
        private double yStart;
        private double NutHeight;

        private double DotWidth;

        //Different font sizes
        private double FretFontSize;
        private double FingerFontSize;
        private double NameFontSize;
        private double SuperScriptFontSize;
        private double GuitarStringFontSize;
        private double MarkerWidth;

        private const string _ForegroundBrush = "#000";
        private const string _BackgroundBrush = "#FFF";

        private static int BaseFret;

        public ChordDrawer(Canvas2DContext ctx, Chord chord, double size)
        {
            _Ctx = new GraphicsContext(ctx);

            _ChordPositions = chord.FretList.ToList();
            _Fingers = chord.FingeringList.ToList();
            _ChordName = chord.ChordName;


            _Size = size;
            FretWidth = 4 * _Size;
            NutHeight = 0.5 * FretWidth;
            LineWidth = Math.Ceiling(_Size * 0.31);
            DotWidth = Math.Ceiling(0.9 * FretWidth);
            MarkerWidth = 0.5 * FretWidth;
            BoxWidth = 5 * FretWidth + 6 * LineWidth;
            BoxHeight = _FretCount * (FretWidth + LineWidth) + LineWidth;

            //Find out font sizes
            double perc = 2;
            FretFontSize = FretWidth / perc;
            FingerFontSize = FretWidth * 0.8;
            GuitarStringFontSize = FretWidth * 0.8;
            NameFontSize = (FretWidth * 2) / perc;
            SuperScriptFontSize = 0.7 * NameFontSize;
            if (_Size == 1)
            {
                NameFontSize += 2;
                FingerFontSize += 2;
                FretFontSize += 2;
                SuperScriptFontSize += 2;
            }

            xStart = FretWidth;
            yStart = Math.Round(0.2 * SuperScriptFontSize + NameFontSize + NutHeight + 1.7 * MarkerWidth) + 25;

            ImageWidth = (BoxWidth + 5 * FretWidth);
            ImageHeight = (BoxHeight + yStart + FretWidth + FretWidth);


            int minFret = int.MaxValue;
            int maxFret = 0;
            BaseFret = 1;

            foreach (var fret in _ChordPositions)
            {
                if (fret == -1)
                    continue;

                maxFret = Math.Max(fret, maxFret);

                if (fret == 0)
                    continue;

                minFret = Math.Min(fret, minFret);
            }

            if (maxFret > 5)
                BaseFret = minFret;

        }

        public async Task DrawChordBox()
        {
            await _Ctx.FillRectangle(_BackgroundBrush, 0, 0, ImageWidth, ImageHeight);

            Pen pen = _Ctx.CreatePen(_ForegroundBrush, LineWidth);
            double totalFretWidth = FretWidth + LineWidth;

            for (var i = 0; i <= _FretCount; i++)
            {
                double y = yStart + i * totalFretWidth;
                await _Ctx.DrawLine(pen, xStart, y, xStart + BoxWidth - LineWidth, y);
            }

            for (int i = 0; i < 6; i++)
            {
                var x = xStart + (i * totalFretWidth);
                await _Ctx.DrawLine(pen, x, yStart, x, yStart + BoxHeight - LineWidth);
            }

            if (BaseFret == 1)
            {
                //Need to draw the nut
                double nutHeight = FretWidth / 2;
                await _Ctx.FillRectangle(_ForegroundBrush, xStart - LineWidth / 2, yStart - nutHeight, BoxWidth, nutHeight);
            }
        }

        public async Task DrawBars()
        {
            Dictionary<Chord.Fingers, ChordBar> bars = new Dictionary<Chord.Fingers, ChordBar>();
            ChordBar bar = new ChordBar();

            for (var i = 0; i < 5; i++)
            {
                if (_ChordPositions[i] != -1 && _ChordPositions[i] != 0
                    && _Fingers[i] != Chord.Fingers.NoFinger && !bars.ContainsKey(_Fingers[i]))
                {
                    bar.String = i;
                    bar.Position = _ChordPositions[i];
                    bar.Length = 0;
                    bar.Finger = _Fingers[i];

                    for (int j = i + 1; j < 6; j++)
                    {
                        if (_Fingers[j] == bar.Finger && _ChordPositions[j] == _ChordPositions[i])
                        {
                            bar.Length = j - i;
                        }
                    }
                    if (bar.Length > 0)
                    {
                        bars[bar.Finger] = bar;
                    }
                }
            }

            //TODO: figure out why there are two pens here
            Pen pen = _Ctx.CreatePen(_ForegroundBrush, LineWidth * 3);
            double totalFretWidth = FretWidth + LineWidth;
            foreach (var b in bars)
            {
                bar = b.Value;
                double xstart = xStart + bar.String * totalFretWidth;
                double xend = xstart + bar.Length * totalFretWidth;
                double y = yStart + ((int)bar.Position - BaseFret + 1) * totalFretWidth - (totalFretWidth / 2);
                pen = _Ctx.CreatePen(_ForegroundBrush, DotWidth / 2);
                await _Ctx.DrawLine(pen, xstart, y, xend, y);
            }
        }

        public async Task DrawChordPositions()
        {
            double xpos = xStart + 0.5 * (LineWidth - FretWidth);
            double yoffset = yStart - FretWidth;
            foreach (var absolutePos in _ChordPositions)
            {
                int relativePos = absolutePos - BaseFret + 1;

                if (relativePos > 0)
                {
                    double ypos = relativePos * (FretWidth + LineWidth) + yoffset;
                    await _Ctx.FillCircle(_ForegroundBrush, xpos, ypos, DotWidth);
                }
                else if (absolutePos == 0)
                {
                    Pen pen = _Ctx.CreatePen(_ForegroundBrush, LineWidth);
                    double ypos = yStart - FretWidth;
                    var markerXpos = xpos + ((DotWidth - MarkerWidth) / 2);
                    if (BaseFret == 1)
                    {
                        ypos -= NutHeight;
                    }
                    await _Ctx.DrawCircle(pen, markerXpos, ypos, MarkerWidth);
                }
                else if (absolutePos == -1)
                {
                    Pen pen = _Ctx.CreatePen(_ForegroundBrush, LineWidth * 1.5);
                    var ypos = yStart - FretWidth;
                    var markerXpos = xpos + ((DotWidth - MarkerWidth) / 2);
                    if (BaseFret == 1)
                    {
                        ypos -= NutHeight;
                    }
                    await _Ctx.DrawLine(pen, markerXpos, ypos, markerXpos + MarkerWidth, ypos + MarkerWidth);
                    await _Ctx.DrawLine(pen, markerXpos, ypos + MarkerWidth, markerXpos + MarkerWidth, ypos);
                }

                xpos += FretWidth + LineWidth;
            }
        }

        public async Task DrawChordPositionsAndFingers()
        {
            double yoffset = yStart - FretWidth;
            Font font = _Ctx.CreateFont(_FontName, FingerFontSize);

            double xpos = xStart - 0.5 * (FretWidth - LineWidth);
            foreach (var pf in _ChordPositions.Zip(_Fingers, (absolutePos, finger) => (absolutePos, finger)))
            {
                int relativePos = pf.absolutePos - BaseFret + 1;

                if (relativePos > 0)
                {
                    double ypos = relativePos * (FretWidth + LineWidth) + yoffset;
                    await _Ctx.FillCircle(_ForegroundBrush, xpos, ypos, DotWidth);

                    if (pf.finger != Chord.Fingers.NoFinger)
                    {
                        await _Ctx.DrawString(pf.finger.ToString("d"), font, _BackgroundBrush, xpos + 0.5 * DotWidth, ypos + LineWidth);
                    }
                }
                else if (pf.absolutePos == 0)
                {
                    Pen pen = _Ctx.CreatePen(_ForegroundBrush, LineWidth);
                    double ypos = yStart - FretWidth;
                    var markerXpos = xpos + ((DotWidth - MarkerWidth) / 2);
                    if (BaseFret == 1)
                        ypos -= NutHeight;

                    await _Ctx.DrawCircle(pen, markerXpos, ypos, MarkerWidth);

                    if (pf.finger != Chord.Fingers.NoFinger)
                    {
                        await _Ctx.DrawString(pf.finger.ToString("d"), font, _BackgroundBrush, xpos + 0.5 * DotWidth, ypos + LineWidth);
                    }
                }
                else if (pf.absolutePos == -1)
                {
                    Pen pen = _Ctx.CreatePen(_ForegroundBrush, LineWidth * 1.5);
                    var ypos = yStart - FretWidth;
                    var markerXpos = xpos + ((DotWidth - MarkerWidth) / 2);
                    if (BaseFret == 1)
                    {
                        ypos -= NutHeight;
                    }
                    await _Ctx.DrawLine(pen, markerXpos, ypos, markerXpos + MarkerWidth, ypos + MarkerWidth);
                    await _Ctx.DrawLine(pen, markerXpos, ypos + MarkerWidth, markerXpos + MarkerWidth, ypos);

                    if (pf.finger != Chord.Fingers.NoFinger)
                    {
                        await _Ctx.DrawString(pf.finger.ToString("d"), font, _BackgroundBrush, xpos + 0.5 * DotWidth, ypos + LineWidth);
                    }
                }

                xpos += FretWidth + LineWidth;
            }
        }

        public async Task DrawFingers()
        {
            double xpos = xStart;
            double ypos = yStart + BoxHeight + 0.25 * DotWidth;
            Font font = _Ctx.CreateFont(_FontName, FingerFontSize);
            foreach (var finger in _Fingers)
            {
                if (finger != Chord.Fingers.NoFinger)
                {
                    await _Ctx.DrawString(finger.ToString("d"), font, _ForegroundBrush, xpos, ypos);
                }
                xpos += (FretWidth + LineWidth);
            }
        }

        public async Task DrawStringNames()
        {
            double xpos = xStart + (0.5 * LineWidth);
            double ypos = yStart + BoxHeight;
            Font font = _Ctx.CreateFont(_FontName, GuitarStringFontSize);
            foreach (string guitarString in _StringNames)
            {
                await _Ctx.DrawString(guitarString, font, _ForegroundBrush, xpos, ypos);
                xpos += (FretWidth + LineWidth);
            }
        }

        public async Task DrawChordName()
        {
            Font nameFont = _Ctx.CreateFont(_FontName, NameFontSize);
            Font superFont = _Ctx.CreateFont(_FontName, SuperScriptFontSize);
            string name;
            string supers;
            if (!_ChordName.Contains("_"))
            {
                name = _ChordName;
                supers = "";
            }
            else
            {
                var parts = _ChordName.Split("_");
                name = parts[0];
                supers = parts[1];
            }

            double xTextStart = xStart + 0.5 * BoxWidth;
            await _Ctx.DrawString(name, nameFont, _ForegroundBrush, xTextStart, 0);
            if (supers != "")
            {
                await _Ctx.DrawString(supers, superFont, _ForegroundBrush, xTextStart, 0);
            }

            if (BaseFret > 1)
            {
                Font fretFont = _Ctx.CreateFont(_FontName, FretFontSize);
                double offset = (FretFontSize - FretWidth) / 2;
                await _Ctx.DrawString(BaseFret + "fr", fretFont, _ForegroundBrush, xStart + BoxWidth + 0.4 * FretWidth, yStart - offset, TextAlign.Left);
            }
        }
    }
}
