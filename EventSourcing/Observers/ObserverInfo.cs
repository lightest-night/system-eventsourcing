using System.Reflection;

namespace LightestNight.System.EventSourcing.Observers
{
    public class ObserverInfo
    {
        /// <summary>
        /// The method to be invoked by the observer
        /// </summary>
        public MethodInfo MethodInfo { get; set; }
        
        /// <summary>
        /// The observer itself
        /// </summary>
        public object Observer { get; set; }
    }
}