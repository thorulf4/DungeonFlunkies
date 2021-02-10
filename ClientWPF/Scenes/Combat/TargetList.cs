using ClientWPF.Utils.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Scenes.Combat
{
    public class TargetList : IList<Target>
    {
        private List<Target> targets = new List<Target>();

        public IReadOnlyList<Target> Targets { get { return targets.AsReadOnly(); } }

        public event EventHandler<Target> OnTargetSelected;

        public RelayCommand TargetSelected
        {
            get
            {
                return new RelayCommand(o =>
                {
                    OnTargetSelected?.Invoke(this, (Target)o);
                });
            }
        }

        public Target this[int index] { get{ return targets[index]; } set => targets[index] = value; }

        public int Count => targets.Count;

        public bool IsReadOnly => false;

        public void Add(Target item)
        {
            targets.Add(item);
        }

        public void Clear()
        {
            targets.Clear();
        }

        public bool Contains(Target item)
        {
            return targets.Contains(item);
        }

        public void CopyTo(Target[] array, int arrayIndex)
        {
            targets.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Target> GetEnumerator()
        {
            return targets.GetEnumerator();
        }

        public int IndexOf(Target item)
        {
            return targets.IndexOf(item);
        }

        public void Insert(int index, Target item)
        {
            targets.Insert(index, item);
        }

        public bool Remove(Target item)
        {
            return targets.Remove(item);
        }

        public void RemoveAt(int index)
        {
            targets.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return targets.GetEnumerator();
        }
    }
}
