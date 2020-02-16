using UnityEngine;
using MazeGenerator;

namespace ResourcesManager
{
    public class ResourceManager
    {

        private readonly GameObject[] _floors;
        private readonly GameObject[] _walls;
        private readonly GameObject _endPoint;
        private readonly GameObject _secretRoom;
        private readonly GameObject _sectionPointsEnd;
        private readonly GameObject _sectionPointsStart;
        private readonly GameObject _startPoint;
        private readonly GameObject _playerSpawner;
        private readonly GameObject _player;

        public ResourceManager()
        {
            _floors=new GameObject[GenSettings.Sections+1];
            _walls = new GameObject[GenSettings.Sections + 1];

            for (int i = 1; i < _floors.Length; i++)
                _floors[i] = Resources.Load<GameObject>(GenSettings.FloorPath + i.ToString());

            for (int i = 1; i < _walls.Length; i++)
                _walls[i] = Resources.Load<GameObject>(GenSettings.WallPath + i.ToString());

            _endPoint= Resources.Load<GameObject>(GenSettings.EndPointPath);
            _secretRoom = Resources.Load<GameObject>(GenSettings.SecretRoomPath);
            _sectionPointsEnd = Resources.Load<GameObject>(GenSettings.SectionPointsEndPath);
            _sectionPointsStart = Resources.Load<GameObject>(GenSettings.SectionPointsStartPath);
            if (_sectionPointsStart == null)
                Debug.Log(GenSettings.SectionPointsStartPath);
            _startPoint = Resources.Load<GameObject>(GenSettings.StartPointPath);
            _playerSpawner = Resources.Load<GameObject>(GenSettings.PlayerSpawnerPath);
            _player= Resources.Load<GameObject>(GenSettings.PlayerPath);
        }

        public GameObject GetFloor(int section)
        {
            return _floors[section];
        }
        public GameObject GetWall(int section)
        {
            return _walls[section];
        }

        public GameObject GetEndPoint()
        {
            return _endPoint;
        }
        public GameObject GetSecretRoom()
        {
            return _secretRoom;
        }
        public GameObject GetSectionPointsEnd()
        {
            return _sectionPointsEnd;
        }
        public GameObject GetSectionPointsStart()
        {
            return _sectionPointsStart;
        }
        public GameObject GetStartPoint()
        {
            return _startPoint;
        }
        public GameObject GetPlayerSpawner()
        {
            return _playerSpawner;
        }
        public GameObject GetPlayer()
        {
            return _player;
        }
    }
}
