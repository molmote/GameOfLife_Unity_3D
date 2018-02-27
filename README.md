# GameOfLife_Unity_3D
Jeongmun(Justin) Ji, Feb/26/2018
------------------------------------------------------------------------------

Game of Life
-------------
Write a program that takes as input a board preset of size M x N and produce life pattern with rules below
Any live cell with fewer than two live neighbours dies.
Any live cell with two or three live neighbours lives.
Any live cell with more than three live neighbours dies.
Any dead cell with exactly three live neighbours becomes a live cell.
+ 3D version of the game

Memory Restrictions
-------------------
- Dynamically allocated memory for each cell(cube)
- Little bit of memory overhead due to N^3 cube especially on huge map (100^3)

Complexity and performance in Common Cases 
-------------------------
Current solution works pretty well for 40*40*10 sized map
The base logic for game of life ensures O(N^3) performance where N is width,height or depth of the map. The fps in using Unity Profiler shows 60~100FPS under Intel i5 2.90GHz 12GB memory with no additional graphics card installed(Intel HD 630). CPU usage is 13% at worst and GPU usage is around 23% (using windows task manager)

Implementations 
---------------
I started with the intention to use shader to render cells since having massive amoung of blocks in 3D space could cause performance issue in both rendering and memory management. Unfortunately, I wasn't able to convert it on time so I just decided to stick to cube generations. The downside of having N^3 cubes at runtime is always performance. However by Instinating all the cubes at the beginning of the game and constantly setting them on and off, I got pretty good result in small maps (20^3).

I've sticked to the original rule even in 3D version but seeing other people doing it, there are so many creative ways to do it just didn't have much time. 

I've decided to load maps from text file but there could have been other ways to do it. What I would've done if time was enough was to make another UI to get input from the user directly on the screen. (Just as they do: http://www.cuug.ab.ca/dewara/life/life.html) 
The way I'm doing is pretty much straight foward but couldn't think of better way to represent 3D map on a text file.
4 - width
4 - height
1001 - 1 is alive and 0 is dead
0110
0000
1010

It would have been great if I was able to color cells based on its lifetime (1frame - white, 2frame - yellow, and so on) or having an complete control of camera in game. I think I would work on them when time permits.


Interface
---------
Select Board object from the Hierarchy
On the GridControl component, 
Change "map to load" to change maps
Change "is3D" to Toggle between 2D/3D
Click Play button to launch the game


References
----------
https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life
https://www.youtube.com/watch?v=23MBR2pZoDQ Game Of Life - Basic Trailer
https://www.youtube.com/watch?v=MDAt2hHtXSY Game Of Life - 3D implementation
http://www.cuug.ab.ca/dewara/life/life.html Nice web simulation of 2D Game of Life
