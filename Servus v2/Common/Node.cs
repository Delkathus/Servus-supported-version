using System.Xml.Serialization;

namespace Servus_v2.Common
{
    public class Node
    {
        [XmlAttribute("h")]
        public float H { get; set; }

        public Position position { get; set; }

        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float Z { get; set; }

        public Zone Zone { get; set; }
    }
}