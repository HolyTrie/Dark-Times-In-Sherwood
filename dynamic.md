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

* **Enemies** </br>
```health``` </br>
Guards will have infinite health, they are required to be killed by stealth. </br>

Our calcualtaions were mostly trial and error, we started with some fixed numbers that sounded good to us, and then tried to play with them. </br>
Also we took some inspiration from other games to see how they defined their attribute system. </br>


## Main Items Placement
* ```tomes``` and ```artifacts``` from the vault, will be placed at the end of each level, after the boss fight. </br>
* ```chest buffs``` we will place near areas that the player will need to use - giving a boost jump for a place that the player cannot reach, giving him invisiblity to sneak past guards. </br>
* ```health potions``` dropped by guards randomly (at very low chance - (0.1)) - after a tedious fight (at some points, we will not give the player HP, to make it more challenging) </br>
* ```arrows``` - also dropped by guards but can be aquired from certains spots where the player might need them to solve a puzzle. </br>




