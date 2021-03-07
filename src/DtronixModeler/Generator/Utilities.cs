using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Xml.Serialization;

namespace DtronixModeler.Generator
{
    public static class Utilities
    {

        public static void addDbParameter(DbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            command.Parameters.Add(param);
        }


        public static string XmlSerializeObject(object element)
        {
            XmlSerializer serializer = new XmlSerializer(element.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, element);
                return writer.ToString();
            }
        }

        public static T XmlDeserializeString<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                try
                {
                    return (T)serializer.Deserialize(reader);
                }
                catch (Exception)
                {
                    return default(T);
                }

            }
        }

        public static void BindChangedCollection<T>(ObservableCollection<T> collection, NotifyCollectionChangedEventHandler collection_changed, PropertyChangedEventHandler property_changed) where T : INotifyPropertyChanged
        {
            collection.CollectionChanged += (coll_sender, coll_e) => {
                if (collection_changed != null)
                {
                    collection_changed(coll_sender, coll_e);
                }
                if (coll_e.Action == NotifyCollectionChangedAction.Add)
                {

                    // If we add a new property, add a new property changed event to it.
                    foreach (T item in coll_e.NewItems)
                    {
                        if (property_changed != null)
                        {
                            item.PropertyChanged += property_changed;
                        }
                    }
                }
            };

            foreach (T item in collection)
            {
                if (property_changed != null)
                {
                    item.PropertyChanged += property_changed;
                }
            }
        }
    }
}
