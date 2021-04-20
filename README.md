# VineetKumar_GAME3002_A3
 Platformer game - Final Assignment for Game Physics

This is the final assignment for Game Physics with all the requirements met: A platformer game with different aspects of physics laws and Unity’s own physics engine combined.

There are three scenes, and the player is greeted with a main menu scene, which also contains basic instructions of the game’s content. The player can start the game by pressing the start button. After successful completion of level, there is an end scene with option to retry, quit to main menu, and quit to desktop.

The controls of the game are quite simple – 
-	Left/Right arrow keys for moving left/right.
-	Spacebar key for jumping.
-	E button to open the door (only if you possess the key)
-	R button for resetting the game anytime in-game.
-	ESC button to quit the game (to desktop) anytime in-game.


# Elements of the game
-	The player has a **spring-arm camera**, like the one in UE4, that follows the player, with lag in 2 main forms. When player is moving the small lag and a zoom-out is noticeable, but when player jumps, there is remarkable lag and zoom-out. This is to give the player a better idea and UX of the overall level before moving/jumping ahead.
-	3 environments – Concrete, bouncy and icey each with platforms having the respective physics materials.
-	Accessing second & third environment requires key because those are locked with a door.
-	There are **3 Traps with Torque**
-	There are **2 unstable platforms** that rely on **spring** recoil and player’s mass to direct the player correctly.
-	**3 Speed-Up zones** that uses trigger.
-	**1 Slow-Down zone** that uses trigger.
-	There are a ton of **spikes**, on platforms and on rotating cylinders kept at the edges of the level.

When the player progresses through the level, the UI presents **hints** for a small duration, wherever it was felt as required.

Whenever the player acquires a key by landing on the key platform, there is a rotating cube on the player’s head to indicate that the player possesses a key. If the player tries to access a door without a key, there be a red light and a prompt message stating that a key is required. With a key, the door light will be green, and a prompt will appear that will state that the door can be opened with the activate button.

There is trophy cup at the end of the level which when collected will mark the end of level.

However if the timer runs-out before finishing the level, it will auto-restart.
