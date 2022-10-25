using System;
using UnityEngine;

namespace Game
{
    public class HoldableElementTarget : InstantElement
    {
        public HoldableElement target;
        
        public void OnHold()
        {
            if (Protagonist.instance.holding == target)
            {
                Active = true;
            }
        }

        public void UpHold()
        {
            Active = false;
        }

        public void InteractTest()
        {
            Debug.Log("123");
        }
    }
}