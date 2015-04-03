namespace finale
{
    public class Balloon
    {
        public int Altitude {get; set; }
        public Vec2 Location { get; set; }
		public bool IsDead { get; set; }

        public Balloon(int altitude, Vec2 location)
        {
            Altitude = altitude;
            Location = location;
        }
    }
}
