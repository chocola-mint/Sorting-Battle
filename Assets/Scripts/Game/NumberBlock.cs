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
        private GameGrid gameGrid;
        public GameTile gameTile { get; private set; }
        private int number = 0;
        private PulledFollow pulledFollow;
        private Animator animator;
        private Graphic[] graphics;
        private static class AnimState
        {
            public static readonly int Vanish = Animator.StringToHash(nameof(Vanish));
            public static readonly int PushUp = Animator.StringToHash(nameof(PushUp));
        }
        public void SetRandomNumber() => SetNumber(Random.Range(0, 100));
        public void SetNumber(int number) {
            if(this.number != number) 
                numberDisplay.text = number.ToString();
            this.number = number;
        }
        public int GetNumber() => number;
        public void ToTrash()
        {
            // TODO: Smooth animation?
            
        }
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
            animator = GetComponent<Animator>();
            SetRandomNumber();
            AutoResize();
            animator.Play(AnimState.PushUp);

            
            gameGrid = GetComponentInParent<GameGrid>();
            gameGrid.state.RegisterBlockCallbacks(gameTile.gridCoord, coord => {
                MoveTo(gameGrid.GetGameTile(coord));
            },
            OnRemove);
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
            DisableRaycast();
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
            EnableRaycast();
        }
        public Coroutine MoveTo(GameTile next)
        {
            // PromiseMoveTo(next).Start();
            StopAllCoroutines();
            return StartCoroutine(CoMoveTo(next));
        }
        public void FollowPointer(Vector2 pointerPosition)
        {
            // DisableRaycast();
            pulledFollow.enabled = true;
            pulledFollow.pullSource = gameTile.transform.position.MapToZPlane(transform.position.z);
            pulledFollow.followTarget = Camera.main.ScreenToWorldPoint(pointerPosition).MapToZPlane(transform.position.z);
        }
        public void StopFollowPointer()
        {
            // EnableRaycast();
            pulledFollow.followTarget = pulledFollow.pullSource;
        }
        private void DisableRaycast()
        {
            foreach(var graphic in graphics)
                graphic.raycastTarget = false;
        }
        private void EnableRaycast()
        {
            foreach(var graphic in graphics)
                graphic.raycastTarget = true;
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

        public void OnRemove()
        {
            // ! We CANNOT set parent to null here, because NumberBlock is an UI object.
            // ! It won't render if it's not under a Canvas.
            // ! It's okay because we will turn off its raycast targets.
            // transform.SetParent(null);
            StopAllCoroutines();
            DisableRaycast();
            animator.Play(AnimState.Vanish);
            StartCoroutine(animator.WaitUntilCurrentStateIsDone(() => {
                Destroy(gameObject);
            }));
        }
    }


}