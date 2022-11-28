using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SortGame.Modifiers;
using ChocoUtil.Coroutines;
using static ChocoUtil.Algorithms.Ease;
namespace SortGame
{
    [RequireComponent(typeof(PulledFollow))]
    public class NumberBlock : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text numberDisplay;
        public GameTile gameTile { get; private set; }
        private int number = 0;
        private PulledFollow pulledFollow;
        private Graphic[] graphics;
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
            pulledFollow = gameObject.GetComponent<PulledFollow>();
            StopFollowPointer();
            graphics = GetComponentsInChildren<Graphic>();
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
        public IEnumerator CoMoveTo(GameTile next)
        {
            foreach(var graphic in graphics)
                graphic.raycastTarget = false;
            gameTile = next;
            transform.SetParent(next.transform);
            pulledFollow.enabled = false;
            yield return Tween.MoveWorld(
                transform, 
                from: transform.position,
                to: next.transform.position,
                duration: 0.25f,
                ease: Linear,
                useUnscaledTime: false
            );
            pulledFollow.pullSource = next.transform.position;
            foreach(var graphic in graphics)
                graphic.raycastTarget = true;
        }
        public Coroutine MoveTo(GameTile next)
        {
            // PromiseMoveTo(next).Start();
            StopAllCoroutines();
            return StartCoroutine(CoMoveTo(next));
        }
        public void FollowPointer(Vector2 pointerPosition)
        {
            pulledFollow.enabled = true;
            pulledFollow.pullSource = gameTile.transform.position.MapToZPlane(transform.position.z);
            pulledFollow.followTarget = Camera.main.ScreenToWorldPoint(pointerPosition).MapToZPlane(transform.position.z);
        }
        public void StopFollowPointer()
        {
            pulledFollow.followTarget = pulledFollow.pullSource;
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