using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;

namespace Romanesco.Model.Infrastructure
{
    public class MasterListContext
    {
        private readonly Dictionary<string, MasterList> masterDictionary = new Dictionary<string, MasterList>();
        private readonly Subject<string> onKeyAdded = new Subject<string>();

        public IReadOnlyDictionary<string, MasterList> Masters => masterDictionary;

        public IObservable<string> OnKeyAdded => onKeyAdded;

        public void ClearList()
        {
            masterDictionary.Clear();
        }

        public void AddList(MasterList master)
        {
            if (!Masters.ContainsKey(master.MasterName))
            {
                masterDictionary[master.MasterName] = master;
            }
            else
            {
                throw new InvalidOperationException($"Duplicated masterName {master.MasterName}");
            }
        }
    }
}
