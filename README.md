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
- Enemies: 1 type of mob enemy in a grey colour, plus a more difficult but slower black-coloured boss.
- A combat system which uses the player's sword to rotate and attack enemies and also monitors the player for enemy collisions (attacks).
- A forest environment with a linear path leading to the boss. This makes navigation easier.
- Menus: A main menu is implemented. A win menu is displayed when the final boss is killed. A game-over menu is displayed when the player dies. A pause menu is also accessible.
- Instructions can be found within the game's main menu.
- Accessibility feature: Total Recall is implemented - the user can pause the game and view game instructions at any point while playing.

## Attributions

### Assets

### Code

#### Tanur's attributions
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
  
3. PauseMenu.cs
  - https://www.youtube.com/watch?v=9dYDBomQpBQ helpful for pausing the game.

4. General C# Documentation Guidance.
   - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments assisted with proper C# documentation.
    
## Getting Started

Here's how to get the game running on your local machine.

### Installation

A step-by-step guide on how to install and set up your project:

1. Clone this repository:
   ```sh
   git clone https://github.com/hannanj0/GameDev.git
