using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Base.Infrastructure.Interface
{
    public interface IUnitOfWork
    {
		/// <summary>
		/// 提交所有修改
		/// </summary>
		/// <returns></returns>
        int Commit();

		/// <summary>
		/// 异步提交所有修改
		/// </summary>
		/// <returns></returns>
        Task<int> CommitAsync();

		/// <summary>
		/// 执行SQL查询
		/// </summary>
		/// <param name="QueryString">SQL字符串</param>
		/// <param name="parameters">格式化参数</param>
		void Implement(string QueryString, params object[] parameters);

		/// <summary>
		/// 执行SQL查询,返回执行结果
		/// </summary>
		/// <typeparam name="TModel">格式化模型</typeparam>
		/// <param name="QueryString">SQL字符串</param>
		/// <param name="parameters">格式化参数</param>
		/// <returns></returns>
		IEnumerable<TModel> Implement<TModel>(string QueryString, params object[] parameters);

		
	}
}
