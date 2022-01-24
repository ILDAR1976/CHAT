using System;


namespace Chat
{
    class Vertex
    {
        public int Number { get; set; }
        public int Weight { get; set; }

        public Vertex(int number, int weight = 1)
        {
            Number = number;
            Weight = weight;
        }

        public override string ToString()
        {
            return Number.ToString();
        }

        public void SubWeight(int weight)
        {
            if ( Weight >= weight) Weight -= weight;
        }
    }
}
