using Outlook.Model;
using OutlookIndia.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Outlook.Model
{
    public class DatabaseOperations
    {
        public const string LATESTNEWS = "LATEST NEWS";
        public const string PHOTOS = "PHOTOS";
        public const string BLOGS = "BLOGS";
        public const string BOOKS = "BOOKS";
        public const string ARTSANDENETERTAINEMNT = "ARTS-ENTERTAINMENT";
        public const string SPORTS = "SPORTS";
        public const string NATIONAL = "NATIONAL";
        public const string INTERNATIONAL = "INTERNATIONAL";
        public const string BUSINESS = "BUSINESS";

        private static DatabaseOperations _instance = null;

        private SQLite.SQLiteAsyncConnection _dbConnection;

        private DatabaseOperations()
        {
            _dbConnection = new SQLite.SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "articlesDb.sqlite"));
        }

        public static DatabaseOperations GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseOperations();
                TaskFactory factory = new TaskFactory();
                factory.StartNew(async () =>
                {
                    await _instance.CreateDatabaseIfNotCreated();
                });
            }
            return _instance;
        }

        public async Task AddCategoryAsync(Category categoryDetails)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryDetails.CategoryName.ToLower() select category;
                    if (allCategories != null && await allCategories.CountAsync() >= 1)
                    {
                        await _dbConnection.UpdateAsync(categoryDetails);
                    }
                    else
                    {
                        await _dbConnection.InsertAsync(categoryDetails);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task AddOrUpdateArticleAsync(Article articleToAddorUpdate)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<Article>() where article.ID == articleToAddorUpdate.ID select article;
                    if (allArticles != null && await allArticles.CountAsync() >= 1)
                    {
                        await _dbConnection.UpdateAsync(articleToAddorUpdate);
                    }
                    else
                    {
                        await _dbConnection.InsertAsync(articleToAddorUpdate);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteAllArticlesAsync()
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = await _dbConnection.Table<Article>().ToListAsync();
                    foreach (var article in allArticles)
                    {
                        await _dbConnection.DeleteAsync(article);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteArticleAsync(int Id, string category)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<Article>() where article.ID == Id select article;
                    if (allArticles != null && await allArticles.CountAsync() >= 1)
                    {
                        var temp = await allArticles.FirstOrDefaultAsync();
                        await _dbConnection.DeleteAsync(temp);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteArticlesAsync(string category)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<Article>() where article.Category.ToLower() == category.ToLower() select article;
                    foreach (var article in await allArticles.ToListAsync())
                    {
                        await _dbConnection.DeleteAsync(article);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteExpiredArticlesAsync()
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = await _dbConnection.Table<Article>().ToListAsync();

                    foreach (var article in allArticles)
                    {
                        if (article.ArticleDate != null)
                        {
                            if (article.Category.ToLower() == LATESTNEWS.ToLower() || article.Category.ToLower() == NATIONAL.ToLower() || article.Category.ToLower() == INTERNATIONAL.ToLower() || article.Category.ToLower() == PHOTOS.ToLower())
                            {
                                var diff = DateTime.Now - (DateTime)article.ArticleDate;
                                if (diff.TotalHours > 48)
                                {
                                    await _dbConnection.DeleteAsync(article);
                                }
                            }
                            else if (article.Category.ToLower() == SPORTS.ToLower())
                            {
                                var diff = DateTime.Now - (DateTime)article.ArticleDate;
                                if (diff.TotalHours > 72)
                                {
                                    await _dbConnection.DeleteAsync(article);
                                }
                            }
                            else
                            {
                                var diff = DateTime.Now - (DateTime)article.ArticleDate;
                                if (diff.TotalHours > 240)
                                {
                                    await _dbConnection.DeleteAsync(article);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }


        public async Task<Article> GetArticleAsync(int Id, string category = null)
        {
            Article tempArticle = null;
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<Article>() where article.ID == Id && article.Category.ToLower() == category.ToLower() select article;
                    tempArticle = await allArticles.FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return tempArticle;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            List<Category> list = null;
            if (_dbConnection != null)
            {
                try
                {
                    list = await _dbConnection.Table<Category>().ToListAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return list;
        }

        public async Task<List<Article>> GetCategoryArticlesAsync(string category)
        {
            List<Article> list = null;
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<Article>() where article.Category.ToLower() == category.ToLower() select article;
                    if (allArticles != null)
                    {
                        list = await allArticles.ToListAsync();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return list;
        }

        public async Task<Category> GetCategoryDetailAsync(string categoryName)
        {
            Category categoryObj = null;
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryName.ToLower() select category;
                    if (allCategories != null)
                    {
                        categoryObj = await allCategories.FirstOrDefaultAsync();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return categoryObj;
        }


        public bool IsCategoriesDbAlreadyCreated()
        {
            if (_dbConnection != null)
            {
                var temp = _dbConnection.Table<Category>();
                if (temp != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task UpdateCategoryAsync(Category categoryModel)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryModel.CategoryName.ToLower() select category;
                    if (allCategories != null)
                    {
                        Category tempCategory = await allCategories.FirstOrDefaultAsync();
                        if (tempCategory != null)
                        {
                            tempCategory.CategoryUrl = categoryModel.CategoryUrl;
                            tempCategory.LastUpdated = categoryModel.LastUpdated;
                            await _dbConnection.UpdateAsync(tempCategory);
                        }
                        else
                        {
                            await _dbConnection.InsertAsync(categoryModel);
                        }
                    }
                    else
                    {
                        await _dbConnection.InsertAsync(categoryModel);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get db connection");
            }
        }

        public async Task UpdateCategoryLastAccessedValueAsync(string categoryName, DateTime lastFetchedDateTime)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryName.ToLower() select category;
                    if (allCategories != null)
                    {
                        Category tempCategory = await allCategories.FirstOrDefaultAsync();
                        if (tempCategory != null)
                        {
                            tempCategory.LastUpdated = lastFetchedDateTime;
                            await _dbConnection.UpdateAsync(tempCategory);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get db connection");
            }
        }

        private async Task CreateDatabaseIfNotCreated()
        {
            try
            {
                await _dbConnection.CreateTablesAsync<Article, Category>();
            }
            catch (Exception)
            {
            }
        }
    }
}