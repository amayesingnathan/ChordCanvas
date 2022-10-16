using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordCanvas
{
    internal class ChordBar
    {
        public int String { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public Chord.Fingers Finger { get; set; }
    }
}
