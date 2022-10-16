ChordCanvas
=======

ChordCanvas is a small C# library which provides an API for generating images of guitar chord diagrams on a HTML5 canvas. Based on https://github.com/acspike/ChordJS, which in turn was based on a .NET web service.

This library uses the implementation of a HTML5 canvas for Microsoft Blazor, [found here](https://github.com/BlazorExtensions/Canvas). This API does not provide a mechanism to create or retrieve chord objects, nor the actual canvas for drawing to. It should be paired with a web service or app that can construct the requested chord data, and the canvas context is passed as a parameter on a draw call.

## Documentation

### Syntax
A chord draw request is made by calling `CanvasAPI.CreateImage(Canvas2DContext ctx, Chord chord, Layout layout, double size)`. This function is marked as async and has no return value.

 - ctx: The context for the canvas on which the image is drawn. Full type signature is Blazor.Extensions.Canvas.Canvas2D.Canvas2DContext.

 - chord: The chord to be drawn. This object is explained below in further detail.

 - layout: The layout enum for the chord diagram.
		`'1'` would draw the finger names onto the strings and show the string names below the diagram. 
		`'2'` would draw the finger names below the chord diagrams and not dispaly the string names. 

 - size: The size of the chord diagram to generate. E.g.: `3` for medium size or `5` for a pretty big size.

### Chord
The layout of the chord object is heavily influenced by the UberChord API (https://api.uberchord.com/), as this is what was used to retrieve chords. 

 - Strings: A string containing the fret on each string for the chord, separated by spaces. E.g. 'X 3 2 0 1 0' for C major.

 - Fingering: Similar to strings, except the fingers to use on each string. E.g. 'X 3 2 X 1 X' for C major.

 - ChordName: A string containing 4 components separated by commas. The components are root note, quality, tension, and bass note. E.g. 'C,,,' for C major.

 - EnharmonicChordName: Same as ChordName, but the enharmonic equivalent chord name. E.g. 'A#,,,' for Bb major. 

 - VoicingID: Provides an ID for the chord shape (i.e. Am shape).

 - Tones: A list of the notes in the chord separated by commas. E.g. 'A,C#,E,G' for Amaj7.