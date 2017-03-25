using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BHTree
{
    public abstract class Composite : Behaviour
    {
        protected List<IBehaviour> Children { get; set; }

        protected Composite()
        {
            Children = new List<IBehaviour>();
            BInitialize = () => { };
            BTerminate = status => { };
            BUpdate = () => BHStatus.Running;
        }

        public IBehaviour GetChild(int index)
        {
            return Children[index];
        }

        public int ChildCount()
        {
            return Children.Count;
        }

        public T AddBehaviour<T>() where T : class, IBehaviour, new()
        {
            var t = new T { BParent = this };
            Children.Add(t);
            return t;
        }
    }
}

