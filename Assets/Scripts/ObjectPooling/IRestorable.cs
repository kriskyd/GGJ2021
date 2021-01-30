namespace ObjectPooling
{
    /// <summary>Interface used to mark Component as restorable. It allows the ObjectPool to restore object to its original state on despawn.</summary>
    interface IRestorable
    {
        /// <summary>Restores object to its original state.</summary>
        void Restore();
    }
}