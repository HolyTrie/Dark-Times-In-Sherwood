/*
This file is meant to expose common interfaces or abastract classes to all entities, hence the 'funny' I in the class name.
*/

public abstract class GhostBehaviour //walls platforms player and enemies will need to know this exists!
{
    private bool _ghosted = false;
    public void TrySetGhostStatus() // be sure to put this in your Update() method!
    {
        if(_ghosted != GameManager.IsPlayerGhosted)
        {
            _ghosted = GameManager.IsPlayerGhosted;
            OnGhostStateSwitch();
        }
    }
    protected void OnGhostStateSwitch()
    {
        if(_ghosted)
        {
            OnGhostSet();
        }
        else
        {
            OnGhostUnset();
        }
    }

    protected abstract void OnGhostSet(); // these are very different per entity!
    protected abstract void OnGhostUnset(); // these are very different per entity!
}