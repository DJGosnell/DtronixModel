using DtronixModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using DtronixModeler.Generator.MySql;
using System.IO.Compression;
using System.Xml.XPath;
using System.Xml;
using System.Threading.Tasks;

namespace DtronixModeler.Generator.MySqlMwb
{
    public class MySqlMwbDdlGenerator : DdlGenerator
    {

        List<Configuration> configurations = new List<Configuration>();
        private string mwb_file_location;

        private bool xml_format;

        public MySqlMwbDdlGenerator(string file_location, bool xml_format)
            : base(new MySqlTypeTransformer())
        {
            mwb_file_location = file_location;
            this.xml_format = xml_format;
        }

        public override async Task<Database> GenerateDdl()
        {
            await Task.Run(() =>
            {
                var mwb_xml = new XmlDocument();

                if (xml_format)
                {
                    mwb_xml.Load(mwb_file_location);
                }
                else
                {
                    using (var mwb_archive = ZipFile.OpenRead(mwb_file_location))
                    {
                        var zip_mwb = mwb_archive.GetEntry("document.mwb.xml");

                        if (zip_mwb == null)
                        {
                            throw new FileNotFoundException(
                                "Could not find the 'document.mwb.xml' inside the specified MWB file.");
                        }

                        using (var stream = zip_mwb.Open())
                        {
                            mwb_xml.Load(stream);
                        }

                    }
                }

                var xml_db_schema = mwb_xml.SelectSingleNode(@"/data
					/value[@struct-name='workbench.Document']
					/value[@content-struct-name='workbench.physical.Model']
					/value[@struct-name='workbench.physical.Model']
					/value[@struct-name='db.mysql.Catalog']
					/value[@content-struct-name='db.mysql.Schema']
					/value[@struct-name='db.mysql.Schema']");

                var xml_tables = xml_db_schema.SelectNodes(@"
					value[@content-struct-name='db.mysql.Table']
					/value");

                database.Name = xml_db_schema.SelectSingleNode("value[@key='name']").InnerText;

                database.TargetDb = DbProvider.MySQL;
                //database.Name = Path.GetFileNameWithoutExtension(connection.DataSource);

                // Get the tables.
                foreach (XmlNode xml_table in xml_tables)
                {
                    var name_node = xml_table.SelectSingleNode("value[@key=\"name\"]");
                    var table = new Table()
                    {
                        Name = name_node.InnerText
                    };

                    var xml_columns = xml_table.SelectNodes(@"value[@key='columns']/value");

                    foreach (XmlNode xml_column in xml_columns)
                    {
                        var column = new Column()
                        {
                            Name = xml_column.SelectSingleNode(@"value[@key='name']").InnerText,
                            Description = xml_column.SelectSingleNode(@"value[@key='comment']").InnerText,
                            DefaultValue = xml_column.SelectSingleNode(@"value[@key='defaultValue']").InnerText,
                        };

                        var flags = xml_column.SelectSingleNode(@"value[@key='flags']");

                        for (int i = 0; i < flags.ChildNodes.Count; i++)
                        {
                            switch (flags.ChildNodes[i].InnerText.ToLower())
                            {
                                case "unsigned":
                                    column.IsUnsigned = true;
                                    break;

                            }



                        }

                        var auto_increment = xml_column.SelectSingleNode(@"value[@key='autoIncrement']").InnerText;
                        column.IsAutoIncrement = (auto_increment == "0" || auto_increment == null) ? false : true;

                        var length = xml_column.SelectSingleNode(@"value[@key='length']").InnerText;
                        column.DbLength = (length == "-1") ? 0 : Convert.ToInt32(length);
                        if (column.DbLength != 0)
                            column.DbLengthSpecified = true;

                        var nullable = xml_column.SelectSingleNode(@"value[@key='isNotNull']").InnerText;
                        column.Nullable = (nullable == "0" || nullable == null) ? true : false;

                        var db_type = xml_column.SelectSingleNode(@"link[@struct-name='db.SimpleDatatype']");

                        if (db_type != null)
                        {
                            column.DbType = db_type.InnerText.Replace("com.mysql.rdbms.mysql.datatype.", "").ToUpper();

                        }
                        else
                        {
                            db_type = xml_column.SelectSingleNode(@"link[@struct-name='db.UserDatatype']");

                            if (db_type != null)
                            {
                                column.DbType = db_type.InnerText.Replace("com.mysql.rdbms.mysql.userdatatype.", "")
                                    .ToUpper();

                            }
                            else
                            {
                                throw new Exception("Unknown data type for column '" + column.Name + "' in table '" +
                                                    table.Name + "'.");
                            }
                        }

                        try
                        {
                            column.NetType = TypeTransformer.DbToNetType(column.DbType, column.IsUnsigned);
                        }
                        catch
                        {
                            throw new Exception("Unknown data type for column '" + column.Name + "' in table '" +
                                                table.Name + "'.");
                        }

                        column.IsPrimaryKey = column.IsAutoIncrement;


                        table.Column.Add(column);
                    }

                    database.Table.Add(table);
                }
            });

            return database;

        }
    }
}
