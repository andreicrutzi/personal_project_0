using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace decent
{
    public class XmlManager<T>
    {
        public Type Type;

        public T Load(string path)
        {
            T instance;
            using (TextReader reader = new StreamReader(path)) 
            {
                XmlSerializer xml = new (Type);
                instance = (T)xml.Deserialize(reader);

            }
            return instance;
        }

        public void Save(string path, object obj) 
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
