using Romanesco.Common.Model.Basics;
using Romanesco.Model.Services.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services.Save
{
	internal class ProjectSaveServiceFactory
	{
		private readonly IStateSerializer serializer;
		private IProjectSaveService? saveServiceCache;

		public ProjectSaveServiceFactory(IStateSerializer serializer)
		{
			this.serializer = serializer;
		}

		public IProjectSaveService Create(ProjectContext project)
		{
			return saveServiceCache ??= new WindowsSaveService(serializer, project);
		}
	}
}
