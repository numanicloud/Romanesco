using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model
{
    class EditorContext
    {
        public Editor Editor { get; }
        public ObjectInterpreter Interpreter { get; set; }
        public IProjectSettingProvider SettingProvider { get; }

        public EditorContext(Editor editor, ObjectInterpreter interpreter, IProjectSettingProvider settingProvider)
        {
            Editor = editor;
            Interpreter = interpreter;
            SettingProvider = settingProvider;
        }
    }
}
