using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Dependency {
    public static class ServiceLocator {
        private static readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        public static void RegisterSingleton<T>(T instance) where T : class {
            Assert.IsFalse(singletons.ContainsKey(instance.GetType()), $"Instance already registered for {instance.GetType ()}");

            singletons[instance.GetType()] = instance;
        }

        public static bool TryGetSingleton <T>(out T instance) where T : class {
            if (!singletons.ContainsKey (typeof (T))) {
                instance = null;
                return false;
            }

            instance = (T)singletons[typeof(T)];

            return true;
        }
    }
}

