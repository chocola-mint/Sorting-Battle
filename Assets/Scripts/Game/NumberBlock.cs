using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SortGame
{
    public class NumberBlock : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text numberDisplay;
        public GameTile gameTile { get; private set; }
        private int number = 0;
        public void SetNumber(int number) {
            if(this.number != number) 
                numberDisplay.text = number.ToString();
            this.number = number;
        }
        public int GetNumber() => number;
        private void Reset() 
        {
            numberDisplay = GetComponentInChildren<TMP_Text>();
            AutoResize();
        }
        private void Awake() 
        {
            gameTile = GetComponentInParent<GameTile>();
            SetNumber(Random.Range(0, 100));
            AutoResize();
        }
        public void AutoResize()
        {
            gameTile = GetComponentInParent<GameTile>();
            if(!gameTile) return;
            var parentGroup = GetComponentInParent<HorizontalOrVerticalLayoutGroup>();
            if(parentGroup)
            {
                var size = (gameTile.transform as RectTransform).sizeDelta;
                var spacing = parentGroup.spacing;
                GetComponent<RectTransform>().sizeDelta = size;
                GetComponent<RectTransform>().anchorMin 
                = GetComponent<RectTransform>().anchorMax = Vector2.one * 0.5f;
                foreach(var textMesh in GetComponentsInChildren<TMP_Text>())
                {
                    textMesh.rectTransform.sizeDelta = new Vector2(size.x - spacing, size.x - spacing);
                    textMesh.rectTransform.pivot = Vector2.one * 0.5f;
                    
                }
            }
        }
        public void MoveTo(GameTile next)
        {
            gameTile = next;
            transform.SetParent(next.transform);
        }
        // Start is called before the first frame update
        void Start()
        {
            numberDisplay.text = number.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }


}