# M6A2 ADATS v1.2

This mod replaces the M2 Bradley with the M6 Linebacker but pretend it has the GAU-12 and ADATS instead!

## Features:

<p>
	<ul> 
		<li>Converts M2 Bradley to a hypothetical M6A1/A2 ADATS variant</li>
		<li>Replaces the vanilla 25mm M242 Bushmaster autocannon with an improved M242 or 25mm GAU-12/U Equalizer rotary cannon</li>
		<li>GAU-12/U Equalizer: 3600 RPM and 1500 rounds (300 AP/1200 APHE)</li>
		<li>Designated as M6A1 ADATS when using improved M242, while it's designated as M6A2 ADATS when using GAU-12</li>
		<li>Automatic gun lead calculation (like the Abrams) and optional reticle horizontal stabilization</li>
		<li>Replaces the BGM-71C I-TOW with the MIM-146 ADATS</li>
		<li>M791 APDS-T: 1345 m/s velocity and 60 mm RHA penetration</li>
		<li>M919 APFSDS-T: 1390 m/s velocity and 102 mm RHA penetration</li>
		<li>APEX APHE-T: 1270 m/s velocity and 35 mm RHA penetration</li>
		<li>M920 MPAB-T: 1270 m/s velocity, 15 mm RHA penetration and a constant time-delay airburst fuze </li>
		<li>MIM-146 ADATS: 510 m/s velocity, 1000 mm RHA penetration, proximity fuze toggle and 10 km max range. 4 ready to launch (imagine two tubes on each side of the turret) and 12 stowed</li>
		<li>AP option between M791 and M919, and HE option between APEX and MPAB</li>
		<li>Option for ADATS tandem warhead</li>
		<li>Option for super optics (main/thermals)</li>
		<li>Option for better vehicle dynamics (engine/transmission/suspension/tracks)</li>
		<li>Option for better AI (faster spotting and improved AI gun accuracy</li>
		<li>Option for composite hull and turret that provides 50% more protection with no weight penalty</li>
	</ul>
</p>

## Installation:
1.) Install [MelonLoader v0.6.1](https://github.com/LavaGang/MelonLoader/).

2.) Download the latest version from the [release page](https://github.com/Cyances/M6A2-ADATS/releases).

3.) Place zM6A2Adats.dll file in the mods folder:

4.) Launch the game directly (not from Steam).
   
5.) On first time running this mod, the entries in MelonPreferences.cfg will only appear after launching the game then closing it.


## How to use the M920 MPAB-T and MIM-146 ADATS:
### MPAB Point-detonate + Time-delay Fuze

- To use airburst mode, simply laze the target. The round will detonate araound the distance set by LRF/manual elevation.
- To use point-detonate mode, make sure the range setting is at least 10 meters more than the target to ensure it would not be in airburst mode. As long as the round directly hits the target, it will use the point-detonate fuze.


### ADATS Point-detonate + Proxmity Fuze
- To use proximity mode, press middle mouse button and the round should have [Proximity] suffix to its name in the lower left part of the UI
- To use point-detonate mode, make sure the [Proximity] suffix is not present

## Mod Configuration (in UserData/MelonPreferences.cfg):

<p>
	<ul> 
		<li>I suggest getting Notepad++ so it would be easier to identify each category</li>
		<li>Use GAU-12 (true by default)</li>
		<li>Use M919 (false by default)</li>
		<li>Use M920 MPAB (false by default)</li>
		<li>AP and HE round count (300/1200 by default). If total round count exceeds 1500, the mod will use the default mix.</li>
		<li>Enable ADATS tandem warhead (false by default)</li>
		<li>ADATS proximity fuze sensitivity (3 by default)</li>
		<li>Reticle horizontal stabilization when leading (false by default)</li>
		<li>Super optics (false by default)</li>
		<li>Better vehicle dynamics (false by default)</li>
		<li>Better AI spotting and gunnery skill (false by default)</li>
		<li>Composite hull (false by default)</li>
		<li>Composite turret (false by default)</li>
	</ul>
</p>

![ADATS MelonPreferences](https://github.com/Cyances/M6A2-ADATS/assets/154455050/cc844e91-b272-4593-99e4-68e2bd2895b4)

Special thanks to Swiss (https://github.com/SovGrenadier) for allowing the forking of this mod and ATLAS (https://github.com/thebeninator) for assisting with some of the code.
