using UnityEngine;

namespace Script.UI
{
    [RequireComponent(typeof(Animator))]
    public abstract class IAPAnimateWindowController : MonoBehaviour
    {
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");
        private Animator _animator;
        
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            _animator.SetTrigger(Show);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }
        
        public void OnClosedAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}