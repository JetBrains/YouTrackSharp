using System;
using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.CommonControls;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.TreeModels;
using JetBrains.UI.PopupWindowManager;
using JetBrains.UI.TreeView;
using JetBrains.UI.WindowManagement;

namespace YouTrackPowerToy
{
    public class YouTrackTreeViewController : TreeModelBrowserDescriptor
    {
        readonly TreeModel _treeModel;
        readonly YouTrackIssuePresenter _presenter;
        readonly ISolution _solution;

        public YouTrackTreeViewController(ISolution solution, TreeSimpleModel treeModel)
            : base(solution)
        {
            _treeModel  = treeModel;
            
            _solution = solution;
            _presenter = new YouTrackIssuePresenter();
        }

        public override ISolution Solution
        {
            get { return _solution;  }
        }

        public override string Title
        {
            get { return "YouTrack Search Results"; }
        }

        public override TreeModel Model
        {
            get { return _treeModel; }
        }

        public override StructuredPresenter<TreeModelNode, IPresentableItem> Presenter
        {
            get { return _presenter; }
        }

    }
}