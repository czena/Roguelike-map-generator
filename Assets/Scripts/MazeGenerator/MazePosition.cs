
namespace MazeGenerator
{
    public class MazePosition 
    {
        public Point GlobalPosition { get; set; }
        public int Section { get; set; }
        public int Level { get; set; }
        public UnityEngine.GameObject Prefab { get; set; }

        public MazePosition(Point globalPosition, int section, int level, UnityEngine.GameObject prefab)
        {
            GlobalPosition = globalPosition;
            Section = section;
            Level = level;
            Prefab = prefab;
        }

    }
}
