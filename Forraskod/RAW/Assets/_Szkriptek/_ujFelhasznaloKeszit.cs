using UnityEngine;
using UnityEngine.UI;

public class _ujFelhasznaloKeszit : MonoBehaviour
{

    #region VÁLTOZÓK
    public InputField nevBeText;
    public InputField korBeText;

    #endregion

    #region METÓDUSOK
    public void NevErtekValtozas()
    {
        if (nevBeText.text.Length >= 50)
        {
            nevBeText.interactable = false;
        }
    }
    public void KorErtekValtozas()
    {
        if (korBeText.text.Length > 3)
        {
            korBeText.interactable = false;
        }
    }
    #endregion
}
