namespace ChordCanvas
{
    public class Chord
    {
        public enum Fingers
        {
            NoFinger = -1,
            Thumb,
            Index,
            Middle,
            Ring,
            Little
        }

        public string Strings { get; set; } = string.Empty;
        public IEnumerable<int> FretList => Strings.Split(" ").Select(
            stringName => (stringName.Equals("X") ? -1 : Int32.Parse(stringName)));
        public string Fingering { get; set; } = string.Empty;
        public IEnumerable<Fingers> FingeringList => Fingering.Split(" ").Select(
            fingerName => (Fingers)(fingerName.Equals("X") ? -1 : Int32.Parse(fingerName)));
        public string ChordName { get; set; } = string.Empty;
        public string EnharmonicChordName { get; set; } = string.Empty;
        public string VoicingID { get; set; } = string.Empty;
        public string Tones { get; set; } = string.Empty;

    }
}
