namespace Servus_v2.FFXi
{
    public class Ability
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Element { get; set; }

        public int Icon_id { get; set; }
        public int Mp_cost { get; set; }

        public ushort Recast_id { get; set; }

        public int Targets { get; set; }

        public int Tp_cost { get; set; }

        public int Duration { get; set; }

        public int Range { get; set; }

        public string Prefix { get; set; }

        public string En { get; set; }

        public string Ja { get; set; }
    }
}