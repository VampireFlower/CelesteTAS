# CelesteTAS
Simple TAS Tools for the game Celeste

## Installation

### Everest

The easiest way to install is through the [Everest interop mod](https://github.com/EverestAPI/CelesteTAS-EverestInterop). 

- [Download the zip from here](https://github.com/EverestAPI/CelesteTAS-EverestInterop/releases)
- Place it in your `Mods` directory
- [Download the TAS addon here](https://github.com/ShootMe/CelesteTAS/releases), either Celeste-Addons-OpenGL.dll or Celeste-Addons-XNA.dll, whichever corresponds with your version of Celeste
- Place it in the same directory as `Celeste.exe`, and rename it to `Celeste-Addons.dll`
- Enable TAS in the mod settings.

### Manually

- Hey you know what would be great? Installing the TAS tools manually. You know what wouldn't suck? Installing it with Everest.
- Follow the Everest instructions.
- Go to Celeste.AreaComplete.Begin and remove the identicon.

For playback to be correct, make sure Jump is bound to 'A' and 'Y', Dash is bound to 'B' and 'X', Grab is bound to 'RB', Quick Reset is bound to 'LB', and talk is bound to 'B'. If you don't have a controller, you have to edit the settings file manually. Here's the relevant section:

```
  <BtnGrab>
    <Buttons>RightTrigger</Buttons>
    <Buttons>RightShoulder</Buttons>
  </BtnGrab>
  <BtnJump>
    <Buttons>A</Buttons>
    <Buttons>Y</Buttons>
  </BtnJump>
  <BtnDash>
    <Buttons>X</Buttons>
    <Buttons>B</Buttons>
  </BtnDash>
  <BtnTalk>
    <Buttons>B</Buttons>
  </BtnTalk>
  <BtnAltQuickRestart>
    <Buttons>LeftTrigger</Buttons>
    <Buttons>LeftShoulder</Buttons>
  </BtnAltQuickRestart>
```

## Input File
Input file is called Celeste.tas and needs to be in the main Celeste directory (usually C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.tas)

Format for the input file is (Frames),(Actions)

ie) 123,R,J (For 123 frames, hold Right and Jump)

## Actions Available
- R = Right
- L = Left
- U = Up
- D = Down
- J = Jump / Confirm
- K = Jump Bind 2
- X = Dash / Talk
- C = Dash Bind 2
- G = Grab
- S = Start
- Q = Quick Reset
- F = Feather Aim
- O = Confirm
- N = Journal (Used only for Cheat Code)

## Special Input
- You can create a break point in the input file by typing *** by itself on a single line
- The program when played back from the start will try and go at 400x till it reaches that line and then go into frame stepping mode
- You can also specify the speed with ***X where X is the speedup factor. ie) ***10 will go at 10x speed

- Read,Relative File Path,Starting Line
- Will Read inputs from the specified file.
- ie) Read,1A - Forsaken City.tas,7 will read all inputs after line 7 from the '1A - Forsaken City.tas' file

## Playback of Input File
### Controller
While in game
- Playback: Right Stick
- Stop: Right Stick
- Faster Playback: Right Stick X+
- Frame Step: DPad Up
- While Frame Stepping:
  - One more frame: DPad Up
  - Continue at normal speed: DPad Down
  - Frame step continuously: Right Stick X+

### Keyboard
While in game
- Playback: RightControl + [
- Stop: RightControl + [
- Faster Playback: RightControl + RightShift
- Frame Step: [
- While Frame Stepping:
  - One more frame: [
  - Continue at normal speed: ]
  - Frame step continuously: RightControl + RightShift
  
## Celeste Studio
Can be used instead of notepad or similar for easier editing of the TAS file. Is located in [Releases](https://github.com/ShootMe/CelesteTAS/releases) as well.

If Celeste.exe is running it will automatically open Celeste.tas if it exists. You can hit Ctrl+O to open a different file, which will automatically save it to Celeste.tas as well. Ctrl+Shift+S will open a Save As dialog as well.
