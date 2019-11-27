# CelesteTAS
Simple TAS Tools for the game Celeste

## Installation

### Everest

The easiest version to install is the [Everest version](https://github.com/EverestAPI/CelesteTAS-EverestInterop). 

- Install [Everest](https://everestapi.github.io/) if you haven't already.
- Use the 1-click installer [here.](https://gamebanana.com/tools/6715)
- Enable TAS in the mod settings.
- Download [Celeste Studio](https://github.com/ShootMe/CelesteTAS/releases/download/TAS/Celeste.Studio.exe), our input editor. (Note that Studio is Windows-only.)

### Syncing the 100% TAS
- Copy the contents of the LevelFiles folder into the main Celeste directory.
- If you haven't unlocked Variants (completed 8C), uncomment the read command on line 161 of 0 - 100%.tas
- Either rename 0 - 100%.tas to Celeste.tas or open 0 - 100%.tas in Celeste Studio. (Ctrl + O)
- Use Celeste version 1.3.1.2 (current patch as of writing)
- Any build of Celeste should work, although we have not tested on Mac.
- Disable "Show Mod Options In Game" in Mod Options in the main menu.
- From "Begin" on the first save file, press RightCtrl + [
- If you have a non-US keyboard, you can change the keybinding for KeyStart in gamefolder/Saves/modsettings-CelesteTAS.celeste
