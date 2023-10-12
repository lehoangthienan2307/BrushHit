# Brush Hit
A 3D game consists of multiple levels, each with 3-4 rounds. The player controls a 
device with two parts: an anchor hub and a rotating hub that can rotate 360 
degrees. When the player clicks or taps, the rotating hub stops, and it's the anchor
hub's turn to rotate. The player's task is to color all rubbers within the round and 
can only move on the positions of these objects. If the player moves off the 
platforms, they lose. The platforms can move, and there are enemy AI that attempt 
to revert the colored objects back to their original state. The score is determined by 
the number of rubbers the player successfully colors, with additional score
awarded for creating combos through consecutive successful colorings. The player 
can retry a round at any time

![Game Play Image](/Image/BrushHit.png)
The player can use the button on the right to extend the coloring range, available 
once per round. Ideally, there should be a cooldown period and a duration for the 
effect, but I wasn't able to implement it in time. I'll add these features later.