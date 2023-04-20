using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Dependency {
    public static class ServiceLocator {
        private static readonly Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        public static void RegisterSingleton<T>(T instance) where T : class {
            var typeOfInstance = instance.GetType();

            Assert.IsFalse(singletons.ContainsKey(typeOfInstance), 
                $"Instance already registered for {typeOfInstance}");

            singletons[typeOfInstance] = instance;
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

