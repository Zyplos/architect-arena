# Architect Arena

A Unity game where a team of players try to escape a level created by another player that oversees all.

Originally made for the University of Illinois Chicago's CS426: Video Game Design class.

## Assignment 6 Checkpoint

This game's level layout is created by the Architect player, but we still have a hand on the design decisions of how they construct the level. Each level is made up of components called Level Blocks. There are different unique types of Level Blocks, each with their own unique properties.

To make it easy for the Architect to make their layout, these Level Blocks are shaped like Cubes so they can be arranged in a grid. They are meant to connect to each other, so each Level Block is has an exit archway on each side that can connect to another Level Block. Every Level Block's exterior is the same, but the interior is different depending on the type of Level Block, so the Architect can mix and match traps and items to create a challenging level.

The following AI constructs and Mecanim animations were implemented for this checkpoint:

- (Angel) The Pet in the LobbyRoom uses NavMesh to follow players
- (Angel) The Pet in the LobbyRoom uses Mecanim animations to show if its chasing or idle
- (Filip) The Rat in the PuzzleRoom used NavMesh to navigate room and run away from player when it gets close
- (Filip) The Character model in the PuzzleRoom uses Mecanin animations to IDLE, Run, and Jump

The following changes were made including sound and UI designs ( ASGN 7 )

- Made the join code more readable by adding contrast and a shadow effect to it
- Repositioned the UI buttons to make it look more sleek and modern
- Also added color, highlighting and shadows to buttons and text fields
- Added menu music
- Added button sound effects
- Added ambient sound to puzzle room
- Added door and fire sound effects to puzzle room
- Resized puzzle room to fit level template
- Added more varied pressure pads and doors to puzzle room design

## Assignment 8 Checkpoint

Several critical bugs that Alpha testers pointed out were also fixed.
The Architect had issues placing down Level Blocks, sometimes two would spawn at the same time, or one would spawn inside another.
The highlight showing where the Level Block would be inconsistent, this was fixed as well. It has also been changed to be more visible.
There was a hint text that would show what the next Level Block being placed would be.
This was removed, which is fine since this information is repeated on the side of the screen.
