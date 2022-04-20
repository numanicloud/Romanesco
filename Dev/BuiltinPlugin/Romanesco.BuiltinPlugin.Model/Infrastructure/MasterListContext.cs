using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
    public class MasterListContext
    {
        private readonly Dictionary<string, MasterList> masterDictionary = new Dictionary<string, MasterList>();
        private readonly Subject<string> onKeyAdded = new Subject<string>();

        public IReadOnlyDictionary<string, MasterList> Masters => masterDictionary;

        public IObservable<string> OnKeyAdded => onKeyAdded;

		public MasterListContext(IApiFactory api)
		{
			api.OnProjectChanged.Subscribe(x => ClearList());
		}

        public void ClearList()
        {
            masterDictionary.Clear();
        }

        public void AddList(MasterList master)
        {
            if (!Masters.ContainsKey(master.MasterName))
            {
                masterDictionary[master.MasterName] = master;
                onKeyAdded.OnNext(master.MasterName);
            }
            else
            {
                throw new InvalidOperationException($"Duplicated masterName {master.MasterName}");
            }
        }
    }
}
