using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Services.Supports
{
    public interface IArticleService : IService
	{
        Article GetArticleById(int Id);
        int AddArticle(Article entry);
        void UpdateArticle(Article entry);
        void RemoveArticle(Article entry);
        IEnumerable<Article> GetAllArticle(int page, int pageSize);
        int GetAllArticleCount();
        IEnumerable<Article> GetArticleByCateId(int cateId, int page, int pageSize);
        int GetArticleByCateIdCount(int cateId);
        IEnumerable<Article> SearchArticleByTitle(string keyword);

        IEnumerable<Article> FilterArticle(string keyword, bool? status, int categoryId, int pageIndex, int pageSize);
        int FilterArticleCount(string keyword, bool? status, int categoryId);
        Article GetArticleByAlias(string alias, int categoryId);

        List<Article> GetByCategoryType(int categoryType);

        List<Article> GetAllByCategoryId(int categoryId);
        Article GetFirstByCategory(int categoryId);

        List<Article> GetByTypeId(int typeId, int pageIndex, int pageSize);
        int GetByTypeIdCount(int typeId);
        List<Article> SearchArticle(string keyword, int typeId, int pageIndex, int pageSize);
        int SearchArticleCount(string keyword, int typeId);
        List<Article> GetBySupportTypeId(int supportTypeId);
        ArticleContent GetContentById(int id);
	}
}