using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    /// <summary>Component allowing spawning of GameObjects based on erlier instantiated pool of objects.</summary>
    public class ObjectPool : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private string poolID;

        [SerializeField]
        private GameObject pooledGameObject = null;

        [SerializeField]
        private int _minPoolSize = 0;
        /// <summary>Minimal size of this ObjectPool.</summary>
        public int MinPoolSize { get { return _minPoolSize; } }

        [SerializeField]
        private bool _limitPoolSize = false;
        /// <summary>Should pool size be limited to max pool size.</summary>
        public bool LimitPoolSize { get { return _limitPoolSize; } }

        [SerializeField]
        private int _maxPoolSize = 1;
        /// <summary>Maximal size of this ObjectPool.</summary>
        public int MaxPoolSize { get { return _maxPoolSize; } }

        [SerializeField]
        private int _poolExpandingSize = 0;
        /// <summary>Expanding size of this ObjectPool. Pool expands whenever all created instances are in use.</summary>
        public int PoolExpandingSize { get { return _poolExpandingSize; } }

        [Tooltip("Should pool destroy exceeding unused objects.")]
        [SerializeField]
        private bool _isPoolShrinkingEnabled = false;
        /// <summary>Should this ObjectPool shrink when there are too many unused instances.</summary>
        public bool IsPoolShrinkingEnabled { get { return _isPoolShrinkingEnabled; } }

        [Tooltip("Pool will shrink always when unused objects count is greater than sum of PoolExpandingSize and PoolShrinkingSize.")]
        [SerializeField]
        private int _poolShrinkingSize = 0;
        /// <summary>Shrinking size of this ObjectPool. Pool shrinks whenever unused object count is greater than sum of PoolExpandingSize and PoolShrinkingSize.</summary>
        public int PoolShrinkingSize { get { return _poolShrinkingSize; } }

        private int _poolSize = 0;
        /// <summary>Current size of this ObjectPool.</summary>
        public int PoolSize
        {
            get
            {
                return _poolSize;
            }
        }

        /// <summary>Currently used instances count.</summary>
        public int UsedObjectCount
        {
            get
            {
                return usedInstances != null ? usedInstances.Count : 0;
            }
        }

        /// <summary>Currently unused instances count.</summary>
        public int UnUsedObjectCount
        {
            get
            {
                return unUsedInstances != null ? unUsedInstances.Count : 0;
            }
        }

        private HashSet<PooledObject> usedInstances;
        private Queue<PooledObject> unUsedInstances;

        private bool isPoolInitialized = false;

        #endregion Variables

        #region MonoBehaviour Methods

        private void Awake()
        {
            if (pooledGameObject != null)
            {
                Initialize(pooledGameObject);
                ObjectPoolManager.Instance?.RegisterPool(poolID, this);
            }
        }

        private void OnValidate()
        {
            if (_minPoolSize < 0) _minPoolSize = 0;
            if (_maxPoolSize < _minPoolSize) _maxPoolSize = _minPoolSize;
            if (_poolExpandingSize < 0) _poolExpandingSize = 0;
            if (_poolShrinkingSize <= 0) _poolShrinkingSize = 1;
        }

        #endregion MonoBehaviour Methods

        #region Methods

        /// <summary>Initializes this ObjectPool and sets pooled GameObject.</summary>
        /// <param name="pooledGameObject">GameObject to be set as pooled by this ObjectPool.</param>
        public void Initialize(GameObject pooledGameObject)
        {
            if (pooledGameObject == null)
            {
                Debug.LogWarning($"Cannot initialize ObjectPool ({gameObject.name}) with none pooled GameObject.");
                return;
            }
            this.pooledGameObject = pooledGameObject;

            usedInstances = new HashSet<PooledObject>();
            unUsedInstances = new Queue<PooledObject>();
            for (int i = 0; i < _minPoolSize; ++i)
            {
                GameObject spawnedObject = Instantiate(this.pooledGameObject, transform);
                spawnedObject.SetActive(false);
                PooledObject pooledObject = spawnedObject.AddComponent<PooledObject>();
                pooledObject.Pool = this;
                pooledObject.hideFlags = HideFlags.NotEditable;
                unUsedInstances.Enqueue(pooledObject);
            }
            _poolSize = unUsedInstances.Count + usedInstances.Count;
            isPoolInitialized = true;
        }

        private bool Expand()
        {
            int newInstancesCount = _poolExpandingSize;
            if (_limitPoolSize && (_poolSize + _poolExpandingSize < _maxPoolSize))
            {
                newInstancesCount = _maxPoolSize - _poolSize;
            }
            if (newInstancesCount <= 0) return false;
            for (int i = 0; i < newInstancesCount; ++i)
            {
                GameObject spawnedObject = Instantiate(pooledGameObject, transform);
                spawnedObject.SetActive(false);
                PooledObject pooledObject = spawnedObject.AddComponent<PooledObject>();
                pooledObject.Pool = this;
                unUsedInstances.Enqueue(pooledObject);
            }
            _poolSize = unUsedInstances.Count + usedInstances.Count;
            return true;
        }

        /// <summary>Spawns GameObject from this ObjectPool.</summary>
        /// <returns>Spawned GameObject.</returns>
        public GameObject Spawn()
        {
            return Spawn(Vector3.zero, Quaternion.identity, null, true);
        }

        /// <summary>Spawns GameObject from this ObjectPool and sets its parent.</summary>
        /// <param name="parent">Parent Transform.</param>
        /// <returns>Spawned GameObject.</returns>
        public GameObject Spawn(Transform parent)
        {
            return Spawn(Vector3.zero, Quaternion.identity, parent, false);
        }

        /// <summary>Spawns GameObject from this ObjectPool at specified position.</summary>
        /// <param name="position">World space position where object should be spawned.</param>
        /// <returns>Spawned GameObject.</returns>
        public GameObject Spawn(Vector3 position)
        {
            return Spawn(position, Quaternion.identity, null, true);
        }

        /// <summary>Spawns GameObject from this ObjectPool at specified position, with specified rotation, and with optional specified parent.</summary>
        /// <param name="position">World space position where object should be spawned.</param>
        /// <param name="rotation">World space rotation of spawned object.</param>
        /// <param name="parent">Parent Transform.</param>
        /// <returns>Spawned GameObject.</returns>
        public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Spawn(position, rotation, parent, true);
        }

        private GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            if (!isPoolInitialized)
            {
                Debug.LogWarning($"ObjectPool ({gameObject.name}) wasn't initialized so it cannot be used yet.");
                return null;
            }

            if (unUsedInstances.Count <= 0)
            {
                bool expanded = Expand();
                if (!expanded)
                {
                    Debug.LogWarning($"ObjectPool ({gameObject.name}) reached maximum pool size. Consider increasing <i>maxPoolSize</i> for this ObjectPool.");
                    return null;
                }
            }

            PooledObject pooledObject = unUsedInstances.Dequeue();

            usedInstances.Add(pooledObject);

            if (worldPositionStays)
            {
                pooledObject.gameObject.transform.SetParent(parent);
                pooledObject.gameObject.transform.position = position;
                pooledObject.gameObject.transform.rotation = rotation;
            }
            else
            {
                pooledObject.gameObject.transform.SetParent(parent, false);
            }

            pooledObject.gameObject.SetActive(true);
            return pooledObject.gameObject;
        }

        /// <summary>Despawns pooled GameObject and returns it into the pool.</summary>
        /// <param name="pooledGameObject">Pooled GameObject to despawn.</param>
        public void Despawn(GameObject pooledGameObject)
        {
            if(pooledGameObject.TryGetComponent(out PooledObject pooledObject))
            {
                Despawn(pooledObject);
            }
        }

        /// <summary>Despawns pooledObject and returns it into the pool.</summary>
        /// <param name="pooledObject">PooledObject to despawn.</param>
        internal void Despawn(PooledObject pooledObject)
        {
            if (!isPoolInitialized)
            {
                Debug.LogWarning($"ObjectPool ({gameObject.name}) wasn't initialized so it cannot be used yet.");
                return;
            }

            pooledObject.gameObject.SetActive(false);
            pooledObject.Restore();
            usedInstances.Remove(pooledObject);
            unUsedInstances.Enqueue(pooledObject);

            if (_isPoolShrinkingEnabled)
            {
                if (_poolSize > MinPoolSize)
                {
                    if (UnUsedObjectCount > _poolExpandingSize + _poolShrinkingSize)
                    {
                        for (int i = 0; i < _poolShrinkingSize; ++i)
                        {
                            Destroy(unUsedInstances.Dequeue().gameObject);
                        }
                        _poolSize = unUsedInstances.Count + usedInstances.Count;
                    }
                }
            }
        }

        #endregion Methods
    }
}