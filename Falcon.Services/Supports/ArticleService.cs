using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Falcon.Data.Repository;
using Falcon.Data.Domain;
using Dapper;
using Falcon.Common;

namespace Falcon.Services.Supports
{
    public class ArticleService : BaseService, IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            this._articleRepository = articleRepository;
        }

        public Article GetArticleById(int id)
        {
            var article = _articleRepository.Table.SingleOrDefault(c => c.Id == id);
            if(article != null)
            {
                string sqlDetail = @"select ArticleId, Content from ArticleContents where ArticleId = @ArticleId";
                var conn = GetOpenConnection();
                ArticleContent articleContent = conn.Query<ArticleContent>(sqlDetail, new { ArticleId = id }).SingleOrDefault();
                if(articleContent != null)
                {
                    article.Content = articleContent.Content;
                }
            }
            return article;
        }

        public ArticleContent GetContentById(int id)
        {
            string sqlDetail = @"select ArticleId, Content from ArticleContents where ArticleId = @ArticleId";
            var conn = GetOpenConnection();
            ArticleContent articleContent = conn.Query<ArticleContent>(sqlDetail, new { ArticleId = id }).SingleOrDefault();
            return articleContent;
        }

        public int AddArticle(Article article)
        {
            _articleRepository.Add(article);

            string sql = @"insert into ArticleContents(ArticleId, Content) values (@ArticleId, @Content)";
            var conn = GetOpenConnection();
            conn.Execute(sql, new { ArticleId = article.Id, Content = article.Content });
            conn.Close();
            return article.Id;
        }

        public void UpdateArticle(Article article)
        {
            string sql = @"update ArticleContents set Content = @Content where ArticleId = @ArticleId";
            var conn = GetOpenConnection();
            conn.Execute(sql, new { ArticleId = article.Id, Content = article.Content });
            conn.Close();
            _articleRepository.SubmitChanges();
        }

        public void RemoveArticle(Article article)
        {
            _articleRepository.Remove(article);
        }

        //public IEnumerable<Article> GetAllArticle(int page, int pageSize)
        //{
        //    return _articleRepository.Table.OrderBy(c => c.OrderNumber).OrderByDescending(c => c.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        //}

        public int GetAllArticleCount()
        {
            return _articleRepository.Table.Count();
        }

        //public IEnumerable<Article> GetArticleByCateId(int cateId, int page, int pageSize)
        //{
        //    return _articleRepository.Table.Where(c => c.CategoryId == cateId && c.Status == true).OrderBy(c => c.OrderNumber).OrderByDescending(c => c.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        //}

        public int GetArticleByCateIdCount(int cateId)
        {
            return _articleRepository.Table.Where(c => c.CategoryId == cateId && c.Status == true).Count();
        }

        public IEnumerable<Article> SearchArticleByTitle(string keyword)
        {
            return _articleRepository.Table.Where(a => a.Status == true && a.Name.Contains(keyword)).ToList();
        }

        public IEnumerable<Article> FilterArticle(string keyword, bool? status, int categoryId, int pageIndex, int pageSize)
        {
            return _articleRepository.QuerySP<Article>("Articles_FilterAdmin", new { Keyword = keyword, CategoryId = categoryId, Status = status, PageIndex = pageIndex, PageSize = pageSize }).ToList();
        }

        public int FilterArticleCount(string keyword, bool? status, int categoryId)
        {
            var t = new
            {
                Keyword = keyword,
                CategoryId = categoryId,
                Status = status
            };
            return _articleRepository.QuerySP<SP_CountResult>("Articles_FilterAdmin_Count", new { Keyword = keyword, CategoryId = categoryId, Status = status }).FirstOrDefault().Total;
        }

        public Article GetArticleByAlias(string alias, int categoryId)
        {
            return _articleRepository.Table.Where(c => c.Alias == alias && c.CategoryId == categoryId).FirstOrDefault();
        }

        //public List<Article> GetByCategoryType(int categoryType)
        //{
        //    return _articleRepository.Table.Where(a => a.CategoryTypeId == categoryType && a.Status == true).OrderBy(c => c.OrderNumber).ToList();
        //}

        //public List<Article> GetAllByCategoryId(int categoryId)
        //{
        //    return _articleRepository.Table.Where(a => a.CategoryId == categoryId && a.Status == true).OrderBy(c => c.OrderNumber).ToList();
        //}

        public Article GetFirstByCategory(int categoryId)
        {
            return _articleRepository.Table.FirstOrDefault(a => a.CategoryId == categoryId && a.Status == true);
        }

        public List<Article> GetByTypeId(int typeId, int pageIndex, int pageSize)
        {
            return _articleRepository.QuerySP<Article>("SupportArticles_GetByTypeId", new { TypeId = typeId, PageIndex = pageIndex, PageSize = pageSize }).ToList();
        }

        public int GetByTypeIdCount(int typeId)
        {
            return _articleRepository.QuerySP<int>("SupportArticles_GetByTypeId_Count", new { TypeId = typeId }).SingleOrDefault();
        }

        public List<Article> SearchArticle(string keyword, int typeId, int pageIndex, int pageSize)
        {
            return _articleRepository.QuerySP<Article>("Articles_Search_Filter", new { Keyword = keyword, TypeId = typeId, PageIndex = pageIndex, PageSize = pageSize }).ToList();
        }

        public int SearchArticleCount(string keyword, int typeId)
        {
            return (int)_articleRepository.QuerySP<SP_CountResult>("Articles_Search_FilterCount", new { Keyword = keyword, TypeId = typeId }).FirstOrDefault().Total;
        }

        public IEnumerable<Article> GetAllArticle(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetArticleByCateId(int cateId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetByCategoryType(int categoryType)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetAllByCategoryId(int categoryId)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetBySupportTypeId(int supportTypeId)
        {
            throw new NotImplementedException();
        }

      

      

        //public List<Article> GetBySupportTypeId(int supportTypeId)
        //{
        //    return _articleRepository.Table.Where(a => a.TypeId == supportTypeId).OrderBy(c => c.OrderNumber).ToList();
        //}
    }
}