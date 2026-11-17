# Lecture 8: generator-room escape

Open `Lecture8/Lecture8.uproject` with Unreal Engine **5.8.4**. The starting map is `/Game/Lecture08/Maps/L_GeneratorRoom`.

Find the battery, carry it to the generator, restore power and leave through the exit.

- WASD: move. Mouse: look. Space: jump.
- E: equip the optional lamp near the entrance, pick up the battery, insert it or open the powered exit.
- The equipped lamp hides while carrying the battery and returns after insertion.
- Hold F for 1.5 seconds beside the generator after inserting the battery. Releasing early or walking away cancels charging.
- R: restart the normal map, restoring health and the puzzle.

`L_GeneratorRoom_Debug` is an optional map with one deliberately unassigned lamp reference. R returns to the healthy normal map.
