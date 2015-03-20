﻿using RoytinBlog.Core;
using RoytinBlog.Core.ViewProjections.Home;
using Nancy;

namespace RoytinBlog.Modules
{
    public class FrontModule : BaseNancyModule
    {
        protected IViewProjectionFactory _viewFactory;

        public FrontModule(IViewProjectionFactory viewFactory)
        {
            _viewFactory = viewFactory;

            After.AddItemToEndOfPipeline(SetRecentBlogPosts);
            After.AddItemToEndOfPipeline(SetTagCloud);
        }

        private void SetTagCloud(NancyContext obj)
        {
            ViewBag.TagCould =
                _viewFactory.Get<TagCloudBindingModel, TagCloudViewModel>(new TagCloudBindingModel() { Threshold = 2 });
        }

        private void SetRecentBlogPosts(NancyContext obj)
        {
            ViewBag.Recent =
                _viewFactory.Get<RecentBlogPostSummaryBindingModel, RecentBlogPostSummaryViewModel>(
                    new RecentBlogPostSummaryBindingModel { Page = 10 });
        }
    }
}