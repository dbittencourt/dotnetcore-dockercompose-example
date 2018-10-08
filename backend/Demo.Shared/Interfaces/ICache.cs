using System;
using System.Collections.Generic;

namespace Demo.Shared.Interfaces
{
    public interface ICache
    {
        void SetValue(string key, object obj);
        void SetValue(string key, object obj, int ttl);
        T GetValue<T>(string key);
        T GetValue<T>(string key, Func<T> defaultValueGetter);
        T GetValue<T>(string key, Func<T> defaultValueGetter, int ttl);
        IEnumerable<T> GetValues<T>(string keyPattern);
        void RemoveValue(string key);
        bool IsKeyPresent(string key);
        void Clear(string type);
    }
}
