﻿using System.Net;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;

namespace FubuMVC.Ajax.Tests
{
    [TestFixture]
    public class default_assets_are_included_in_the_bottle
    {
        [Test]
        public void fetch_fubu_continuations()
        {
            var scripts = SelfHostHarness.Endpoints.Get<AssetEndpoint>(x => x.get_continuations())
                    .StatusCodeShouldBe(HttpStatusCode.OK)
                    .ScriptNames();

            scripts.ShouldHaveTheSameElementsAs(
                "_content/scripts/fubucontinuations.js",
                "_content/scripts/fubucontinuations.diagnostics.js");
        }

        [Test]
        public void fetch_fubu_continuations_jquery()
        {
            var scripts = SelfHostHarness.Endpoints.Get<AssetEndpoint>(x => x.get_jquery_continuations())
                    .StatusCodeShouldBe(HttpStatusCode.OK)
                    .ScriptNames();

            scripts.ShouldHaveTheSameElementsAs(
                "_content/scripts/fubucontinuations.js",
                "_content/scripts/jquery-1.8.2.min.js",
                "_content/scripts/fubucontinuations.diagnostics.js",
                "_content/scripts/fubucontinuations.jquery.js");
        }

        [Test]
        public void fetch_jquery_continuations_forms()
        {
            SelfHostHarness.Endpoints.Get<AssetEndpoint>(x => x.get_jquery_continuations_forms())
                .StatusCodeShouldBe(HttpStatusCode.OK)
                .ScriptNames().ShouldHaveTheSameElementsAs(
                    "_content/scripts/fubucontinuations.js",
                    "_content/scripts/jquery-1.8.2.min.js",
                    "_content/scripts/fubucontinuations.diagnostics.js",
                    "_content/scripts/jquery.form.js",
                    "_content/scripts/fubucontinuations.jquery.js",
                    "_content/scripts/fubucontinuations.jquery.forms.js");
        }

        [Test]
        public void fetch_FormActivator()
        {
            SelfHostHarness.Endpoints.Get<AssetEndpoint>(x => x.get_FormActivator())
                .StatusCodeShouldBe(HttpStatusCode.OK)
                .ScriptNames().ShouldHaveTheSameElementsAs(
                    "_content/scripts/fubucontinuations.js",
                    "_content/scripts/jquery-1.8.2.min.js",
                    "_content/scripts/fubucontinuations.diagnostics.js",
                    "_content/scripts/jquery.form.js",
                    "_content/scripts/fubucontinuations.jquery.js",
                    "_content/scripts/fubucontinuations.jquery.forms.js",
                    "_content/scripts/FormActivator.js");
        }
    }

    public class AssetEndpoint
    {
        private readonly FubuHtmlDocument _document;

        public AssetEndpoint(FubuHtmlDocument document)
        {
            _document = document;
        }

        public HtmlDocument get_continuations()
        {
            _document.Asset("continuations");
            _document.WriteAssetsToHead();

            return _document;
        }

        public HtmlDocument get_jquery_continuations()
        {
            _document.Asset("jquerycontinuations");
            _document.WriteAssetsToHead();

            return _document;
        }

        public HtmlDocument get_jquery_continuations_forms()
        {
            _document.Asset("fubucontinuations.jquery.forms.js");
            _document.WriteAssetsToHead();

            return _document;
        }

        public HtmlDocument get_FormActivator()
        {
            _document.Asset("FormActivator.js");
            _document.WriteAssetsToHead();

            return _document;
        }
    }
}