using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//thanks to this guy -> https://www.youtube.com/watch?v=sUvwKH7qyQQ&ab_channel=Comp-3Interactive //
public class StaminaBar : MonoBehaviour
{
    [SerializeField] public Slider staminaBar;

    private int maxStamina = 100;
    private int currentStamina;
    private bool _canUseStamina;
    public bool canUseStamina { get { return _canUseStamina; } }

    private WaitForSeconds regenTimer = new WaitForSeconds(0.1f);

    private Coroutine regen;

    public static StaminaBar instance; // used for later scenes

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
        _canUseStamina = true;
    }

    public void UseStamina(int amount)
    {
        if (currentStamina - amount > 0)
        {
            _canUseStamina = true;
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null) //if we're already regenerating stamina, no need to use coroutnie untill its done.
                StopCoroutine(regen);

            if (currentStamina - amount < 0) // case where players tries to double jump 
                _canUseStamina = false;

            regen = StartCoroutine(RegenStamina(amount));

        }
        else
        {
            _canUseStamina = false;
            Debug.Log("Not Enough Stamina...");
        }
    }

    private IEnumerator RegenStamina(int amount)
    {
        yield return new WaitForSeconds(2); // wait 2 seconds untill regen after used

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTimer;

            if (currentStamina > amount) // case wher player choses to jump with not full stamina
                _canUseStamina = true;

        }
        regen = null;
    }

}
