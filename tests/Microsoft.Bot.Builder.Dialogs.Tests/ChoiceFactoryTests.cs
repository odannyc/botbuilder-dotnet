﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Bot.Builder.Dialogs.Tests
{
    [TestClass]
    [TestCategory("Prompts")]
    [TestCategory("Choice Tests")]
    public class ChoiceFactoryTests
    {
        private static List<Choice> colorChoices = new List<Choice> { new Choice("red"), new Choice("green"), new Choice("blue") };
        private static List<Choice> extraChoices = new List<Choice> { new Choice("red"), new Choice("green"), new Choice("blue"), new Choice("alpha") };

        [TestMethod]
        public void ShouldRenderChoicesInline()
        {
            var activity = ChoiceFactory.Inline(colorChoices, "select from:");
            Assert.AreEqual("select from: (1) red, (2) green, or (3) blue", activity.Text);
        }

        [TestMethod]
        public void ShouldRenderChoicesAsAList()
        {
            var activity = ChoiceFactory.List(colorChoices, "select from:");
            Assert.AreEqual("select from:\n\n   1. red\n   2. green\n   3. blue", activity.Text);
        }

        [TestMethod]
        public void ShouldRenderUnincludedNumbersChoicesAsAList()
        {
            var activity = ChoiceFactory.List(colorChoices, "select from:", options: new ChoiceFactoryOptions { IncludeNumbers = false });
            Assert.AreEqual("select from:\n\n   - red\n   - green\n   - blue", activity.Text);
        }

        [TestMethod]
        public void ShouldRenderChoicesAsSuggestedActions()
        {
            var activity = ChoiceFactory.SuggestedAction(colorChoices, "select from:");
            Assert.AreEqual("select from:", activity.Text);
            Assert.IsNotNull(activity.SuggestedActions);
            Assert.AreEqual(3, activity.SuggestedActions.Actions.Count);
            Assert.AreEqual("imBack", activity.SuggestedActions.Actions[0].Type);
            Assert.AreEqual("red", activity.SuggestedActions.Actions[0].Value);
            Assert.AreEqual("red", activity.SuggestedActions.Actions[0].Title);
            Assert.AreEqual("imBack", activity.SuggestedActions.Actions[1].Type);
            Assert.AreEqual("green", activity.SuggestedActions.Actions[1].Value);
            Assert.AreEqual("green", activity.SuggestedActions.Actions[1].Title);
            Assert.AreEqual("imBack", activity.SuggestedActions.Actions[2].Type);
            Assert.AreEqual("blue", activity.SuggestedActions.Actions[2].Value);
            Assert.AreEqual("blue", activity.SuggestedActions.Actions[2].Title);
        }

        [TestMethod]
        public void ShouldAutomaticallyChooseRenderStyleBasedOnChannelType()
        {
            var activity = ChoiceFactory.ForChannel(Channels.Emulator, colorChoices, "select from:");
            Assert.AreEqual("select from:", activity.Text);
            Assert.IsNotNull(activity.SuggestedActions);
            Assert.AreEqual(3, activity.SuggestedActions.Actions.Count);
            Assert.AreEqual("imBack", activity.SuggestedActions.Actions[0].Type);
            Assert.AreEqual("red", activity.SuggestedActions.Actions[0].Value);
            Assert.AreEqual("red", activity.SuggestedActions.Actions[0].Title);
            Assert.AreEqual("imBack", activity.SuggestedActions.Actions[1].Type);
            Assert.AreEqual("green", activity.SuggestedActions.Actions[1].Value);
            Assert.AreEqual("green", activity.SuggestedActions.Actions[1].Title);
            Assert.AreEqual("imBack", activity.SuggestedActions.Actions[2].Type);
            Assert.AreEqual("blue", activity.SuggestedActions.Actions[2].Value);
            Assert.AreEqual("blue", activity.SuggestedActions.Actions[2].Title);
        }

        [TestMethod]
        public void ShouldChooseCorrectStylesForCortana()
        {
            var activity = ChoiceFactory.ForChannel(Channels.Cortana, colorChoices, "select from:");

            Assert.IsNotNull(activity.Attachments);

            var heroCard = (HeroCard)activity.Attachments.First().Content;

            Assert.AreEqual(3, heroCard.Buttons.Count);
            Assert.AreEqual("imBack", heroCard.Buttons[0].Type);
            Assert.AreEqual("red", heroCard.Buttons[0].Value);
            Assert.AreEqual("red", heroCard.Buttons[0].Title);
            Assert.AreEqual("imBack", heroCard.Buttons[1].Type);
            Assert.AreEqual("green", heroCard.Buttons[1].Value);
            Assert.AreEqual("green", heroCard.Buttons[1].Title);
            Assert.AreEqual("imBack", heroCard.Buttons[2].Type);
            Assert.AreEqual("blue", heroCard.Buttons[2].Value);
            Assert.AreEqual("blue", heroCard.Buttons[2].Title);
        }

        [TestMethod]
        public void ShouldChooseCorrectStylesForTeams()
        {
            var activity = ChoiceFactory.ForChannel(Channels.Msteams, colorChoices, "select from:");

            Assert.IsNotNull(activity.Attachments);

            var heroCard = (HeroCard)activity.Attachments.First().Content;

            Assert.AreEqual(3, heroCard.Buttons.Count);
            Assert.AreEqual("imBack", heroCard.Buttons[0].Type);
            Assert.AreEqual("red", heroCard.Buttons[0].Value);
            Assert.AreEqual("red", heroCard.Buttons[0].Title);
            Assert.AreEqual("imBack", heroCard.Buttons[1].Type);
            Assert.AreEqual("green", heroCard.Buttons[1].Value);
            Assert.AreEqual("green", heroCard.Buttons[1].Title);
            Assert.AreEqual("imBack", heroCard.Buttons[2].Type);
            Assert.AreEqual("blue", heroCard.Buttons[2].Value);
            Assert.AreEqual("blue", heroCard.Buttons[2].Title);
        }
    }
}
