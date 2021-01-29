using UnityEngine;

namespace ObjectPooling
{
    /// <summary>Component automatically added to GameObject created via ObjectPool Spawn method. Allows to despawn object and return it to correct pool.</summary>
    [DisallowMultipleComponent]
    internal class PooledObject : MonoBehaviour
    {
        private ObjectPool _pool;
        /// <summary>ObjectPool in which this object was created. Reference to pool is set by the pool itself and cannot be changed afterwards.</summary>
        public ObjectPool Pool
        {
            get { return _pool; }
            set
            {
                if (_pool == null) _pool = value;
            }
        }

        /// <summary>Despawns object and returns it into the pool.</summary>
        public void Despawn()
        {
            _pool.Despawn(this);
        }

        /// <summary>Restores all components implementing IRestorable interface in this object and all of its children.</summary>
        public void Restore()
        {
            foreach (IRestorable restorable in gameObject.GetComponentsInChildren<IRestorable>(true))
            {
                restorable.Restore();
            }
        }
    }
}