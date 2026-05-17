# Unity Slot Machine Game

A polished slot machine game developed in Unity as part of a game development assignment.

## Features

* Smooth reel spinning animation
* Randomized slot outcomes
* Betting and balance system
* Winning payout system
* Reel stop sound effects
* Background casino music
* Sequential reel stopping
* Win/Lose feedback system

## Winning Logic

Player wins when all 3 visible symbols match.

### Payout Multipliers

| Symbol | Multiplier |
| ------ | ---------- |
| Seven  | x10        |
| Bell   | x5         |
| Cherry | x3         |
| Bar    | x2         |

## Controls

* Increase Bet → Increase current bet amount
* Decrease Bet → Decrease current bet amount
* Lever → Start spinning the slot reels

## WebGL Build

The WebGL build is available inside:

Build/WebGL/

## Instructions to Run WebGL Build

1. Navigate to the `Build/WebGL` folder.

2. Open the folder using a local development server.

3. Recommended method:
   - Open the folder in Visual Studio Code
   - Install the "Live Server" extension
   - Right-click `index.html`
   - Select `Open with Live Server`

4. The game will launch automatically in the browser.

> Note: Unity WebGL builds may not run correctly when opening `index.html` directly because modern browsers restrict local WebGL file access.

## Folder Structure

Assets/

* Scripts/
* Animations/
* Sounds/
* Sprites/

## Bonus Features

* Betting system
* Balance system
* Audio feedback
* Popup feedback system
* Background music
* Restart functionality

## Approach

The project was designed with a modular structure using separate managers for reels, audio, and gameplay logic. The focus was to create a lightweight but polished slot machine experience with smooth animations and responsive audio/UI feedback.
