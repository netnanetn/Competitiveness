using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Falcon.Configuration;
using System.Web;

namespace Falcon.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the Falcon engine.
    /// </summary>
    public class EngineContext
    {
        #region Initialization Methods
        /// <summary>Initializes a static instance of the Falcon factory.</summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        /// <param name="databaseIsInstalled">A value indicating whether database is installed</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static FalconEngine Initialize(bool forceRecreate)
        {            
            if (Singleton<FalconEngine>.Instance == null || forceRecreate)
            {
                Singleton<FalconEngine>.Instance = new FalconEngine();
                Singleton<FalconEngine>.Instance.Initialize();                
            }
            return Singleton<FalconEngine>.Instance;
        }

        /// <summary>Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.</summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        //public static void Replace(FalconEngine engine)
        //{
        //    Singleton<FalconEngine>.Instance = engine;
        //}
        
        #endregion

        /// <summary>Gets the singleton Falcon engine used to access Falcon services.</summary>
        public static FalconEngine Current
        {
            get
            {
                if (Singleton<FalconEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<FalconEngine>.Instance;
            }
        }
    }
}
