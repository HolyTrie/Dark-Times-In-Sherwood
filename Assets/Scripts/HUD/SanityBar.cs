using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//thanks to this guy -> https://www.youtube.com/watch?v=sUvwKH7qyQQ&ab_channel=Comp-3Interactive //
public class SanityBar : MonoBehaviour
{
    [SerializeField] public Slider sanityBar;

    private int maxSanity = 100;
    private int currentSanity;
    private bool _canUseSanity;
    public bool canUseSanity { get { return _canUseSanity; } }

    private WaitForSeconds regenTimer = new WaitForSeconds(0.1f);
    private WaitForSeconds sanityUseTimer = new WaitForSeconds(0.1f);
    private Coroutine regen;
    private Coroutine deplete;

    public static SanityBar instance; // used for later scenes

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentSanity = maxSanity;
        sanityBar.maxValue = maxSanity;
        sanityBar.value = maxSanity;
        _canUseSanity = true;
    }

    public void UseSanity(int amount)
    {
        deplete = StartCoroutine(UseSanityCallback(amount));
    }
    private IEnumerator UseSanityCallback(int amount)
    {
        while (currentSanity - amount > 0)
        {
            _canUseSanity = true;
            currentSanity -= amount;
            sanityBar.value = currentSanity;
            Debug.Log("Used Sanity");
            yield return sanityUseTimer;

            if (currentSanity - amount < 0 && GameManager.IsPlayerGhosted) //if sanity is below 0, player shoud die or something
            {
                _canUseSanity = false;
                Debug.Log("Not Enough Sanity... YOU ARE DEAD.");
            }
            if (!GameManager.IsPlayerGhosted)
            {
                deplete = null;
                break;
            }
        }
    }

    public void RegenSanity(int amount)
    {
        regen = StartCoroutine(RegenSanityCallback(amount));
    }

    private IEnumerator RegenSanityCallback(int amount)
    {
        while (currentSanity < maxSanity)
        {
            if (!GameManager.IsPlayerGhosted)
            {
                currentSanity += maxSanity / 100;
                sanityBar.value = currentSanity;
                yield return regenTimer;
            }

            if (currentSanity > amount && GameManager.IsPlayerGhosted) // case wher player choses to use ghosted behavior again and sanity is not full
                break;
        }
        regen = null;
    }
}
