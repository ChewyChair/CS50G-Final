# SpaceWar (CS50G Final Project)
#### Video Demo: https://youtu.be/92F-6dCY1Oc

## Description
This is my final project for CS50G built in Unity. At this stage it is a very simple game with only one level. I started this project with the goal of trying to implement a combat system similar to Starsector from scratch. This project does not use any external resources.

Complexity: Although the game only has one level, the turret system was unfortunately far more challenging than anything I had to implement in CS50G on its own. Furthermore, everything in this project was done from scratch, and I rewrote some systems multiple times.
<br>

### Features:
A player controlled ship that moves towards where the player clicks.

Ships with simple AI. Strikecraft are an extension of that, being mini-ships that spawn from hangar bays.

Turrets with firing arcs and rudimentary AI. There is also a distinction between the normal turrets and CIWS turrets, which prioritise strikecraft and missiles.

Missiles with simple AI that home towards their target. Not very obvious in the game because all the ships are so slow.

Simple camera movable with arrow keys and scrolling the mouse wheel to zoom in and out.

## Ships:
*Alien Fighter:* 
1 fixed mini alien missile launcher

*Alien Cruiser:*
12 alien missile launchers, 3 on each arm, 270 degrees firing arc.

*Human Fighter:*
1 fixed CIWS cannon

*Human Destroyer:*
- 1 cannon turret, 270 degrees firing arc
- 2 CIWS turrets, 1 on each wing, 270 degrees firing arc
- 1 missile launcher, 180 degrees firing arc

*Human Cruiser:*
- 4 fixed frontal autocannons
- 2 autocannon turrets, 270 degrees firing arc
- 6 CIWS turrets, 4 dorsal, 1 on each wing, 270 degrees firing arc

### Design
Unfortunately, I went into this project without any planning and only wanted to create something simple that was reminscent of Starsector. As a result, I unfortunately had to rewrite the code several times.

The bulk of my efforts went into the targeting and turret code, as documented below. 
<br>

#### Implementations of Turrets and Targeting
Targeting is handled by maintaining lists of both allied and enemy ships and strikecraft (includes missiles). Whenever a ship/missile/strikecraft is instantiated they are added to the list. On destruction, they are removed.

The first iteration was simple; at this point I had only drawn up the destroyer, CIWS and turret sprites. I made each ship add itself to a list on instantiation. I made one player controlled destroyer and one enemy destroyer to test this. In this iteration, everything was simple. Targeting was based on the closest target, and turrets utilised the built in `Quartenion.RotateTowards()` function to rotate towards the target. This was a simple firing solution that I implemented quickly.

However, I wanted to implement firing arcs. I started off by first limiting the angle of fire as seen here:
```
    // Case where the firing arc is unbroken
    bool isWithinArcCaseOne = (minAngle < maxAngle && angle > minAngle && angle < maxAngle);
    // Case where the firing arc is broken, ie the 0 degree point cuts the arc somewhere
    bool isWithinArcCaseTwo = (minAngle > maxAngle && (angle < maxAngle || angle > minAngle));
```

These angles are shown here:
```
    minAngle = absoluteAngle(transform.parent.rotation.eulerAngles.z + rotationOffset - firingArc / 2);
    maxAngle = absoluteAngle(transform.parent.rotation.eulerAngles.z + rotationOffset + firingArc / 2);
    currAngle = absoluteAngle(transform.rotation.eulerAngles.z);
```

- `rotationoffset` is specified so the arc of fire of non forward-facing turrets is correct.
- `transform.parent.rotation.eulerAngles.z` refers to the ship angle, and `transform.rotation.eulerAngles.z` refers to the turret angle.
- `currAngle` is the angle of the turret relative to the ship. 
- `absoluteAngle()` is a function that converts any angle to a positive one, bounded by 0 to 360 degrees exclusive.

However, while I could easily stop the turrets from firing illegally, I did not want them turning into the restricted angles as well. I simply bounded the angle, but realised that I could no longer use `RotateTowards()` as it takes the shortest path, which means that the turrets would jam at the edge of the bounded angle, instead of rotating the other far around. 

I then implemented a system with 10 cases.
- If the firing arc is unbroken by the zero angle, then it is trivial to turn towards the target angle (2 cases).
- If the firing arc is broken, there are 2 cases, either the target angle is in the left half or the right half:
    - If the current angle is in the other half, then we turn towards the half with the target angle (2 cases).
    - If the current angle is in the same half as the target angle, then we trivially turn it towards the target angle (2 cases).
- This makes for a total of 2 * 4 + 2 = 10 cases.

My final code directly modifies the `transform.rotation` of the turrets with each update. I am not sure if this is how `RotateTowards` operates under the hood, nor am I sure if this is a good practice, but it seemingly works.

After taking too long to do this (many hours), I rewrote the targeting code as well, since it was too simple and did not make much sense due to two cases:
 1. It did not work well for the CIWS. They were turning huge angles just to fire at the closest target. They were spending more time turning than firing and were in general terrible at doing their job.
 2. For the fixed cannons of the cruiser, this meant that even though there was another alien ship within their firing arc, they would not fire since their target was the closer alien ship that was not in the arc.

So I rewrote the targeting algorithm to prioritise the target that requires the smallest angle to reach, then by distance. This also means that turret behaviour is more organic, since different turrets are more likely to fire at different targets. The previous implementation resulted in most turrets on the same ship firing on a single target. 

For the longest time, I thought that this was the solution, however when making the video for submission I realised that it still was not working correctly. Under certain conditions, the turrets were still turning through their blind spots. I then rewrote the turret code again (this time in a far shorter timeframe).  This is the gist of the code:

- If the current angle is less than the target angle, check to see if the blind spot is within both angles. 
    - If so, we turn away from it (decrement angle).
    - Else we turn towards the target angle (increment angle).
- Else, again we check if the blind spot is between both angles.
    - If so, we turn away from it (increment angle).
    - Else we turn towards the target angle (decrement angle).

This seems to be an even simpler and more intuitive solution to the problem, but again I am not 100% sure if it works correctly. In the demo video it seems to hold up under some testing.
<br>

#### Implementations of Other Stuff
The ship AI is simple, they reuse the targeting code and move and rotate towards the target, stopping when they are within their engagement range. Strikecraft AI reuses the ship AI, except they have an engagement range of 0. Missile AI is similar, but they only acquire the target once, at instantiation. The player ship turns and accelerates towards the marker. Past 90 degrees, it reverses instead.

I utilised heavy particle effects for thrusters, projectiles, explosions and impacts. This could be done with sprites and would be less demanding than particle systems, but my ship sprites were already terrible enough. Also, I quite like the muzzle flash effect that can be achieved with particles.

Projectile impacts and destroyed ships are their own prefabs with particle systems that are instantiated on the projectile and ship destruction respectively.

Whether or not an object is an enemy or ally is propagated down from the ship to its turrets or then to its projectiles, or to its hangar then strikecraft then projectiles.

Similarly, I reused the projectiles quite a bit. The projectile/missile prefabs do not have their own hardcoded stats; I chose to propagate the stats down from the turrets instead.

I implemented a rudimentary carrier system where strikecraft spawn on a timer from a ship's hangar bays, up to maximum squadron size. On destruction of a strikecraft, it calls its parent hangar's `craftDestroyed()` function (assuming it still exists) which updates the current number of strikecraft, allowing a new one to spawn.

There are eight layers used to handle collisions. The ship layer collides with enemy projectiles and missiles, projectiles collide with enemy ships, missiles and strikecraft, missiles collide with enemy ships, projectiles and strikecraft, and strikecraft collide with enemy projectiles and missiles (although no missiles currently target strikecraft).

### Credits:
[Krita](https://krita.org/en/) for sprites
<br>
[jsfxr](https://sfxr.me/) for sounds 



