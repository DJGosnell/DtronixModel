using DtronixModeler.Generator.Output;
using DtronixModeler.Ddl;
using DtronixModeler.Generator.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DtronixModeler.Xaml;
using System.Windows;
using System.Runtime.InteropServices;
using NDesk.Options;
using DtronixModeler.Generator.MySqlMwb;

namespace DtronixModeler.Generator
{
    class Program
    {

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static bool CommandlineVisible { get; private set; }

        [STAThread]
        static void Main(string[] args)
        {
            // Get and hide the console and show the UI if there are no arguments passed.
            if (args.Length == 0 ||
                (args.Length == 1 && Path.GetExtension(args[0]) == ".ddl"))
            {
                CommandlineHide();
                // Show the UI and start the main loop.
                var app = new Application();

                if (args.Length == 1)
                {
                    app.Run(new MainWindow(args[0]));
                }
                else
                {
                    app.Run(new MainWindow());
                }
                return;
            }

            CommandOptions options = new CommandOptions(args, Console.Out);

            // Verify that the parsing was successful.
            if (options.ParseSuccess == false)
            {
                return;
            }

            try
            {
                Task.Run(async () => {
                    await ExecuteOptions(options, null);
                }).Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error in executing specified options. Error:");
                foreach (var ex in e.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }

        public static void CommandlineShow()
        {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
            CommandlineVisible = true;
        }

        public static void CommandlineHide()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
            CommandlineVisible = false;
        }

        public static async Task ExecuteOptions(CommandOptions options, Database input_database)
        {
            DdlGenerator generator = null;
            TypeTransformer type_transformer = new SqliteTypeTransformer();
            bool rooted_path = Path.IsPathRooted(options.SqlOutput);

            if (options.InputType == CommandOptions.InType.Ddl)
            {
                if (input_database == null)
                {
                    try
                    {
                        using (FileStream stream = new FileStream(options.Input, FileMode.Open))
                        {
                            var serializer = new XmlSerializer(typeof(Database));
                            input_database = (Database)serializer.Deserialize(stream);
                        }
                    }
                    catch (Exception e)
                    {
                        writeLineColor("Could not open input DDL file at '" + options.Input + "'.", ConsoleColor.Red);
                        writeLineColor(e.ToString(), ConsoleColor.Red);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Using database passed.");
                }

            }
            else if (options.InputType == CommandOptions.InType.DatabaseSqlite)
            {
                generator = new SqliteDdlGenerator(@"Data Source=" + options.Input + ";Version=3;");

                input_database = await generator.GenerateDdl();
            }
            else if (options.InputType == CommandOptions.InType.Mwb || options.InputType == CommandOptions.InType.MwbXml)
            {

                if (File.Exists(options.Input) == false)
                {
                    throw new OptionException("MWB file '" + options.Input + "' specified does not exist.", "input");
                }


                generator = new MySqlMwbDdlGenerator(options.Input, (options.InputType == CommandOptions.InType.MwbXml));

                input_database = await generator.GenerateDdl();
            }

            // Ensure that the base database is initialized.
            input_database.Initialize();

            // Overrides for database variables.
            if (string.IsNullOrWhiteSpace(options.Namespace) == false)
            {
                input_database.Namespace = options.Namespace;
            }

            if (string.IsNullOrWhiteSpace(options.ContextClass) == false)
            {
                input_database.ContextClass = options.ContextClass;
            }

            if(string.IsNullOrWhiteSpace(input_database.Namespace))
                throw new Exception("Namespace not set.");

            input_database.ImplementINotifyPropertyChanged = options.NotifyPropertyChanged;
            input_database.ImplementProtobufNetDataContracts = options.ProtobufDataContracts;
            input_database.ImplementMessagePackAttributes = options.MessagePackAttributes;
            input_database.ImplementSystemTextJsonAttributes = options.SystemTextJsonAttributes;
            input_database.ImplementDataContractMemberOrder = options.DataContractMemberOrder;
            input_database.ImplementDataContractMemberName = options.DataContractMemberName;
            input_database.ProtobufPackage = options.ProtobufPackage;

            // Output SQL file if required.
            if (options.SqlOutput != null)
            {
                var sql_code_writer = new SqlTableCreateGenerator(input_database, type_transformer);

                if (options.SqlOutput == "")
                {
                    options.SqlOutput = Path.GetFileNameWithoutExtension(input_database.Name);
                }

                if (Path.HasExtension(options.SqlOutput) == false)
                {
                    options.SqlOutput = Path.ChangeExtension(options.SqlOutput, ".sql");
                }
                using (var fs = new FileStream(options.SqlOutput, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(sql_code_writer.Generate());
                        sw.Flush();
                    }
                }
            }

            // Output code if required.
            if (options.CodeOutput != null)
            {
                if (options.CodeOutput == "")
                {
                    options.CodeOutput = Path.ChangeExtension(Path.GetFileNameWithoutExtension(input_database.Name), ".cs");
                }

                if (Path.HasExtension(options.CodeOutput) == false)
                {
                    options.CodeOutput = Path.ChangeExtension(options.CodeOutput, ".cs");
                }

                // Output code file if required.
                using (var fs = new FileStream(options.CodeOutput, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(new CSharpCodeGenerator(input_database).TransformText());
                        sw.Flush();

                    }
                }
            }

            // Output Ddl if required.
            if (options.DdlOutput != null)
            {
                if (options.DdlOutput == "")
                {
                    options.DdlOutput = Path.GetFileNameWithoutExtension(input_database.Name);
                }

                if (Path.HasExtension(options.DdlOutput) == false)
                {
                    options.DdlOutput = Path.ChangeExtension(options.DdlOutput, ".ddl");
                }

                // Output code file if required.
                using (var fs = new FileStream(options.DdlOutput, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(Database));
                    serializer.Serialize(fs, input_database);
                }
            }

            // Output Ddl if required.
            if (options.ProtobufOutput != null)
            {
                if (options.ProtobufOutput == "")
                {
                    options.ProtobufOutput = Path.GetFileNameWithoutExtension(input_database.Name);
                }

                if (Path.HasExtension(options.ProtobufOutput) == false)
                {
                    options.ProtobufOutput = Path.ChangeExtension(options.ProtobufOutput, ".proto");
                }
                
                // Output code file if required.
                using var fs = new FileStream(options.ProtobufOutput, FileMode.Create);
                using var sw = new StreamWriter(fs);

                sw.Write(new ProtobufGenerator(input_database).Generate());
                sw.Flush();
            }
        }

        public static void writeColor(string text, ConsoleColor color)
        {
            var original_color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = original_color;
        }

        public static void writeLineColor(string text, ConsoleColor color)
        {
            var original_color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = original_color;
        }
    }
}
