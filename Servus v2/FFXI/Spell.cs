namespace Servus_v2.FFXi
{
    public class Spell
    {
        public int id { get; set; }
        public string type { get; set; }
        public int element { get; set; }
        public int targets { get; set; }
        public int skill { get; set; }
        public int mp_cost { get; set; }
        public float cast_time { get; set; }
        public int recast { get; set; }
        public int recast_id { get; set; }
        public int icon_id_nq { get; set; }
        public int requirements { get; set; }
        public int range { get; set; }
        public string prefix { get; set; }
        public string en { get; set; }
    }
}