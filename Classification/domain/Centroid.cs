namespace Classification.domain
{
    public class Centroid
    {
        public Centroid(double[] position)
        {
            Position = position;
        }

        public Centroid(double[] position, string label)
        {
            Position = position;
            Label = label;
        }

        public int Dimensions { get => Position.Length; }
        public double[] Position { get; set; }
        public string Label { get; set; }
    }
}
