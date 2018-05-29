
// Some explanation on the data format. You are gonna need it !.
// For uncle Bob. I didn't invent the format. It has no "Clean Code" approval.
/*
 *                                  BoundingBoxWidth
 *                               ------------------
 *                              |1                 |
 *                              |0                 |
 *                              |9   *       *     |
 *                              |8   *       *     |
 *                              |7   *       *     |
 *                              |6   *********     |
 *                              |5   *       *     | BoundingBoxHeight
 *                              |4   *       *     |
 *                              |3   *       *     |
 *                              |2   *       *     |
 *                              |1                 |
 *                              |012345678901234567|
 *                               ------------------
 * 
 *  The Glyph (character) (0,0) is BOTTOM left
 *  The Drawing AND Data (0, 0) is TOP left.
 
 *  BoundingBoxWidth and BoundingBoxHeight are font properties.
 *  This is the reference size in pixels of this font, and for all the Glyphs in this font the same. 
 * 
 *  BitMapWidth and BitMapHeight are Glyph properties. 
 *  They represent size in pixels of this glyph. It's should be a minimum bounding box.
 *  
 *  BitMapOffsetX and BitMapOffsetY are Glyph properties. 
 *  They represent the offset between the origin of the BoundingBox and Glyph.
 *  
 *  BitMapPitch is a Glyph property. 
 *  It represent the horizontal spacing of this glyph.
 *  
 *  So in this example:
 *  BoundingBoxWidth = 18
 *  BoundingBoxHeight = 12
 *  BitMapWidth = 9
 *  BitMapHeight = 8
 *  BitMapOffsetX = 4
 *  BitMapOffsetY = 2
 *  BitMapPitch = 15 (probably)
 *  
 *  Glyph Data.
 *  The data are stored as bits. Bits are stored compressed into a stream.
 *  Starting on the top left corner of the glyph. Working to the right side.
 *  It automaticly goes to the left of the next line when it reaches the end of this line.
 *  The uncompressed stream of this example would be 80bits (BitMapWidth * BitMapHeight). 
 *  100000001100000001100000001111111111100000001100000001100000001100000001
 *  Glyph compresseion
 *  Data are stored in a pattern that will be repeated if possible.
 *  In the header of the font m0 and m1 is defined. It defines the number of bits that are used for the pattern.
 *  the patter is "NrOfZeros" followed by "NrOfOnes" Followed by Repeatbit (1 repeat, 0 next pattern)
 *  In this example m0 and m1 could be set at 3. So 3 bit's are used nr "NrOfZeros" and 3 bits for "NrOfOnes".
 *  Let's try this on this example, starting at the top left of the bitmap again.

 *  First bit is '1'. So 0 Zero's. and 1 Ones. This results in compressed stream
 *  000 001 0   In this pattern one 1 bit is stored. Not much of compression yet.

 *  Next we have 7 zero's and 2 one. This can be stored as
 *  111 010
 *  It also possible to repeat this pattern 2 times more (3 in total).
 *  110 (every 1 is repeat, 0 is next pattern)
 * 
 *  Next are 0 zeros and 9 ones. Make a 3 ones code, that repeats 3 times:
 *  000011110
 *  
 *  Then we have 7 zero's followed by 2 ones. Also 
 *  1110101110
 *  
 *  Uncompressed stream alligned to the Compressed stream:
 *  
 *  1           000000011 000000011 000000011   111 111 111   000000011 000000011 000000011 00000001
 *  |           |         |         |           |   |   |     |         |         |         |
 *  |           |       *-*         |           |   |   **    |       *-*         |         |
 *  |           |       |           |           |   |    |    |       |*----------*         |
 *  |           |       |*----------*           |   *---*|    |       ||*-------------------*
 *  |           |       ||                      |       ||    |       ||| 
 *  000 001 0   111 010 110                     000 011 110   111 010 1110
 *  
 */

