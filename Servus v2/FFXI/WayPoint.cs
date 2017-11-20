using System.Xml.Serialization;
using FFACETools;

namespace Gambits.Model.FFXi
{
    public class WayPoint
    {
        [XmlIgnore]
        public FFACE.Position Poistion { get; set; }

        [XmlAttribute("zone")]
        public Zone Zone { get; set; }

        [XmlAttribute("x")]
        public float X { get; set; }

        [XmlAttribute("y")]
        public float Y { get; set; }

        [XmlAttribute("z")]
        public float Z { get; set; }
    }
}
