public abstract class GhostBehaviour //walls platforms player and enemies will need to know this exists!
{
    private bool _ghosted = false;
    public void TrySetGhostStatus() // be sure to put this in your Update() method!
    {
        if (_ghosted != GameManager.IsPlayerGhosted)
        {
            _ghosted = GameManager.IsPlayerGhosted;
            OnGhostStateSwitch();
        }
    }
    protected void OnGhostStateSwitch()
    {
        if (_ghosted)
        {
            OnGhostSet();
        }
        else
        {
            OnGhostUnset();
        }
    }

    // this state tells what happens when the entity is ghosted
    protected abstract void OnGhostSet(); 

    // this state tells what happens when the entity stops being ghosted
    protected abstract void OnGhostUnset(); 
}