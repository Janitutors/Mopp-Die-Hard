\# Mopp Die Hard



Small 2D top-down arcade survival game made with Unity.



\## Game Overview



You play as a half-crazy guy in white boxer shorts stuck on a rooftop, armed only with a mop.  

Aliens fall from the sky. Toxic barrels crash onto the roof. Chaos increases over time.



There are no levels. No progression. No upgrades.



Survive longer. Beat your high score.



---



\## Core Gameplay Loop



Move -> Fight -> Survive -> Take Damage -> Die -> Restart



---



\## Sorting Layer Standard (Fake 45° Top-Down)



Sorting Layers:



1\. Ground  

&nbsp;  Rooftop tilemap, floor markings, borders.



2\. Shadow  

&nbsp;  Character and enemy shadows (semi-transparent).



3\. Entities  

&nbsp;  Player, enemies, barrels, interactive objects.



4\. VFX  

&nbsp;  Attack effects, explosions, acid splashes, impact effects.



5\. UI  

&nbsp;  All canvas-based UI elements.



Planned rule (Day 3/4):



sortingOrder = -(int)(transform.position.y \* 100) + offset



Lower on screen = drawn on top.

