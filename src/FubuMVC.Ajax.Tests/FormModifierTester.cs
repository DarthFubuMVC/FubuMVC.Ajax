﻿using FubuCore;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Core.Urls;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Ajax.Tests
{
    [TestFixture]
    public class FormModifierTester
    {
        private BehaviorGraph theGraph;
        private IAssetRequirements theRequirements;

        [SetUp]
        public void SetUp()
        {
            theRequirements = MockRepository.GenerateStub<IAssetRequirements>();
            theGraph = BehaviorGraph.BuildFrom(x =>
            {
                x.Actions.IncludeType<FormModeEndpoint>();
                x.Import<AjaxExtensions>();
            });
        }

        private FormRequest requestFor<T>() where T : class, new()
        {
            var services = new InMemoryServiceLocator();
            services.Add<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));
            services.Add(theRequirements);
            services.Add<IChainUrlResolver>(new ChainUrlResolver(new StandInCurrentHttpRequest()));
            services.Add(new FormSettings());

            var request = new FormRequest(new ChainSearch {Type = typeof (T)}, new T());
            request.Attach(services);
            request.ReplaceTag(new FormTag("test"));

            return request;
        }

        [Test]
        public void modifies_the_form()
        {
            var theRequest = requestFor<AjaxTarget>();

            var modifier = new FormModifier();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.Attr("data-form-mode").ShouldEqual("ajax");
            theRequest.CurrentTag.HasClass("activated-form").ShouldBeTrue();
        }

        [Test]
        public void ignored_request()
        {
            var theRequest = requestFor<NoneTarget>();

            var modifier = new FormModifier();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.HasAttr("data-form-mode").ShouldBeFalse();
            theRequest.CurrentTag.HasClass("activated-form").ShouldBeFalse();
        }

        [Test]
        public void writes_the_form_activator_requirement()
        {
            var theRequest = requestFor<AjaxTarget>();
            var modifier = new FormModifier();
            modifier.Modify(theRequest);

            theRequirements.AssertWasCalled(x => x.Require("FormActivator.js"));
        }
    }
}