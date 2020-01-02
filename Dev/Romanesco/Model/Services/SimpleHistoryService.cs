using Romanesco.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services
{
    class SimpleHistoryService : IProjectHistoryService
    {
        private readonly Project project;

        public SimpleHistoryService(Project project)
        {
            this.project = project;
        }

        public void Redo()
        {
            project.Context.CommandHistory.Redo();
        }

        public void Undo()
        {
            project.Context.CommandHistory.Undo();
        }
    }
}
