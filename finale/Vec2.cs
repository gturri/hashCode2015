namespace finale
{
    public struct Vec2
    {
        public short R { get; private set; }
        public short C { get; private set; }

        public Vec2(short deltaRow, short deltaCol)
            : this()
        {
            R = deltaRow;
            C = deltaCol;
        }

        public override string ToString()
        {
            return R + "," +  C;
        }
    }
}
