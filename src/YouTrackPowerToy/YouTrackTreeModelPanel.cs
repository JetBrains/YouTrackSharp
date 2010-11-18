using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.CommonControls;
using JetBrains.IDE.TreeBrowser;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace YouTrackPowerToy
{
    public class YouTrackTreeModelPanel : TreeModelBrowserPanel
    {
        YouTrackIssueView _issueView;
        readonly YouTrackStatusPanel _statusPanel;
        YouTrackTreeViewController _controller;

        public YouTrackTreeModelPanel(YouTrackTreeViewController controller) : base(controller)
        {
            _controller = controller;
            _statusPanel = new YouTrackStatusPanel()
            {
                Dock = DockStyle.Top,
                Height = 10
            };
        }

        protected override void Dispose(bool disposing)
        {
        }

        protected override StructuredPresenter<TreeModelNode, IPresentableItem> GetPresenter()
        {
            return _controller.Presenter;
        }

        protected override IActionBar CreateActionBar(string actionId)
        {
            return ActionBarManager.Instance.CreateActionBar(actionId, this, true);
        }

        protected override TreeModelPresentableView CreateView(TreeModel model)
        {
            _issueView = new YouTrackIssueView(TreeModel, Descriptor);
            return _issueView;
        }

        protected override void InitializeCustomBar()
        {
            base.InitializeCustomBar();
            Controls.Add(_statusPanel);
        }

      
    }
}