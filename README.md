# Group AI - Unity Game Development Prototype

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Attributions](#attributions)
  - [Assets](#assets)
  - [Code](#code)
- [Getting Started](#getting-started)
  - [Installation](#installation)

## Introduction

Welcome to group AI's game prototype! This project explores concept #1 which is a survival, crafting game. 
The player must face difficult enemies on their journey to the final boss and beat it to win the game. 
Many core concepts were used to make up this game prototype, as you will see in the next section.

## Features

Here are some key concepts and features included in our prototype:

- Player stats to keep track of player health, hunger level, attack damage, and movement speed.
- An inventory system to pick up items discovered on the player's journey.
- Pickups: food items, health items, attack-enhancing items.
- A crafting system which currently allows the crafting of health potions.
- Enemies: 1 type of mob enemy in a grey colour, plus a more difficult but slower black-coloured boss.
- A combat system which uses the player's sword to rotate and attack enemies and also monitors the player for enemy collisions (attacks).
- A forest environment with a linear path leading to the boss. This makes navigation easier.
- Menus: A main menu is implemented. A win menu is displayed when the final boss is killed. A game-over menu is displayed when the player dies. A pause menu is also accessible.
- Instructions can be found within the game's main menu.
- Accessibility feature: Total Recall is implemented - the user can pause the game and view game instructions at any point while playing.

## Attributions

### Assets

### Code Snippets
    
#### Tanur's attributions
Note: Most of these are sources used for inspiration and understanding concepts and bugs better, so no code is copied directly.
Implementing the flashing red colour in WeaponAttack.cs is more closely followed on the other hand.
1. WeaponAttack.cs
   - https://discussions.unity.com/t/adding-a-cooldown-time-to-a-attack/140231/3 assisted with configuring our game's attack and cooldowns.
   - https://www.youtube.com/watch?v=3aWgstSctMw assisted with making the enemy flash red.

2. WeaponRotation.cs
   
   Below are some resources I consulted which helped with Slerp usage, understanding normalised angle rotations and ensuring smooth rotations for the player's sword:
   - https://www.youtube.com/shorts/jAN2IoWdPzM?feature=share
   - https://stackoverflow.com/questions/53106326/quaternion-slerp-not-smoothly-rotating-for-some-reason
   - https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
   - https://www.reddit.com/r/Unity3D/comments/lqmb8p/quaternion_lerpslerp_snapping_instead_of_rotating/
   - https://discussions.unity.com/t/quaternion-slerp-in-a-coroutine/33180
  
4. PauseMenu.cs
   - https://www.youtube.com/watch?v=9dYDBomQpBQ helpful for the foundations of my pause menu object and its functionality.

5. General C# Documentation Guidance.
   - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments assisted with proper C# documentation.

#### Hugos's attributions

Note: Most of these are sources used for inspiration and understanding concepts and bugs better, so no code is copied directly. Implementing the flashing red colour in WeaponAttack.cs is more closely followed on the other hand.

1. PlayerMovement.cs

    - [https://www.youtube.com/watch?v=vFV0uJ0KE2Q](https://www.youtube.com/watch?v=vFV0uJ0KE2Q) assisted with using the new input system with movement.
    - https://www.youtube.com/watch?v=q-VfsQQlji0 - overview video on creation of a basic movement system
      
2. ThirdPersonCameraController.cs

	- https://www.youtube.com/watch?v=04EpnVbMKpU - fundamentals of implementing the camera system with new input system, relative positioning of the camera
	- https://www.youtube.com/watch?v=qnjKoTmko3Q - quick overview of how camera systems are made and work.
 
3. Playercontrols.cs (inputactions)

	- https://www.youtube.com/watch?v=Yjee_e4fICc - explained how to configure and use for other scenarios
	- Videos on QMplus (guided on the initial creation)

4. General C# Documentation Guidance.
   - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments assisted with proper C# documentation.

#### Hannan's attributions
Note: Most of these are sources used for inspiration and understanding concepts and bugs better, so no code is copied directly.
Implementing the advanced enemy movement AI in AIController.cs is more closely followed on the other hand.

1. MainMenu.cs
   - https://www.youtube.com/watch?v=-GWjA6dixV4 - Helped my understanding of creating a main menu scene, and switching to the main scene via the code and configuring the build settings in order for this to work.

2. HealthBar.cs + HungerBar.cs + PlayerState.cs
   
   The following were used as inspiration to create the Status bars for the player:
   - https://www.youtube.com/watch?v=k2H3cJL1M9A 
   - https://www.youtube.com/watch?v=_lREXfAMUcE
   - https://discussions.unity.com/t/health-bar-goes-down-with-time/191485 - Helpful for implementation of hunger bar decreasing over time.
  
4. AIController.cs
   - https://www.youtube.com/watch?v=6Ai0xg6xTUk - This helped with ensuring the enemy is looking the correct way with the eyes
   - https://www.youtube.com/watch?v=ieyHlYp5SLQ - This assisted with the enemy patrolling, viewing angles and when to chase the player as well as avoiding obstacles
   - I also used the module resources to help with the NavMesh stuff needed for the patrolling and movement of enemy AI

5. General C# Documentation Guidance.
   - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments assisted with proper C# documentation.
    
## Getting Started

Here's how to get the game running on your local machine.

### Installation

1. Clone this repository:
   ```sh
   git clone https://github.com/hannanj0/GameDev.git
