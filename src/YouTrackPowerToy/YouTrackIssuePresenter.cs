using System;
using JetBrains.CommonControls;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;
using YouTrackSharp;

namespace YouTrackPowerToy
{
    public class YouTrackIssuePresenter: TreeModelBrowserPresenter 
    {
        protected override void PresentObject(object value, IPresentableItem item, TreeModelNode modelNode, PresentationState state)
        {
            item.RichText.Text = ((Issue) value).Summary;
        }
    }
}