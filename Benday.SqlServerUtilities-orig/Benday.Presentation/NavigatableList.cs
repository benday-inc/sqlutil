using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Presentation
{
    public class NavigatableList<T> : List<T>
    {
        public bool IsAtFirst
        {
            get
            {
                if (CurrentIndex == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public bool IsAtLast
        {
            get
            {
                if ((CurrentIndex + 1) >= this.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private int _CurrentIndex;
        public int CurrentIndex
        {
            get
            {
                if (Count == 0)
                {
                    _CurrentIndex = -1;    
                }
                else if (_CurrentIndex < 0)
                {
                    _CurrentIndex = -1;
                }
                else if (_CurrentIndex >= Count)
                {
                    _CurrentIndex = Count - 1;
                }

                return _CurrentIndex;
            }
            private set
            {
                _CurrentIndex = value;
            }
        }

        public T Value
        {
            get
            {
                if (CurrentIndex == -1)
                {
                    return default(T);
                }
                else
                {
                    return this[CurrentIndex];    
                }                
            }            
        }

        public void Next()
        {
            CurrentIndex++;
        }

        public void Previous()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
            }
        }

        public void MoveToFirst()
        {
            CurrentIndex = 0;
        }

        public void MoveToLast()
        {
            var proposedIndex = Count - 1;

            if (proposedIndex < 0)
            {
                proposedIndex = 0;
            }

            CurrentIndex = proposedIndex;
        }
    }
}
