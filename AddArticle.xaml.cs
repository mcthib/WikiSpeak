using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

namespace WikiSpeak
{
    public partial class AddArticle : PhoneApplicationPage
    {
        public AddArticle()
        {
            InitializeComponent();

            this.DataContext = App.AddArticleViewModel;
        }

        private void Language_Click(object sender, EventArgs e)
        {

        }

        private void SearchButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SearchForArticlesAsync(SearchTerm.Text, "en");
        }

        private async Task SearchForArticlesAsync(string searchTerm, string language)
        {
            App.AddArticleViewModel.Articles.Clear();
            WikipediaHelper.ArticleTitleAndUrl[] articles = await WikipediaHelper.SearchForArticlesAsync(searchTerm, language, 10);
            foreach (WikipediaHelper.ArticleTitleAndUrl article in articles)
            {
                App.AddArticleViewModel.Articles.Add(
                    new ViewModels.ArticleViewModel()
                    {
                        Article = new Article(article)
                    });
            }
        }

        private void ArticleUserControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ArticleUserControl newSelection = sender as ArticleUserControl;

            if (newSelection != null)
            {
                // Get the list of items in the LongListSelector
                List<UserControl> listItems = new List<UserControl>();
                MainPage.GetItemsRecursive<UserControl>(ArticleList, ref listItems);

                // Iterate through the list items. Select / unselect as appropriate
                foreach (UserControl listItem in listItems)
                {
                    bool result = VisualStateManager.GoToState(listItem, listItem == newSelection ? "Selected" : "Normal", true);
                }
            }
        }

        private void ArticleUserControl_Add(object sender, EventArgs e)
        {

        }

    }
}