using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using DtronixModeler.Generator.Ddl;
using DtronixModeler.Generator.Output;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DtronixModel.Tools
{
    public class CompileDDL : Task
    {
        private string _baseOutputPath;

        [Required]
        public string[] DdlPaths { get; set; }   
        
        [Required]
        public string IntermediateOutputPath { get; set; }  
        
        [Required]
        public string ProjectDirectory { get; set; }

        [Output]
        public string[] ExpectedOutputs { get; set; }

        public override bool Execute()
        {
            _baseOutputPath = Path.Combine(IntermediateOutputPath, "DtronixModel");

            if (!Directory.Exists(_baseOutputPath))
                Directory.CreateDirectory(_baseOutputPath);

            var output = new List<string>();

            foreach (var ddlPath in DdlPaths)
                output.Add(CreateDdlOutput(ddlPath));

            ExpectedOutputs = output.ToArray();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            return true;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debugger.Launch();
        }

        public string CreateDdlOutput(string ddlPath)
        {
            var ddlFullPath = Path.Combine(ProjectDirectory, ddlPath);
            var database = Database.LoadFromFile(ddlFullPath);

            var ddlName = Path.GetFileNameWithoutExtension(ddlPath);
            var outDir = Path.Combine(_baseOutputPath, database.Namespace);

            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            var csFile = Path.Combine(outDir, ddlName + ".cs");

            var generator = new CSharpCodeGenerator(database);

            using (var writer = File.CreateText(csFile))
            {
                writer.Write(generator.TransformText());
            }

            return csFile;
        }
    }
}
