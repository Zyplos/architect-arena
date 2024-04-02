# Architect Arena

A Unity game where a team of players try to escape a level created by another player that oversees all.

Originally made for the University of Illinois Chicago's CS426: Video Game Design class.

## Assignment 6 Checkpoint

This game's level layout is created by the Architect player, but we still have a hand on the design decisions of how they construct the level. Each level is made up of components called Level Blocks. There are different unique types of Level Blocks, each with their own unique properties.

To make it easy for the Architect to make their layout, these Level Blocks are shaped like Cubes so they can be arranged in a grid. They are meant to connect to each other, so each Level Block is has an exit archway on each side that can connect to another Level Block. Every Level Block's exterior is the same, but the interior is different depending on the type of Level Block, so the Architect can mix and match traps and items to create a challenging level.

The following AI constructs and Mecanim animations were implemented for this checkpoint:

- (Angel) The Pet in the LobbyRoom uses NavMesh to follow players
- (Angel) The Pet in the LobbyRoom uses Mecanim animations to show if its chasing or idle
