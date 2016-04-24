using GalaSoft.MvvmLight;
using Outlook.Model.Helpers;
using SQLite;
using System;
using System.Collections.Generic;

namespace Outlook.Model
{
    public class Article : ViewModelBase
    {
        #region Constructor

        public Article()
        {
        }

        #endregion Constructor

        #region Properties
        private string _story;
        public string Story
        {
            get
            {
                return _story;
            }
            set
            {
                _story = value;
                RaisePropertyChanged("Story");
            }
        }

        private string _subStory;
        public string SubStory
        {
            get
            {
                return _subStory;
            }
            set
            {
                _subStory = value;
                RaisePropertyChanged("SubStory");
            }
        }

        private string _thumb;
        public string Thumb
        {
            get
            {
                return _thumb;
            }
            set
            {
                _thumb = value;
                RaisePropertyChanged("Thumb");
            }
        }

        private string _webURL;
        public string WebURL
        {
            get
            {
                return _webURL;
            }
            set
            {
                _webURL = value;
                RaisePropertyChanged("WebURL");
            }
        }

        private string _headLine;
        public string HeadLine
        {
            get
            {
                return _headLine;
            }
            set
            {
                _headLine = value;
                RaisePropertyChanged("HeadLine");
            }
        }

        private string _photoCaption;
        public string PhotoCaption
        {
            get
            {
                return _photoCaption;
            }
            set
            {
                _photoCaption = value;
                RaisePropertyChanged("PhotoCaption");
            }
        }

        private int _iD;
        [PrimaryKey]
        public int ID
        {
            get
            {
                return _iD;
            }
            set
            {
                _iD = value;
                RaisePropertyChanged("ID");
            }
        }
        private DateTime _articleDate;
        public DateTime ArticleDate
        {
            get
            {
                return _articleDate;
            }
            set
            {
                _articleDate = value;
                RaisePropertyChanged("ArticleDate");
            }
        }

        private string _category;
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }

        #endregion Properties
    }

    public class RootObject
    {
        public List<Article> NewsItem { get; set; }
    }
}