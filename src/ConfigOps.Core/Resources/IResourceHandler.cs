using System.Threading.Tasks;

namespace ConfigOps.Core.Resources
{
    internal interface IResourceHandler
    {
        bool CanDeserialize();

        /// <summary>
        /// Given a handler, this method will return the value of the deserialized resource.
        /// Note that this should be optional.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<object> DeserializeValue(ResourceExecutionContext context);

        /// <summary>
        /// Used to modify the operation before it's persisted. 
        /// A JSON patch set is expected to be returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> PreOperation(ResourceExecutionContext context);

        /// <summary>
        /// Used to validate the operation before it's persisted
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> FinalizeOperation(ResourceExecutionContext context);

        /// <summary>
        /// Any post operation that needs to be done after the operation is persisted.
        /// Only external system 'reactionary' calls should be made here.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> PostOperation(ResourceExecutionContext context);
    }
}
