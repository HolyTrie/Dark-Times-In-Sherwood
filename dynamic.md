# Dynamic Elements

## Numeric Attributes

* ```Health``` - each entity will have diffrenet amount of health points. </br>
* ```Stamina``` - the energy of the player, it determines if he can jump, attack. </br>
* ```Sanity```  - this is a special ability the player unlocks, he can switch to ghost mode which will deplete sanity energy, as of health, once reaches 0, the player dies. he should carefully use it. </br>
* ```Attack``` - each entity will have diffrente attack points. </br>

### Numbers 
* **Player** </br>
```health,stamina,sanity``` for the player we are giving 100 as starting points. </br>
He will be able to increase those as he progresses further in the game. </br>
```attack```, at first the player will have 0 attack, he will need to get a weapon or upgrade his bow to add to it attack. at the prologue we will prevent him from attacking, but using stealth mechanics. </br>
each upgrade will give the player 10 hit points to his bow. </br>

* **Enemies** </br>
```health``` </br>
Guards will have infinite health, they are required to be killed by stealth. </br>
Mushrooms will have 100 health. </br>

Our calcualtaions were mostly trial and error, we started with some fixed numbers that sounded good to us, and then tried to play with them. </br>
Also we took some inspiration from other games to see how they defined their attribute system. </br>


## Main Items Placement
* ```tomes``` and ```artifacts``` from the vault, will be placed at the end of each level, after the boss fight. </br>
* ```chest buffs``` we will place near areas that the player will need to use - giving a boost jump for a place that the player cannot reach, giving him invisiblity to sneak past guards. </br>
* ```health potions``` dropped by guards randomly (at very low chance - (0.1)) - after a tedious fight (at some points, we will not give the player HP, to make it more challenging) </br>
* ```arrows``` - also dropped by guards but can be aquired from certains spots where the player might need them to solve a puzzle. </br>



## Object Behavior - TBD

## Econmoy 
Our game consist of a simple economy system - </br>
kill enemies, get coins, and with that coin you can purchase upgrades from the blacksmith. </br>

## Information 
* At any given time the player has most of the information available for him his current goal will be in his ```journal``` that can be accessed from the game. </br>
we will hide from him side objectives such as collecting hidden items in the vault or finding seceret rooms. </br>

* The game's perspetive is a 2D side-view. </br>
  we chose this in particular because we thought it fit well with our games design goal and timeframe that was alloted to us. </br>


## Control
The player game control is fast paced with a direct approach - what the player decides to do (in given boundaries), he can do and its not a turn-based game. </br>

## Choices
* The player will have to choose wether to keep helping the main protagonist in order to free himself or to help the town's people. </br>
* The player can decide wether we wants to upgrade his skills for easier combat, or chose a challenging course of action. </br>
