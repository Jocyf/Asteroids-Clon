Asteroids Clon
==============


This is an almost perfect Asteroids Clon made using Unity3D.
You can play it here: http://www.jocyf.com/Preview/AsteroidsWebGL/index.html



![Screenshot](Asteroids.jpg)

Asteroids project notes:
------------------------

Asteroids uses a free tweener asset (DotTween). It's available for downnload in the Asset Store: 
https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
The Tween is used to perform fade when loading a new scene and to make the player ship to Blink.

This project only uses sprites (All sprites are contained in one file) and 2d physics.
The project uses two property attributes (Disable and InpectorButton) to improve the Inspector window view.


Project code notes:
-------------------
Managers.

GameManager -> Basic setup (framerate) and public LoadMainMenu functions.

LevelManager -> StartGame, PauseGame, Detect GameOver or LevelFinished. Manage players live and score and update this values on screen UI.

ScreenBoundsManager -> Calculate the screen bounds to tell if something is in/out the screen (Visible/Invisible). Some other helper functions.

AsteroidsManager -> Generates/Destroys the asteroids and always know the asteroids number left in game (so it can tell us when there isn't any at all)

UFOManager -> Creates/Destroy the UFO after waiting a period of time.

SceneWrapping:

AdvancedWrapping -> Used in the PlayerShip. Creates the playership ghosts so we can switch ship when we are in the screen edge.

ScreenCornerDetectorv1 -> Used in Asteroids and UFOs. Wrap the object when is in the screen edge (change its position to the other side of the screen)

ScreenCornerDetectorv2 -> Used in the PlayerShip. Wrap the ship when is in the screen edge (change its position to the other side of the screen)

Those to different versions of wrapping objects detects the moment to make the wrap in different ways (using different aproaches). 
Besides, One has a deathzone (used in the asteroids so they can dissapear from screen before wrapping them), the other (the player ship one) is inmediate.

Al the oher code enemies, playership, movement, fire , etc it's almost self explanatory.



License
-------
Unity project and source code license:
[MIT License](https://opensource.org/licenses/MIT).

Note: This project is coying the sprites and sounds from the original "Asteroids" game licensed to Atari Inc.
in order to create an Asteroids clon game for learning purposes only.




