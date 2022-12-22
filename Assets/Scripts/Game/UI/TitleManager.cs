using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SortGame.UI
{
    /// <summary>
    /// Manages title UI flow by inheriting from <see cref="UIManager"></see>.
    /// </summary>
    public class TitleManager : UIManager
    {
        [SerializeField] private Button returnButton;
        private new void Awake() 
        {
            base.Awake();
            returnButton.onClick.AddListener(returnable.Return);
        }
    }
}
