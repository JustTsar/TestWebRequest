using SaveSystem;
using UnityEngine;

namespace GameLoader
{
    public class GameLoader : MonoBehaviour
    {
        private void Awake()
        {
            SaveController.TryLoad();
        }
    }
}